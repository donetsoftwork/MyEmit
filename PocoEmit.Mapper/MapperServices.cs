using PocoEmit.Activators;
using PocoEmit.Builders;
using PocoEmit.Configuration;
using PocoEmit.Helpers;
using PocoEmit.Maping;
using System;
using System.Linq.Expressions;

namespace PocoEmit;

/// <summary>
/// 映射扩展方法
/// </summary>
public static partial class MapperServices
{
    #region UseActivator
    /// <summary>
    /// 设置委托来激活
    /// </summary>
    /// <typeparam name="TInstance"></typeparam>
    /// <param name="mapper"></param>
    /// <param name="activatorFunc"></param>
    /// <returns></returns>
    public static IMapper UseActivator<TInstance>(this IMapper mapper, Expression<Func<TInstance>> activatorFunc)
    {
        var key = typeof(TInstance);
        mapper.Configure(key, new DelegateActivator<TInstance>(activatorFunc));
        return mapper;
    }
    /// <summary>
    /// 设置带参委托来激活
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TDest"></typeparam>
    /// <param name="mapper"></param>
    /// <param name="activatorFunc"></param>
    /// <returns></returns>
    public static IMapper UseActivator<TSource, TDest>(this IMapper mapper, Expression<Func<TSource, TDest>> activatorFunc)
    {
        var key = new PairTypeKey(typeof(TSource), typeof(TDest));
        mapper.Configure(key, new DelegateActivator<TSource, TDest>(activatorFunc));
        return mapper;
    }
    /// <summary>
    /// 设置带参委托来激活
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TDest"></typeparam>
    /// <param name="mapper"></param>
    /// <param name="activatorFunc"></param>
    /// <returns></returns>
    public static IMapper UseActivator<TSource, TDest>(this IMapper mapper, Expression<Func<TDest>> activatorFunc)
    {
        var key = new PairTypeKey(typeof(TSource), typeof(TDest));
        var activator = new DelegateActivator<TDest>(activatorFunc);
        mapper.Configure(key, activator);
        return mapper;
    }
    /// <summary>
    /// 设置new来激活
    /// </summary>
    /// <typeparam name="TInstance"></typeparam>
    /// <param name="mapper"></param>
    /// <returns></returns>
    public static IMapper UseActivator<TInstance>(this IMapper mapper)
        where TInstance : new()
        => mapper.UseActivator(static () => new TInstance());
    #endregion
    #region UseDefault
    /// <summary>
    /// 配置类型默认值
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="mapper"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public static IMapper UseDefault<TValue>(this IMapper mapper, TValue value)
    {
        object defaultValue = value;
        var type = typeof(TValue);
        mapper.Configure(type, ConstantBuilder.Use(value, type));
        return mapper;
    }
    /// <summary>
    /// 配置类型默认值
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="mapper"></param>
    /// <param name="valueFunc"></param>
    /// <returns></returns>
    public static IMapper UseDefault<TValue>(this IMapper mapper, Expression<Func<TValue>> valueFunc)
    {
        object defaultValue = valueFunc;
        mapper.Configure(typeof(TValue), new FuncBuilder<TValue>(valueFunc));
        return mapper;
    }
    #endregion
    /// <summary>
    /// 获取成员匹配
    /// </summary>
    /// <param name="mapper"></param>
    /// <param name="sourceType"></param>
    /// <param name="destType"></param>
    /// <returns></returns>
    public static IMemberMatch GetMemberMatch(this IMapper mapper, Type sourceType, Type destType)
        => mapper.GetMemberMatch(new PairTypeKey(sourceType, destType));
    /// <summary>
    /// 配置类型映射
    /// </summary>
    /// <typeparam name="TSource">源类型</typeparam>
    /// <typeparam name="TDest">目标类型</typeparam>
    /// <param name="mapper">映射配置</param>
    /// <param name="comparison">比较器</param>
    /// <returns></returns>
    public static MapHelper<TSource, TDest> ConfigureMap<TSource, TDest>(this IMapper mapper, StringComparison comparison)
        => new(mapper, comparison);
    /// <summary>
    /// 配置类型映射
    /// </summary>
    /// <typeparam name="TSource">源类型</typeparam>
    /// <typeparam name="TDest">目标类型</typeparam>
    /// <param name="mapper">映射配置</param>
    /// <returns></returns>
    public static MapHelper<TSource, TDest> ConfigureMap<TSource, TDest>(this IMapper mapper)
        => new(mapper, StringComparison.OrdinalIgnoreCase);
}
