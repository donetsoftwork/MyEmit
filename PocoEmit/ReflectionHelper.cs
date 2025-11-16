using PocoEmit.Collections;
using PocoEmit.Members;
using System;
using System.Reflection;

namespace PocoEmit;

/// <summary>
/// 反射帮助类
/// </summary>
public static class ReflectionHelper
{
    #region GetTypeMembers
    /// <summary>
    /// 获取类型所有成员
    /// </summary>
    /// <typeparam name="TInstance"></typeparam>
    /// <param name="poco"></param>
    /// <returns></returns>
    public static MemberBundle GetTypeMembers<TInstance>(this IPoco poco)
        => poco.MemberCacher.Get(typeof(TInstance));
    /// <summary>
    /// 获取类型所有成员
    /// </summary>
    /// <param name="poco"></param>
    /// <param name="instanceType"></param>
    /// <returns></returns>
    public static MemberBundle GetTypeMembers(this IPoco poco, Type instanceType)
        => poco.MemberCacher.Get(instanceType);
    #endregion
    #region GetReadMember
    /// <summary>
    /// 获取可读成员
    /// </summary>
    /// <param name="poco"></param>
    /// <param name="instanceType"></param>
    /// <param name="memberName"></param>
    /// <returns></returns>
    public static MemberInfo GetReadMember(this IPoco poco, Type instanceType, string memberName)
    {
        return poco.MemberCacher
            .Get(instanceType)
            ?.GetReadMember(memberName);
    }
    /// <summary>
    /// 获取可读成员
    /// </summary>
    /// <typeparam name="TInstance"></typeparam>
    /// <param name="poco"></param>
    /// <param name="memberName"></param>
    /// <returns></returns>
    public static MemberInfo GetReadMember<TInstance>(this IPoco poco, string memberName)
    {
        return poco.MemberCacher
            .Get(typeof(TInstance))
            ?.GetReadMember(memberName);
    }
    /// <summary>
    /// 获取可读成员
    /// </summary>
    /// <param name="bundle"></param>
    /// <param name="memberName"></param>
    /// <returns></returns>
    public static MemberInfo GetReadMember(this MemberBundle bundle, string memberName)
    {
        if (bundle.ReadMembers.TryGetValue(memberName, out var member))
            return member;
        return null;
    }
    #endregion
    #region GetReader
    /// <summary>
    /// 获取可读成员
    /// </summary>
    /// <param name="poco"></param>
    /// <param name="instanceType"></param>
    /// <param name="memberName"></param>
    /// <returns></returns>
    public static IEmitMemberReader GetEmitReader(this IPoco poco, Type instanceType, string memberName)
    {
        return poco.MemberCacher
            .Get(instanceType)
            ?.GetEmitReader(memberName);
    }
    /// <summary>
    /// 获取可读成员
    /// </summary>
    /// <typeparam name="TInstance"></typeparam>
    /// <param name="poco"></param>
    /// <param name="memberName"></param>
    /// <returns></returns>
    public static IEmitMemberReader GetEmitReader<TInstance>(this IPoco poco, string memberName)
    {
        return poco.MemberCacher
            .Get(typeof(TInstance))
            ?.GetEmitReader(memberName);
    }
    /// <summary>
    /// 获取可读成员
    /// </summary>
    /// <param name="bundle"></param>
    /// <param name="memberName"></param>
    /// <returns></returns>
    public static IEmitMemberReader GetEmitReader(this MemberBundle bundle, string memberName)
    {
        if (bundle.EmitReaders.TryGetValue(memberName, out var reader))
            return reader;
        return null;
    }
    #endregion
    #region GetWriteMember
    /// <summary>
    /// 获取可写成员
    /// </summary>
    /// <param name="poco"></param>
    /// <param name="instanceType"></param>
    /// <param name="memberName"></param>
    /// <returns></returns>
    public static MemberInfo GetWriteMember(this IPoco poco, Type instanceType, string memberName)
    {
        return poco.MemberCacher
            .Get(instanceType)
            ?.GetWriteMember(memberName);
    }
    /// <summary>
    /// 获取可写成员
    /// </summary>
    /// <typeparam name="TInstance"></typeparam>
    /// <param name="poco"></param>
    /// <param name="memberName"></param>
    /// <returns></returns>
    public static MemberInfo GetWriteMember<TInstance>(this IPoco poco, string memberName)
    {
        return poco.MemberCacher
            .Get(typeof(TInstance))
            ?.GetWriteMember(memberName);
    }
    /// <summary>
    /// 获取可读成员
    /// </summary>
    /// <param name="bundle"></param>
    /// <param name="memberName"></param>
    /// <returns></returns>
    public static MemberInfo GetWriteMember(this MemberBundle bundle, string memberName)
    {
        if (bundle.WriteMembers.TryGetValue(memberName, out var member))
            return member;
        return null;
    }
    #endregion
    #region GetWriter
    /// <summary>
    /// 获取可写成员
    /// </summary>
    /// <param name="poco"></param>
    /// <param name="instanceType"></param>
    /// <param name="memberName"></param>
    /// <returns></returns>
    public static IEmitMemberWriter GetEmitWriter(this IPoco poco, Type instanceType, string memberName)
    {
        return poco.MemberCacher
            .Get(instanceType)
            ?.GetEmitWriter(memberName);
    }
    /// <summary>
    /// 获取可写成员
    /// </summary>
    /// <typeparam name="TInstance"></typeparam>
    /// <param name="poco"></param>
    /// <param name="memberName"></param>
    /// <returns></returns>
    public static IEmitMemberWriter GetEmitWriter<TInstance>(this IPoco poco, string memberName)
    {
        return poco.MemberCacher
            .Get(typeof(TInstance))
            ?.GetEmitWriter(memberName);
    }
    /// <summary>
    /// 获取可读成员
    /// </summary>
    /// <param name="bundle"></param>
    /// <param name="memberName"></param>
    /// <returns></returns>
    public static IEmitMemberWriter GetEmitWriter(this MemberBundle bundle, string memberName)
    {
        if (bundle.EmitWriters.TryGetValue(memberName, out var writer))
            return writer;
        return null;
    }
    #endregion    
}
