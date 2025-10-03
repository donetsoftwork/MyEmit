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
/// <param name="complexCached"></param>
public class BuildContext(IMapperOptions options, ComplexCached complexCached)
    : IBuildContext
    , IComplexBundle
{
    /// <summary>
    /// 构建上下文构建器
    /// </summary>
    /// <param name="options"></param>
    public BuildContext(IMapperOptions options)
        : this(options, options.Cached)
    {
    }
    #region 配置
    private readonly IMapperOptions _options = options;
    private readonly ComplexCached _complexCached = complexCached;
    private readonly Dictionary<PairTypeKey, ComplexBundle> _collections = [];
    private readonly Dictionary<PairTypeKey, ComplexBundle> _readies = [];
    private readonly Dictionary<PairTypeKey, ICompiledConverter> _achieves = [];
    private readonly Dictionary<PairTypeKey, IEmitContextConverter> _contextAchieves = [];
    private MethodInfo _createConvertContextMethod = null;
    /// <summary>
    /// Emit配置
    /// </summary>
    public IMapperOptions Options
        => _options;
    /// <summary>
    /// 被缓存状态
    /// </summary>
    public ComplexCached ComplexCached
        => _complexCached;
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
    /// 上下文构建结果
    /// </summary>
    public IEnumerable<IEmitContextConverter> ContextAchieves
        => _contextAchieves.Values;
    /// <summary>
    /// 环状引用上下文
    /// </summary>
    ParameterExpression IBuildContext.ConvertContextParameter
        => null;
    bool IBuildContext.HasCache
        => false;
    /// <inheritdoc />
    public BuildContext Context
        => this;
    /// <inheritdoc />
    IComplexBundle IBuildContext.Bundle
         => this;
    #endregion
    #region IBuildContext
    /// <inheritdoc />
    public ComplexBundle GetBundle(in PairTypeKey key)
    {
        if (_collections.TryGetValue(key, out var bundle) || _readies.TryGetValue(key, out bundle))
            return bundle;
        return null;
    }
    /// <inheritdoc />
    public Expression Call(bool isCircle, LambdaExpression lambda, params Expression[] arguments)
    {
        if (isCircle)
            return Expression.Invoke(lambda, arguments);
        return _options.Call(lambda, arguments);
    }
    /// <summary>
    /// 获取上下文构建结果
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public IEmitContextConverter GetContexAchieve(in PairTypeKey key)
    {
        if(_contextAchieves.TryGetValue(key, out var achieved))
            return achieved;
        var bundle = GetBundle(key);
        if(bundle is null)
            return null;
        if(bundle.HasCache)
        {
            if (!_options.TryGetCache(key, out IEmitContextConverter converter))
                converter = EmitMapperHelper.CreateContextConverter(_options, key);
            return _contextAchieves[key] = converter;
        }
        return null;
    }
    /// <summary>
    /// 获取构建结果
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public ICompiledConverter GetAchieve(in PairTypeKey key)
    {
        if (_achieves.TryGetValue(key, out var achieved))
            return achieved;
        var bundle = GetBundle(key);
        if (bundle is null)
            return null;
        var converter = bundle.Converter;
        if (converter is ICompiledConverter compiled)
        {
            achieved = compiled;
        }
        else
        {
            achieved = EmitMapperHelper.CreateCompiledConverter(_options, key, converter);
        }
        return _achieves[key] = achieved;
    }
    #endregion
    #region bundle
    /// <summary>
    /// 构造复杂类型成员信息
    /// </summary>
    /// <param name="key"></param>
    /// <param name="converter"></param>
    /// <param name="depth"></param>
    /// <param name="isCollection"></param>
    /// <returns></returns>
    internal ComplexBundle CreateBundle(in PairTypeKey key, IEmitConverter converter, int depth, bool isCollection)
    {
        // 忽略基础类型
        if (_options.CheckPrimitive(key.LeftType))
            return null;
        // 按depth倒序构建
        // 优先非集合构建
        var bundle = new ComplexBundle(this, key, converter, isCollection ? depth + 1 : depth + 2, isCollection);
        _collections.Add(key, bundle);
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
    private void RemoveUsed(in PairTypeKey key, ComplexBundle bundle)
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
        => preview.Preview(this);
    /// <summary>
    /// 准备处理
    /// </summary>
    public void Prepare()
    {
        //// 修正使用状态
        //CheckUsed();
        // 检查执行上下文
        CheckConvertContext();
        // 无循环引用构建
        BuildNoCircle();
        if (_collections.Count == 0)
            return;
        // 环形引用构建
        BuildCircle();
    }
    /// <summary>
    /// 处理循环引用状态
    /// </summary>
    /// <param name="bundles"></param>
    /// <returns></returns>
    public static bool CheckCircle(IEnumerable<ComplexBundle> bundles)
    {
        var state = false;
        foreach (var item in bundles)
        {
            if (item.CheckIsCircle())
                state = true;
        }
        if (state)
        {
            foreach (var bundle in bundles)
                bundle.CheckHasCircle();
        }
        return state;
    }
    /// <summary>
    /// 处理缓存状态
    /// </summary>
    /// <param name="bundles"></param>
    /// <returns></returns>
    public static int CheckIsCache(IEnumerable<ComplexBundle> bundles)
    {
        var count = 0;
        foreach (var item in bundles)
        {
            if (item.CheckIsCache())
                count++;
        }
        if (count == 0)
            return 0;
        count = 0;
        foreach (var item in bundles)
        {
            if (item.CheckCacheInclude())
                count++;
        }
        return count;
    }
    /// <summary>
    /// 处理缓存状态
    /// </summary>
    /// <param name="bundles"></param>
    public static void CheckHasCache(IEnumerable<ComplexBundle> bundles)
    {
        foreach (var bundle in bundles)
            bundle.CheckHasCache();
    }
    /// <summary>
    /// 按照循环引用设置缓存
    /// </summary>
    /// <param name="bundles"></param>
    /// <returns></returns>
    public static int CheckCacheByCircle(IEnumerable<ComplexBundle> bundles)
    {
        var count = 0;
        foreach (var item in bundles)
        {
            if (item.CheckCacheByCircle())
                count++;
        }
        return count;
    }
    /// <summary>
    /// 检查执行上下文
    /// </summary>
    public void CheckConvertContext()
    {
        var bundles = _collections.Values;
        var hasCircle = CheckCircle(bundles);
        int cacheCount = 0;
        switch(_complexCached)
        {
            case ComplexCached.Never:
                break;
            case ComplexCached.Always:
                cacheCount = CheckIsCache(bundles);
                break;
            default:
                if (hasCircle)
                    cacheCount = CheckCacheByCircle(bundles);
                break;
        }
        CheckConvertContext(cacheCount);
        _createConvertContextMethod ??= ConvertContext.CreateMethod;
    }

    /// <summary>
    /// 处理引用上下文
    /// </summary>
    /// <param name="cacheCount">缓存数量</param>
    private void CheckConvertContext(int cacheCount)
    {
        if(cacheCount == 0) 
            return;
        var bundles = _collections.Values;
        CheckHasCache(bundles);
        if (cacheCount == 1)
        {
            var cache = bundles.FirstOrDefault(bundle => bundle.IsCache);
            if (cache is null)
                return;
            var key = cache.Key;
            _createConvertContextMethod = ConvertContext.GetCreateSingleMethod(key.LeftType, key.RightType);
        }
        //else
        //{
        //    _createConvertContextMethod ??= ConvertContext.CreateMethod;
        //}
        //_convertContextParameter = ConvertContext.CreateParameter();
    }
    ///// <summary>
    ///// 修正使用状态
    ///// </summary>
    //public void CheckUsed()
    //{
    //    foreach (var item in _collections.Values)
    //        item.CheckUsed();
    //}
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
    /// "无"包含构建
    /// </summary>
    private void BuildNoInclude()
    {
        var list = _collections.Values
            .Where(bundle => bundle.Includes.Count == 0 && !bundle.HasCircle)
            .OrderByDescending(bundle => bundle.Depth)
            .ToArray();
        if (list.Length == 0)
            return;
        var unexpected = false;
        foreach (var bundle in list)
        {
            // 构建完成移除被调用,简化依赖关系
            if (BuildBundle(bundle))
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
            if (BuildBundle(bundle))
                OnFinish(bundle);
        }
    }
    #endregion
    #region BuildCircle
    /// <summary>
    /// 包含上下文构建
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
            BuildContextConvert(bundle);
        BuilCollection();
    }
    /// <summary>
    /// 构建集合包含上下文
    /// </summary>
    public void BuilCollection()
    {
        var list = _collections.Values
             .Where(bundle => bundle.IsCollection)
            .OrderByDescending(bundle => bundle.Depth)
            .OrderBy(bundle => bundle.Includes.Count)
            .ToArray();
        foreach (var bundle in list)
            BuildBundle(bundle);
    }
    #region BuildSelfCircle
    /// <summary>
    /// "自"循环构建
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
            // 构建完成移除被调用,简化依赖关系
            if (BuildBundle(bundle))
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
    #endregion
    #region BuildBundle
    /// <summary>
    /// 构建复杂类型
    /// </summary>
    /// <param name="bundle"></param>
    /// <returns></returns>
    public bool BuildBundle(ComplexBundle bundle)
    {
        if (bundle.HasCache)
            return BuildContextConvert(bundle);
        return BuildNoContext(bundle);
    }
    #region BuildNoContext
    /// <summary>
    /// 无上下文构建
    /// </summary>
    /// <param name="bundle"></param>
    private bool BuildNoContext(ComplexBundle bundle)
    {
        var key = bundle.Key;
        var achieved = GetAchieve(key);
        if (achieved is null)
            return false;
        if (!achieved.Compiled)
        {
            var lambda = BuildLambdaByNoContext(key, bundle.Converter);
            if (lambda is null)
                return false;
            if (achieved.CompileDelegate(lambda))
            {
                _options.Set(key, achieved);
                return true;
            }
        }
        return true;
    }
    /// <summary>
    /// 构建无上下文表达式
    /// </summary>
    /// <param name="key"></param>
    /// <param name="converter"></param>
    /// <returns></returns>
    private LambdaExpression BuildLambdaByNoContext(in PairTypeKey key, IEmitConverter converter)
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
    #region BuildContextConvert
    /// <summary>
    /// 构建上下文转化
    /// </summary>
    /// <param name="bundle"></param>
    public bool BuildContextConvert(ComplexBundle bundle)
    {
        var key = bundle.Key;
        var contexAchieve = GetContexAchieve(key);
        if (contexAchieve is null)
            return false;
        if (!contexAchieve.Compiled)
        {
            var contextLambda = BuildConvertContextLambda(bundle);
            if (contextLambda is null)
                return false;
            if (contexAchieve.CompileDelegate(contextLambda))
            {
                _options.Set(key, contexAchieve);
                return true;
            }
        }
        return true;
    }
    /// <summary>
    /// 构建上下文表达式
    /// </summary>
    /// <param name="bundle"></param>
    public LambdaExpression BuildConvertContextLambda(ComplexBundle bundle)
    {
        if (bundle.Converter is IEmitComplexConverter complexConverter)
            return complexConverter.BuildWithContext(this);
        return null;
    }
    #endregion
    #endregion
    #region IComplexBundle
    /// <inheritdoc />
    ComplexBundle IComplexBundle.Accept(in PairTypeKey item, IEmitConverter converter, bool isCollection)
    {
        //GetBundleOrCreate(item, converter, 2, isCollection);
        var bundle = GetBundle(item);
        if (bundle is null)
        {
            return CreateBundle(item, converter, 0, isCollection);
        }
        return null;
    }
    /// <summary>
    /// 获取转化器
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public IEmitConverter GetConverter(in PairTypeKey key)
        => _options.GetEmitConverter(key);
    #endregion
}
