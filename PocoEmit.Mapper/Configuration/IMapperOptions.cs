using PocoEmit.Activators;
using PocoEmit.Collections;
using PocoEmit.Copies;
using PocoEmit.Maping;
using PocoEmit.Reflection;
using System;

namespace PocoEmit.Configuration;

/// <summary>
///  映射配置接口
/// </summary>
public interface IMapperOptions : IPocoOptions
    , ISettings<MapTypeKey, IEmitCopier>
    , ISettings<Type, IEmitActivator>
    , ISettings<MapTypeKey, IMemberMatch>
    , ISettings<Type, bool>
    , ISettings<Type, object>
    , IReflectionConstructor
{
    /// <summary>
    /// 获取成员匹配
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    IMemberMatch GetMemberMatch(MapTypeKey key);
    /// <summary>
    /// 默认成员匹配
    /// </summary>
    IMemberMatch DefaultMatch { get; set; }
    /// <summary>
    /// 获取Emit类型复制器
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    IEmitCopier GetEmitCopier(MapTypeKey key);
    /// <summary>
    /// 获取Emit类型激活器
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    IEmitActivator GetEmitActivatorr(Type key);
    /// <summary>
    /// 是否基础类型
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    bool CheckPrimitive(Type type);
}
