using PocoEmit.Builders;
using PocoEmit.Configuration;
using PocoEmit.Converters;
using PocoEmit.Resolves;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

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
    private readonly Dictionary<PairTypeKey, ComplexBundle> _collections = [];
    private readonly Dictionary<PairTypeKey, ComplexBundle> _readies = [];
    private readonly Dictionary<PairTypeKey, LambdaExpression> _lambdas = [];
    private readonly Dictionary<PairTypeKey, ContextAchieved> _contextAchieves = [];
    private ParameterExpression _convertContextParameter = null;
    private MethodInfo _createConvertContextMethod = null;
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
    /// 上下文构建结果
    /// </summary>
    public IEnumerable<ContextAchieved> ContextAchieves
        => _contextAchieves.Values;
    /// <summary>
    /// 环状引用上下文
    /// </summary>
    public ParameterExpression ConvertContextParameter
        => _convertContextParameter;
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
    /// <summary>
    /// 获取上下文构建结果
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public ContextAchieved GetAchieve(PairTypeKey key)
    {
        if(_contextAchieves.TryGetValue(key, out var achieved))
            return achieved;
        var bundle = GetBundle(key);
        if(bundle is null)
            return null;
        if(bundle.HasCircle)
        {
            _options.TryGetValue(key, out IContextConverter converter);
            return _contextAchieves[key] = ContextAchieved.CreateByConverter(converter);
        }
        return null;
    }
    #endregion
    #region bundle
    /// <summary>
    /// 获取复杂类型成员信息
    /// </summary>
    /// <param name="key"></param>
    /// <param name="converter"></param>
    /// <param name="depth"></param>
    /// <param name="isCollection"></param>
    /// <returns></returns>
    public ComplexBundle GetBundleOrCreate(PairTypeKey key, IEmitConverter converter, int depth, bool isCollection)
    {
        // 忽略基础类型
        if (_options.CheckPrimitive(key.LeftType))
            return null;
        if (_collections.TryGetValue(key, out var bundle))
            bundle.CheckDepth(depth);
        else
            _collections.Add(key, bundle = new(this, key, converter, depth, isCollection));
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
        CheckCircle();
        // 无循环引用构建
        BuildNoCircle();
        if (_collections.Count == 0)
            return;
        // 环形引用构建
        BuildCircle();
    }
    /// <summary>
    /// 检查循环引用
    /// </summary>
    public void CheckCircle()
    {
        int circleCount = 0;
        ComplexBundle circle = null;
        foreach (var bundle in _collections.Values)
        {
            if (bundle.CheckIsCircle())
            {
                circle = bundle;
                circleCount++;
            }
        }
        foreach (var bundle in _collections.Values)
            bundle.CheckHasCircle();
        switch(circleCount)
        {
            case 0:
                return;
            case 1:
                if(circle is not null)
                {
                    var key = circle.Key;
                    _createConvertContextMethod = ConvertContext.GetCreateSingleMethod(key.LeftType, key.RightType);
                }
                break;
            default:     
                break;
        }
        _convertContextParameter = ConvertContext.CreateParameter();
        _createConvertContextMethod ??= ConvertContext.CreateMethod;
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
        => Expression.Assign(context, Expression.Call(null, _createConvertContextMethod));
    #region BuildNoInclude
    /// <summary>
    /// 无包含构建
    /// </summary>
    private void BuildNoInclude()
    {
        var list = _collections.Values
            .Where(bundle => bundle.Includes.Count == 0)
            .OrderByDescending(bundle => bundle.Depth)
            .ToArray();
        if (list.Length == 0)
            return;
        var unexpected = false;
        foreach (var bundle in list)
        {
            // 构建完成移除被调用,简化依赖关系
            if (BuildNoCircle(bundle))
                OnFinish(bundle);
            else
                unexpected = true;
        }
        // 异常情况避免死循环
        if (unexpected)
            return;
        // 重复预构建,简化依赖关系
        BuildNoInclude();
    }
    #endregion
    #region BuildNoCircle
    /// <summary>
    /// 无循环引用构建
    /// </summary>
    private void BuildNoCircle()
    {
        BuildNoInclude();
        var list = _collections.Values
            .Where(bundle => !bundle.HasCircle)
            .OrderByDescending(bundle => bundle.Depth)
            .OrderBy(bundle => bundle.Includes.Count)
            .ToArray();
        if (list.Length == 0)
            return;
        foreach (var bundle in list)
        {
            // 构建完成移除被调用,简化依赖关系
            if (BuildNoCircle(bundle))
                OnFinish(bundle);
        }
    }
    /// <summary>
    /// 无循环引用构建
    /// </summary>
    /// <param name="bundle"></param>
    private bool BuildNoCircle(ComplexBundle bundle)
    {
        var key = bundle.Key;
        var lambda = BuildLambdaByNoCircle(key, bundle.Converter);
        if (lambda is null)
            return false;
        _lambdas[key] = lambda;
        return true;
    }
    /// <summary>
    /// 构建无循环引用表达式
    /// </summary>
    /// <param name="key"></param>
    /// <param name="converter"></param>
    /// <returns></returns>
    private LambdaExpression BuildLambdaByNoCircle(PairTypeKey key, IEmitConverter converter)
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
    #region BuildCircle
    /// <summary>
    /// 环形引用构建
    /// </summary>
    public void BuildCircle()
    {
        // 自循环构建
        BuildSelfCircle();
        var list = _collections.Values
            .Where(bundle => bundle.HasCircle && !bundle.IsCollection)
            .OrderByDescending(bundle => bundle.Depth)
            .OrderBy(bundle =>  bundle.Includes.Count)
            .ToArray();
        foreach (var bundle in list)
            BuildCircle(bundle);
        BuilCircleCollection();
    }
    /// <summary>
    /// 构建包含环形的集合
    /// </summary>
    public void BuilCircleCollection()
    {
        var list = _collections.Values
             .Where(bundle => bundle.HasCircle && bundle.IsCollection)
            .OrderByDescending(bundle => bundle.Depth)
            .OrderBy(bundle => bundle.Includes.Count)
            .ToArray();
        foreach (var bundle in list)
            BuildCircle(bundle);
    }
    #region BuildSelfCircle
    /// <summary>
    /// 自循环构建
    /// </summary>
    public void BuildSelfCircle()
    {
        var list = _collections.Values
            .Where(bundle => bundle.IsCircle && !bundle.IsCollection && bundle.Includes.Count == 1)
            .OrderByDescending(bundle => bundle.Depth)
            .ToArray();
        if (list.Length == 0)
            return;
        var unexpected = false;
        foreach (var bundle in list)
        {
            if (BuildCircle(bundle))
                OnFinish(bundle);
            else
                unexpected = true;
        }
        // 异常情况避免死循环
        if (unexpected)
            return;
        // 重复自循环构建,简化依赖关系
        BuildSelfCircle();
    }
    #endregion
    /// <summary>
    /// 环形构建
    /// </summary>
    /// <param name="bundle"></param>
    public bool BuildCircle(ComplexBundle bundle)
    {
        var key = bundle.Key;
        var achieved = GetAchieve(key);
        if (achieved is null)
            return false;
        if(achieved.Func is null)
        {
            var contextLambda = BuildCircleLambda(bundle);
            if (contextLambda is null)
                return false;
            achieved.Build(contextLambda);
            _options.Set(key, achieved);
        }
        return true;
    }
    /// <summary>
    /// 构建环形表达式
    /// </summary>
    /// <param name="bundle"></param>
    public LambdaExpression BuildCircleLambda(ComplexBundle bundle)
    {
        if (bundle.HasCircle && bundle.Converter is IEmitComplexConverter complexConverter)
            return complexConverter.BuildWithContext(this);
        return null;
    }
    #endregion
    #region IComplexBundle
    /// <inheritdoc />
    ComplexBundle IComplexBundle.Accept(PairTypeKey item, IEmitConverter converter, bool isCollection)
        => GetBundleOrCreate(item, converter, 2, isCollection);
    /// <summary>
    /// 获取转化器
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public IEmitConverter GetConverter(PairTypeKey key)
        => _options.GetEmitConverter(key);
    #endregion
}
