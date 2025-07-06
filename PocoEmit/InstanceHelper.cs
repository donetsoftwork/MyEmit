using System;

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
    public static Func<TInstance, TValue> EmitGetter<TInstance, TValue>(string memberName)
    {
        var instanceType = typeof(TInstance);
        var property = ReflectionHelper.GetProperty(instanceType, memberName);
        if(property is not null && property.CanRead)
            return InstancePropertyHelper.EmitGetter<TInstance, TValue>(property);
        return InstanceFieldHelper.EmitGetter<TInstance, TValue>(memberName);
    }
    /// <summary>
    /// 写入实例成员
    /// </summary>
    /// <typeparam name="TInstance"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="memberName"></param>
    /// <returns></returns>
    public static Action<TInstance, TValue> EmitSetter<TInstance, TValue>(string memberName)
    {
        var instanceType = typeof(TInstance);
        var property = ReflectionHelper.GetProperty(instanceType, memberName);
        if (property is not null && property.CanWrite)
            return InstancePropertyHelper.EmitSetter<TInstance, TValue>(property);
        return InstanceFieldHelper.EmitSetter<TInstance, TValue>(memberName);
    }
}
