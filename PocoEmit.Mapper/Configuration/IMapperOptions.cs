using PocoEmit.Activators;
using PocoEmit.Collections;
using PocoEmit.Copies;
using PocoEmit.Maping;
using PocoEmit.Reflection;
using System;
using System.Linq.Expressions;

namespace PocoEmit.Configuration;

/// <summary>
///  映射配置接口
/// </summary>
public interface IMapperOptions
    : IPocoOptions
    , IMapper
    , ICacher<MapTypeKey, IEmitCopier>
    , ICacher<Type, IEmitActivator>
    , ICacher<Type, bool>
    , IReflectionConstructor
{
    /// <summary>
    /// 默认成员匹配
    /// </summary>
    IMemberMatch DefaultMatcher { get; set; }

    /// <summary>
    /// 获取Emit类型激活器
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    IEmitActivator GetEmitActivator(MapTypeKey key);
    /// <summary>
    /// 是否基础类型
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    bool CheckPrimitive(Type type);
    /// <summary>
    /// 构造默认值
    /// </summary>
    /// <param name="destType"></param>
    /// <returns></returns>
    Expression CreateDefault(Type destType);
}
