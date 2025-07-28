using PocoEmit.Activators;
using PocoEmit.Collections;
using PocoEmit.Copies;
using PocoEmit.Maping;
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
{
    /// <summary>
    /// 复制器
    /// </summary>
    CopierFactory CopierFactory { get; }
    /// <summary>
    /// 激活器
    /// </summary>
    ActivatorFactory ActivatorFactory { get; }
    /// <summary>
    /// 基础类型配置
    /// </summary>
    PrimitiveConfiguration Primitives { get; }
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
}
