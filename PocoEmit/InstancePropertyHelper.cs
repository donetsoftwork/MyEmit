using System;
using System.Linq.Expressions;
using System.Reflection;

namespace PocoEmit;

/// <summary>
/// Emit实例属性帮助类
/// </summary>
public static class InstancePropertyHelper
{
    /// <summary>
    /// 读属性
    /// </summary>
    /// <typeparam name="TInstance"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="propertyName"></param>
    /// <returns></returns>
    public static Func<TInstance, TValue> EmitGetter<TInstance, TValue>(string propertyName)
    {
        var property = ReflectionHelper.GetProperty(typeof(TInstance), propertyName);
        if (property is null || !property.CanRead)
            return null;
        return EmitGetter<TInstance, TValue>(property);
    }
    /// <summary>
    /// 写属性
    /// </summary>
    /// <typeparam name="TInstance"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="propertyName"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public static Action<TInstance, TValue> EmitSetter<TInstance, TValue>(string propertyName)
    {
        var property = ReflectionHelper.GetProperty(typeof(TInstance), propertyName);
        if (property is null || !property.CanWrite)
            return null;
        return EmitSetter<TInstance, TValue>(property);
    }
    /// <summary>
    /// 读属性
    /// </summary>
    /// <typeparam name="TInstance"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="property"></param>
    /// <returns></returns>
    public static Func<TInstance, TValue> EmitGetter<TInstance, TValue>(PropertyInfo property)
    {
        var instanceType = typeof(TInstance);
        var instance = Expression.Parameter(typeof(TInstance), "instance");
#if (NETSTANDARD1_1 || NETSTANDARD1_3 || NETSTANDARD1_6)
        var declaringType = property.DeclaringType;
#else
        var declaringType = property.ReflectedType;
#endif
        var getProperty = EmitGetter(ReflectionHelper.CheckInstanceType(instance, instanceType, declaringType), property);
        var result = ReflectionHelper.CheckValueType(getProperty, property.PropertyType, typeof(TValue));
        var lambda = Expression.Lambda<Func<TInstance, TValue>>(result, instance);
        return lambda.Compile();
    }
    /// <summary>
    /// 读属性
    /// </summary>
    /// <param name="instance"></param>
    /// <param name="property"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public static Expression EmitGetter(Expression instance, PropertyInfo property)
    {
#if (NETSTANDARD1_1 || NETSTANDARD1_3 || NETSTANDARD1_6)
        var method = property.GetMethod
#else
        var method = property.GetGetMethod()
#endif
            ?? throw new ArgumentException($"Property '{property.Name}' does not have a getter method.");
        return Expression.Property(instance, method);
    }

    /// <summary>
    /// 写属性
    /// </summary>
    /// <typeparam name="TInstance"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="property"></param>
    /// <returns></returns>
    public static Action<TInstance, TValue> EmitSetter<TInstance, TValue>(PropertyInfo property)
    {
        var instanceType = typeof(TInstance);
        var instance = Expression.Parameter(instanceType, "instance");
        var valueType = typeof(TValue);
        var value = Expression.Parameter(valueType, "value");
#if (NETSTANDARD1_1 || NETSTANDARD1_3 || NETSTANDARD1_6)
        var declaringType = property.DeclaringType;
#else
        var declaringType = property.ReflectedType;
#endif
        var setProperty = EmitSetter(ReflectionHelper.CheckInstanceType(instance, instanceType, declaringType), property, ReflectionHelper.CheckValueType(value, valueType, property.PropertyType));
        var lambda = Expression.Lambda<Action<TInstance, TValue>>(setProperty, instance, value);
        return lambda.Compile();
    }
    /// <summary>
    /// 写属性
    /// </summary>
    /// <param name="instance"></param>
    /// <param name="property"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public static Expression EmitSetter(Expression instance, PropertyInfo property, Expression value)
    {
#if (NETSTANDARD1_1 || NETSTANDARD1_3)
        var method = property.SetMethod
#else
        var method = property.GetSetMethod()
#endif
            ?? throw new ArgumentException($"Property '{property.Name}' does not have a setter method.");
        return Expression.Assign(Expression.Property(instance, method), value);
    }
}
