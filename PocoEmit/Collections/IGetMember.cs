using PocoEmit.Reflection;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace PocoEmit.Collections;

/// <summary>
/// 成员读取器
/// </summary>
public interface IGetMember
{
    /// <summary>
    /// 获取所有可读成员
    /// </summary>
    /// <param name="instanceType"></param>
    /// <returns></returns>
    IEnumerable<MemberInfo> GetGetMembers(Type instanceType);
    /// <summary>
    /// 获取可读成员
    /// </summary>
    /// <param name="instanceType"></param>
    /// <param name="memberName"></param>
    /// <returns></returns>
    MemberInfo GetGetMember(Type instanceType, string memberName);
    /// <summary>
    /// 读取器
    /// </summary>
    /// <typeparam name="TInstance"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="member"></param>
    /// <returns></returns>
    Func<TInstance, TValue> Reader<TInstance, TValue>(MemberInfo member);
}
