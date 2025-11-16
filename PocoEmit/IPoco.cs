using Hand.Collections;
using Hand.Configuration;
using Hand.Reflection;
using PocoEmit.Collections;
using PocoEmit.Configuration;
using PocoEmit.Converters;
using System;
using System.Reflection;

namespace PocoEmit;

/// <summary>
/// 简单对象处理接口
/// </summary>
public interface IPoco 
    : IStore<PairTypeKey, IEmitConverter>
    , IConfigure<PairTypeKey, IEmitConverter>
{
    #region MemberInfo
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
    #endregion
    #region Converter
    /// <summary>
    /// 获取Emit类型转化
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    IEmitConverter GetEmitConverter(in PairTypeKey key);
    #endregion
}
