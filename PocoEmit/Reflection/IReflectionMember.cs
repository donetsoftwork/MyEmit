using PocoEmit.Collections;
using System;

namespace PocoEmit.Reflection;

/// <summary>
/// 反射获取成员
/// </summary>
public interface IReflectionMember
{
    /// <summary>
    /// 获取所有成员
    /// </summary>
    /// <param name="instanceType"></param>
    /// <returns></returns>
    MemberBundle GetMembers(Type instanceType);
}