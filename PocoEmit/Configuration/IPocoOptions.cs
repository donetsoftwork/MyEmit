using PocoEmit.Builders;
using PocoEmit.Collections;
using PocoEmit.Converters;
using PocoEmit.Reflection;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace PocoEmit.Configuration;

/// <summary>
/// 简单对象配置
/// </summary>
public interface IPocoOptions
    : ISettings<MapTypeKey, IEmitConverter>
    , ISettings<Type, MemberBundle>
    , ISettings<MemberInfo, Func<object, object>>
    , ISettings<MemberInfo, Action<object, object>>
{
    /// <summary>
    /// 转换器构造器
    /// </summary>
    IConvertBuilder ConvertBuilder { get; }
    /// <summary>
    /// 转换器
    /// </summary>
    IEnumerable<IEmitConverter> Converters { get; }
    /// <summary>
    /// 反射获取成员
    /// </summary>
    IReflectionMember ReflectionMember { get; }
    /// <summary>
    /// 成员缓存器
    /// </summary>
    TypeMemberCacher MemberCacher { get; }
    /// <summary>
    /// 读成员
    /// </summary>
    /// <param name="member"></param>
    /// <returns></returns>
    Func<object, object> GetReadFunc(MemberInfo member);
    /// <summary>
    /// 写成员
    /// </summary>
    /// <param name="member"></param>
    /// <returns></returns>
    Action<object, object> GetWriteAction(MemberInfo member);
    /// <summary>
    /// 获取Emit类型转化
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    IEmitConverter GetEmitConverter(MapTypeKey key);
    /// <summary>
    /// 配置Emit类型转化
    /// </summary>
    /// <param name="key"></param>
    /// <param name="converter"></param>
    void SetConvertSetting(MapTypeKey key, IEmitConverter converter);
    /// <summary>
    /// 获取转化配置
    /// </summary>
    /// <param name="key"></param>
    /// <param name="converter"></param>
    /// <returns></returns>
    bool TryGetConvertSetting(MapTypeKey key, out IEmitConverter converter);
}
