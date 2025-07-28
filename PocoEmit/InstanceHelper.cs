using System;
using System.Reflection;

namespace PocoEmit;

/// <summary>
/// Emit实例成员帮助类
/// </summary>
public static class InstanceHelper
{
    /// <summary>
    /// 读取实例成员
    /// </summary>
    /// <typeparam name="TInstance"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="memberName"></param>
    /// <returns></returns>
    public static Func<TInstance, TValue> GetReadFunc<TInstance, TValue>(string memberName)
        => Poco.Global.GetReadFunc<TInstance, TValue>(memberName);
    /// <summary>
    /// 写入实例成员
    /// </summary>
    /// <typeparam name="TInstance"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="memberName"></param>
    /// <returns></returns>
    public static Action<TInstance, TValue> GetWriteAction<TInstance, TValue>(string memberName)
        => Poco.Global.GetWriteAction<TInstance, TValue>(memberName);
    /// <summary>
    /// 读属性
    /// </summary>
    /// <typeparam name="TInstance"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="member"></param>
    /// <returns></returns>
    public static Func<TInstance, TValue> GetReadFunc<TInstance, TValue>(MemberInfo member)
        => Poco.Global.GetReadFunc<TInstance, TValue>(member);
    /// <summary>
    /// 写属性
    /// </summary>
    /// <typeparam name="TInstance"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="member"></param>
    /// <returns></returns>
    public static Action<TInstance, TValue> GetWriteAction<TInstance, TValue>(MemberInfo member)
        => Poco.Global.GetWriteAction<TInstance, TValue>(member);
    /// <summary>
    /// 读成员
    /// </summary>
    /// <param name="member"></param>
    /// <returns></returns>
    public static Func<object, object> GetReadFunc(MemberInfo member)
        => Poco.Global.GetReadFunc(member);
    /// <summary>
    /// 写成员
    /// </summary>
    /// <param name="member"></param>
    /// <returns></returns>
    public static Action<object, object> GetWriteAction(MemberInfo member)
        => Poco.Global.GetWriteAction(member);
}
