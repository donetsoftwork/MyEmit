using System;
using System.Linq.Expressions;
using System.Reflection;

namespace PocoEmit;

/// <summary>
/// Emit实例字段帮助类
/// </summary>
public class InstanceFieldHelper
{
    /// <summary>
    /// 读字段
    /// </summary>
    /// <typeparam name="TInstance"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="fieldName"></param>
    /// <returns></returns>
    public static Func<TInstance, TValue> EmitGetter<TInstance, TValue>(string fieldName)
    {
        var field = ReflectionHelper.GetField(typeof(TInstance), fieldName);
        if (field is null)
            return null;
        return EmitGetter<TInstance, TValue>(field);
    }
    /// <summary>
    /// 写字段
    /// </summary>
    /// <typeparam name="TInstance"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="fieldName"></param>
    /// <returns></returns>
    public static Action<TInstance, TValue> EmitSetter<TInstance, TValue>(string fieldName)
    {
        var field = ReflectionHelper.GetField(typeof(TInstance), fieldName);
        if (field is null)
            return null;
        return EmitSetter<TInstance, TValue>(field);
    }
    /// <summary>
    /// 读字段
    /// </summary>
    /// <typeparam name="TInstance"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="field"></param>
    /// <returns></returns>
    public static Func<TInstance, TValue> EmitGetter<TInstance, TValue>(FieldInfo field)
    {
        var instanceType = typeof(TInstance);
        var instance = Expression.Parameter(typeof(TInstance), "instance");
#if (NETSTANDARD1_1 || NETSTANDARD1_3 || NETSTANDARD1_6)
        var declaringType = field.DeclaringType;
#else
        var declaringType = field.ReflectedType;
#endif
        var getField = ReflectionHelper.CheckValueType(Expression.Field(ReflectionHelper.CheckInstanceType(instance, instanceType, declaringType), field), field.FieldType, typeof(TValue));
        var lambda = Expression.Lambda<Func<TInstance, TValue>>(getField, instance);
        return lambda.Compile();
    }
    /// <summary>
    /// 写字段
    /// </summary>
    /// <typeparam name="TInstance"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="field"></param>
    /// <returns></returns>
    public static Action<TInstance, TValue> EmitSetter<TInstance, TValue>(FieldInfo field)
    {
        var instanceType = typeof(TInstance);
        var instance = Expression.Parameter(instanceType, "instance");
        var valueType = typeof(TValue);
        var value = Expression.Parameter(valueType, "value");
#if (NETSTANDARD1_1 || NETSTANDARD1_3 || NETSTANDARD1_6)
        var declaringType = field.DeclaringType;
#else
        var declaringType = field.ReflectedType;
#endif
        var setField = EmitSetter(ReflectionHelper.CheckInstanceType(instance, instanceType, declaringType), field, ReflectionHelper.CheckValueType(value, valueType, field.FieldType));
        var lambda = Expression.Lambda<Action<TInstance, TValue>>(setField, instance, value);
        return lambda.Compile();
    }
    /// <summary>
    /// 写字段
    /// </summary>
    /// <param name="instance"></param>
    /// <param name="field"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public static Expression EmitSetter(Expression instance, FieldInfo field, Expression value)
        => Expression.Assign(Expression.Field(instance, field), value);
}
