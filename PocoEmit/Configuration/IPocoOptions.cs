using PocoEmit.Builders;
using PocoEmit.Collections;
using PocoEmit.Converters;
using PocoEmit.Reflection;
using System;
using System.Collections.Generic;

namespace PocoEmit.Configuration;

/// <summary>
/// 简单对象配置接口
/// </summary>
public interface IPocoOptions
    : IPoco
    , ICacher<MapTypeKey, IEmitConverter>
    , ICacher<Type, MemberBundle>
{
    /// <summary>
    /// 转换器构造器
    /// </summary>
    ConvertBuilder ConvertBuilder { get; }
    /// <summary>
    /// 转换器
    /// </summary>
    IEnumerable<IEmitConverter> Converters { get; }
    /// <summary>
    /// 反射获取成员
    /// </summary>
    IReflectionMember ReflectionMember { get; }
}
