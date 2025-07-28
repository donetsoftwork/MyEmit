using System;
using System.Collections.Generic;
using System.Reflection;

namespace PocoEmit.Collections;

/// <summary>
/// 成员写入器
/// </summary>
public interface ISetMember
{
    /// <summary>
    /// 获取所有可读成员
    /// </summary>
    /// <param name="instanceType"></param>
    /// <returns></returns>
    IEnumerable<MemberInfo> GetSetMembers(Type instanceType);
    /// <summary>
    /// 获取可写成员
    /// </summary>
    /// <param name="instanceType"></param>
    /// <param name="memberName"></param>
    /// <returns></returns>
    MemberInfo GetSetMember(Type instanceType, string memberName);
    /// <summary>
    /// 写入器
    /// </summary>
    /// <typeparam name="TInstance"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="member"></param>
    /// <returns></returns>
    Func<TInstance, TValue> Writer<TInstance, TValue>(MemberInfo member);
}
