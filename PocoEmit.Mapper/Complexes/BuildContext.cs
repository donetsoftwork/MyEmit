using PocoEmit.Builders;
using PocoEmit.Configuration;
using PocoEmit.Converters;
using PocoEmit.Resolves;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace PocoEmit.Complexes;

/// <summary>
/// 构建上下文构建器
/// </summary>
/// <param name="options"></param>
public class BuildContext(IMapperOptions options)
    : IBuildContext
    , IComplexBundle
{
    #region 配置
    private readonly IMapperOptions _options = options;
    private readonly ConstantExpression _mapper = Expression.Constant(options, typeof(IMapperOptions));
    private readonly Dictionary<PairTypeKey, ComplexBundle> _collections = [];
    private readonly Dictionary<PairTypeKey, ComplexBundle> _readies = [];
    private readonly Dictionary<PairTypeKey, LambdaExpression> _lambdas = [];
    private readonly Dictionary<PairTypeKey, LambdaExpression> _contextLambdas = [];
    private readonly List<ParameterExpression> _convertContexts = [];
    /// <summary>
    /// Emit配置
    /// </summary>
    public IMapperOptions Options
        => _options;
    /// <summary>
    /// 收集复杂类型
    /// </summary>
    public IEnumerable<ComplexBundle> Collections
        => _collections.Values;
    /// <summary>
    /// 已构建类型
    /// </summary>
    public IEnumerable<ComplexBundle> Readies
        => _readies.Values;
    /// <summary>
    /// 表达式
    /// </summary>
    public IEnumerable<LambdaExpression> Lambdas
        => _lambdas.Values;
    /// <summary>
    /// 上下文表达式
    /// </summary>
    public IEnumerable<LambdaExpression> ContextLambdas
        => _contextLambdas.Values;
    /// <summary>
    /// 环状引用上下文
    /// </summary>
    public List<ParameterExpression> ConvertContexts
        => _convertContexts;
    /// <inheritdoc />
    public BuildContext Context
        => this;
    #endregion
    #region IBuildContext
    /// <inheritdoc />
    public ComplexBundle GetBundle(PairTypeKey key)
    {
        if (_collections.TryGetValue(key, out var bundle) || _readies.TryGetValue(key, out bundle))
            return bundle;
        return null;
    }
    /// <inheritdoc />
    public Expression Call(LambdaExpression lambda, params Expression[] arguments)
        => _options.Call(lambda, arguments);
    /// <inheritdoc />
    public bool TryGetLambda(PairTypeKey key, out LambdaExpression lambda)
        => _lambdas.TryGetValue(key, out lambda);
    /// <inheritdoc />
    public bool TryGetContextLambda(PairTypeKey key, out LambdaExpression lambda)
        => _contextLambdas.TryGetValue(key, out lambda);
    /// <summary>
    /// 保存上下文表达式
    /// </summary>
    /// <param name="key"></param>
    /// <param name="lambda"></param>
    public bool SetContextLambda(PairTypeKey key, LambdaExpression lambda)
    {
        if(lambda.Parameters.Count == 2)
        {
            _contextLambdas[key] = lambda;
            return true;
        }
        return false;
    }
    #endregion
    #region bundle
    /// <summary>
    /// 获取复杂类型成员信息
    /// </summary>
    /// <param name="key"></param>
    /// <param name="converter"></param>
    /// <returns></returns>
    public ComplexBundle GetBundleOrCreate(PairTypeKey key, IEmitConverter converter)
    {
        // 忽略基础类型
        if (_options.CheckPrimitive(key.LeftType))
            return null;
        if (!_collections.TryGetValue(key, out var bundle))
            _collections.Add(key, bundle = new(this, key, converter));
        // 已编译忽略
        if (converter.Compiled)
            return null;
        return bundle;
    }
    /// <summary>
    /// 获取复杂类型成员信息
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TDest"></typeparam>
    /// <returns></returns>
    public ComplexBundle GetBundle<TSource, TDest>()
        => GetBundle(new PairTypeKey(typeof(TSource), typeof(TDest)));
    #region RemoveUsed
    /// <summary>
    /// 移除被调用
    /// </summary>
    /// <param name="key"></param>
    /// <param name="bundle"></param>
    private void RemoveUsed(PairTypeKey key, ComplexBundle bundle)
    {
        _collections.Remove(key);
        // 移除被调用,简化依赖关系
        foreach (var used in bundle.Uses.Keys)
            used.Remove(bundle);
    }
    #endregion
    /// <summary>
    /// 完成构建
    /// </summary>
    /// <param name="bundle"></param>
    private void OnFinish(ComplexBundle bundle)
    {
        var key = bundle.Key;
        RemoveUsed(key, bundle);
        _readies.Add(key, bundle);
    }
    #endregion
    /// <summary>
    /// 预览
    /// </summary>
    /// <param name="preview"></param>
    public void Visit(IComplexPreview preview)
    {
        foreach (var item in preview.Preview(this))
        {
            if (item is null)
                continue;
            var key = item.Key;
            if (_collections.ContainsKey(key))
                continue;
            _collections[key] = item;
        }
    }
    /// <summary>
    /// 准备处理
    /// </summary>
    public void Prepare()
    {
        // 检查循环引用
        if (CheckCircle())
        {
            // 处理上下文常量
            CheckConvertContextParameter();
        }
        // 预构建
        PrepareBuild();
        if (_collections.Count == 0)
            return;

        // 自循环构建
        BuildSelfCircle();
        // 互依赖构建
        BuildTwins();
        // 环形构建
        BuildCircle();
    }
    /// <summary>
    /// 检查循环引用
    /// </summary>
    public bool CheckCircle()
    {
        foreach (var bundle in _collections.Values)
            bundle.CheckIsCircle();
        bool hasCircle = false;
        foreach (var bundle in _collections.Values)
            hasCircle |= bundle.CheckHasCircle();
        return hasCircle;
    }
    /// <summary>
    /// 构建并准备
    /// </summary>
    /// <param name="options"></param>
    /// <param name="preview"></param>
    /// <returns></returns>
    public static BuildContext WithPrepare(IMapperOptions options, IComplexPreview preview)
    {
        var context = new BuildContext(options);
        context.Visit(preview);
        context.Prepare();
        return context;
    }
    /// <inheritdoc />
    public Expression InitContext(ParameterExpression context)
        => ConvertContext.InitParameter(context, _mapper);
    #region PrepareBuild
    /// <summary>
    /// 预构建
    /// </summary>
    private void PrepareBuild()
    {
        var list = _collections.Values
            .Where(bundle => bundle.Includes.Count == 0)
            .ToArray();
        if (list.Length == 0)
            return;
        foreach (var bundle in list)
        {
            // 构建完成移除被调用,简化依赖关系
            if (PrepareBuild(bundle))
                OnFinish(bundle);
            else
                return;
        }
        // 重复预构建,简化依赖关系
        PrepareBuild();
    }
    /// <summary>
    /// 预构建
    /// </summary>
    /// <param name="bundle"></param>
    private bool PrepareBuild(ComplexBundle bundle)
    {
        var key = bundle.Key;
        var lambda = BuildLambdaByConverter(key, bundle.Converter);
        if (lambda is null)
            return false;
        _lambdas[key] = lambda;
        return true;
    }
    /// <summary>
    /// 构建表达式
    /// </summary>
    /// <param name="key"></param>
    /// <param name="converter"></param>
    /// <returns></returns>
    private LambdaExpression BuildLambdaByConverter(PairTypeKey key, IEmitConverter converter)
    {
        LambdaExpression lambda = null;
        if (converter is IEmitComplexConverter complexConverter)
        {
            lambda = complexConverter.Build(this);
            // 更新转化器
            if (lambda is not null)
                _options.Set(key, new FuncConverter(_options, key, lambda));
        }
        else if (converter.Compiled && converter is IBuilder<LambdaExpression> builder)
        {

            lambda = builder.Build();
        }
        else if (converter is FuncConverter funcConverter)
        {
            lambda = funcConverter.Lambda;
        }
        return lambda;
    }
    #endregion
    #region ConvertContextParameters
    /// <summary>
    /// 处理上下文变量
    /// </summary>
    private void CheckConvertContextParameter()
    {
        _convertContexts.Add(ConvertContext.CreateParameter());
    }
    #endregion
    #region BuildSelfCircle
    /// <summary>
    /// 自循环构建
    /// </summary>
    public void BuildSelfCircle()
    {
        var list = _collections.Values
            .Where(bundle => bundle.Includes.Count == 1)
            .ToArray();
        if (list.Length == 0)
            return;
        foreach (var bundle in list)
        {
            if(BuildSelfCircle(bundle))
                OnFinish(bundle);
            else
                return;
        }
        // 重复自循环构建,简化依赖关系
        BuildSelfCircle();
    }
    /// <summary>
    /// 自循环构建
    /// </summary>
    /// <param name="bundle"></param>
    public bool BuildSelfCircle(ComplexBundle bundle)
    {
        var contextLambda = BuildCircleLambda(bundle);
        if (contextLambda is null)
            return false;
        var key = bundle.Key;
        if (SetContextLambda(key, contextLambda))
        {
            var func = Compiler.Instance.CompileDelegate(contextLambda);
            _options.Set(key, func);
        }
        return true;
    }
    #endregion
    #region BuildSelfCircle
    /// <summary>
    /// 构建互依赖
    /// </summary>
    public void BuildTwins()
    {

    }
    /// <summary>
    /// 构建互依赖
    /// </summary>
    /// <param name="front"></param>
    /// <param name="back"></param>
    public void BuildTwins(ComplexBundle front, ComplexBundle back)
    {

    }
    #endregion
    #region BuildCircle
    /// <summary>
    /// 环形构建
    /// </summary>
    public void BuildCircle()
    {
        var list = _collections.Values
            .Where(bundle => bundle.IsCircle)
            .ToArray();
        foreach (var bundle in list)
            BuildCircle(bundle);
    }
    /// <summary>
    /// 环形构建
    /// </summary>
    /// <param name="bundle"></param>
    public void BuildCircle(ComplexBundle bundle)
    {
        var contextLambda = BuildCircleLambda(bundle);
        if (contextLambda is null)
            return;
        var key = bundle.Key;
        if (SetContextLambda(key, contextLambda))
        {
            var func = Compiler.Instance.CompileDelegate(contextLambda);
            _options.Set(key, func);
        }
    }
    /// <summary>
    /// 构建环形表达式
    /// </summary>
    /// <param name="bundle"></param>
    public LambdaExpression BuildCircleLambda(ComplexBundle bundle)
    {
        if (bundle.IsCircle && bundle.Converter is IEmitComplexConverter complexConverter)
            return complexConverter.BuildWithContext(this);
        return null;
    }
    #endregion
    #region IComplexBundle
    /// <inheritdoc />
    ComplexBundle IComplexBundle.Accept(PairTypeKey item, IEmitConverter converter)
        => GetBundleOrCreate(item, converter);
    /// <summary>
    /// 获取转化器
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public IEmitConverter GetConverter(PairTypeKey key)
        => _options.GetEmitConverter(key);
    #endregion
}
