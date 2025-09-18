using PocoEmit.Builders;
using PocoEmit.Configuration;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace PocoEmit.Resolves;

/// <summary>
/// 转化执行上下文
/// </summary>
/// <param name="mapper"></param>
public class ConvertContext(IMapperOptions mapper)
    : IConvertContext
{
    #region 配置
    private readonly Dictionary<ConvertCacheKey, object> _cacher = [];
    private readonly IMapperOptions _mapper = mapper;
    /// <summary>
    /// 对象映射
    /// </summary>
    public IMapperOptions Mapper
        => _mapper;
    #endregion
    #region 功能
    /// <summary>
    /// 转化
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TDest"></typeparam>
    /// <param name="source"></param>
    /// <returns></returns>
    public TDest Convert<TSource, TDest>(TSource source)
    {
        var cacheKey = new ConvertCacheKey(source, typeof(TDest));
        if (_cacher.TryGetValue(cacheKey, out var cached))
            return (TDest)cached;
        var key = new PairTypeKey(typeof(TSource), typeof(TDest));
        if (GetConvertFunc(key) is Func<ConvertContext, TSource, TDest> func)
            return func(this, source);
        return default;
    }
    /// <summary>
    /// 获取转化委托
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    private Delegate GetConvertFunc(PairTypeKey key)
    {
        _mapper.TryGetValue(key, out Delegate func);
        return func;
    }
    /// <summary>
    /// 设置缓存
    /// </summary>
    /// <param name="source"></param>
    /// <param name="dest"></param>
    public void SetCache<TSource, TDest>(TSource source, TDest dest)
        => _cacher[new ConvertCacheKey(source, typeof(TDest))] = dest;
    #endregion
    #region Expression
    /// <summary>
    /// 构造参数
    /// </summary>
    /// <returns></returns>
    public static ParameterExpression CreateParameter()
        => Expression.Parameter(typeof(IConvertContext), "context");
    /// <summary>
    /// 参数赋值初始化
    /// </summary>
    /// <param name="context"></param>
    /// <param name="mapper"></param>
    /// <returns></returns>
    public static Expression InitParameter(ParameterExpression context, Expression mapper)
        => Expression.Assign(context, Expression.New(_constructor, mapper));
    /// <summary>
    /// 调用转化
    /// </summary>
    /// <param name="context"></param>
    /// <param name="key"></param>
    /// <param name="source"></param>
    /// <returns></returns>
    public static Expression CallConvert(ParameterExpression context, PairTypeKey key, Expression source)
        => Expression.Call(context, _convertMethod.MakeGenericMethod(key.LeftType, key.RightType), source);
    /// <summary>
    /// 调用设置缓存
    /// </summary>
    /// <param name="context"></param>
    /// <param name="key"></param>
    /// <param name="source"></param>
    /// <param name="dest"></param>
    /// <returns></returns>
    public static Expression CallSetCache(ParameterExpression context, PairTypeKey key, Expression source, Expression dest)
        => Expression.Call(context, _setCacheMethod.MakeGenericMethod(key.LeftType, key.RightType), source, dest);
    #endregion
    #region Reflection
    /// <summary>
    /// 反射构造函数
    /// </summary>
    private static readonly ConstructorInfo _constructor = ReflectionHelper.GetConstructorByParameterType(typeof(ConvertContext), typeof(IMapper));
    /// <summary>
    /// 反射Convert方法
    /// </summary>
    private static readonly MethodInfo _convertMethod = EmitHelper.GetMethodInfo<IConvertContext, int>(context => context.Convert<int, int>(0))
        .GetGenericMethodDefinition();
    /// <summary>
    /// 反射SetCache方法
    /// </summary>
    private static readonly MethodInfo _setCacheMethod = EmitHelper.GetActionMethodInfo<IConvertContext>(context => context.SetCache(1, 1))
        .GetGenericMethodDefinition();
    #endregion
}
