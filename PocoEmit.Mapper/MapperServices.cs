using PocoEmit.Activators;
using PocoEmit.Collections;
using PocoEmit.Configuration;
using PocoEmit.Helpers;
using PocoEmit.Maping;
using System;
using System.Collections.Generic;

namespace PocoEmit;

/// <summary>
/// 映射扩展方法
/// </summary>
public static partial class MapperServices
{
    #region Activator
    /// <summary>
    /// 设置委托来激活
    /// </summary>
    /// <typeparam name="TInstance"></typeparam>
    /// <param name="settings"></param>
    /// <param name="new"></param>
    /// <returns></returns>
    public static DelegateActivator<TInstance> SetNew<TInstance>(this ISettings<Type, IEmitActivator> settings, Func<TInstance> @new)
    {
        var key = typeof(TInstance);
        var activator = new DelegateActivator<TInstance>(@new);
        settings.Set(key, activator);
        return activator;
    }
    /// <summary>
    /// 设置new来激活
    /// </summary>
    /// <typeparam name="TInstance"></typeparam>
    /// <param name="settings"></param>
    /// <returns></returns>
    public static DelegateActivator<TInstance> SetNew<TInstance>(this ISettings<Type, IEmitActivator> settings)
        where TInstance : new()
        => settings.SetNew(() => new TInstance());
    ///// <summary>
    ///// 尝试设置委托来激活不覆盖
    ///// </summary>
    ///// <typeparam name="TInstance"></typeparam>
    ///// <param name="settings"></param>
    ///// <param name="new"></param>
    ///// <returns></returns>
    //public static IEmitActivator TrySetNew<TInstance>(this ISettings<Type, IEmitActivator> settings, Func<TInstance> @new)
    //{
    //    var key = typeof(TInstance);
    //    if (settings.TryGetValue(key, out var value0) && value0 is not null)
    //        return value0;
    //    var activator = new DelegateActivator<TInstance>(@new);
    //    settings.Set(key, activator);
    //    return activator;
    //}
    ///// <summary>
    ///// 设置new来激活
    ///// </summary>
    ///// <typeparam name="TInstance"></typeparam>
    ///// <param name="settings"></param>
    ///// <returns></returns>
    //public static IEmitActivator TrySetNew<TInstance>(this ISettings<Type, IEmitActivator> settings)
    //    where TInstance : new()
    //    => settings.TrySetNew(() => new TInstance());
    #endregion
    /// <summary>
    /// 获取成员匹配
    /// </summary>
    /// <param name="options"></param>
    /// <param name="sourceType"></param>
    /// <param name="destType"></param>
    /// <returns></returns>
    public static IMemberMatch GetMemberMatch(this IMapperOptions options, Type sourceType, Type destType)
        => options.GetMemberMatch(new MapTypeKey(sourceType, destType));
    /// <summary>
    /// 配置类型映射
    /// </summary>
    /// <typeparam name="TSource">源类型</typeparam>
    /// <typeparam name="TDest">目标类型</typeparam>
    /// <param name="mapper">映射配置</param>
    /// <param name="comparer">比较器</param>
    /// <returns></returns>
    public static MapHelper<TSource, TDest> Configure<TSource, TDest>(this IMapperOptions mapper, IEqualityComparer<string> comparer)
        =>new(mapper, comparer);
    /// <summary>
    /// 配置类型映射
    /// </summary>
    /// <typeparam name="TSource">源类型</typeparam>
    /// <typeparam name="TDest">目标类型</typeparam>
    /// <param name="mapper">映射配置</param>
    /// <returns></returns>
    public static MapHelper<TSource, TDest> Configure<TSource, TDest>(this IMapperOptions mapper)
        => new(mapper, StringComparer.OrdinalIgnoreCase);
}
