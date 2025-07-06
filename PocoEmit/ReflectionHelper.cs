using System;
using System.Linq.Expressions;
using System.Reflection;

namespace PocoEmit;

/// <summary>
/// 反射帮助类
/// </summary>
public static class ReflectionHelper
{
    /// <summary>
    /// 获取属性信息
    /// </summary>
    /// <param name="type"></param>
    /// <param name="propertyName"></param>
    /// <returns></returns>
    public static PropertyInfo GetProperty(Type type, string propertyName)
#if (NETSTANDARD1_1 || NETSTANDARD1_3 || NETSTANDARD1_6)
        => type.GetTypeInfo().GetDeclaredProperty(propertyName);
    /// <summary>
    /// 获取属性信息
    /// </summary>
    /// <param name="type"></param>
    /// <param name="propertyName"></param>
    /// <returns></returns>
    public static PropertyInfo GetProperty(TypeInfo type, string propertyName)
        => type.GetDeclaredProperty(propertyName);
#else
        => type.GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Public);
#endif
    /// <summary>
    /// 获取字段信息
    /// </summary>
    /// <param name="type"></param>
    /// <param name="fieldName"></param>
    /// <returns></returns>
    public static FieldInfo GetField(Type type, string fieldName)
#if (NETSTANDARD1_1 || NETSTANDARD1_3 || NETSTANDARD1_6)
        => type.GetTypeInfo().GetDeclaredField(fieldName);
    /// <summary>
    /// 获取字段信息
    /// </summary>
    /// <param name="type"></param>
    /// <param name="fieldName"></param>
    /// <returns></returns>
    public static FieldInfo GetField(TypeInfo type, string fieldName)
        => type.GetDeclaredField(fieldName);
#else
        => type.GetField(fieldName, BindingFlags.Instance | BindingFlags.Public);
#endif
    /// <summary>
    /// 判断是否兼容声明类型
    /// </summary>
    /// <param name="instanceType"></param>
    /// <param name="declaringType"></param>
    /// <returns></returns>
    public static bool CheckDeclaringType(Type instanceType, Type declaringType)
#if (NETSTANDARD1_1 || NETSTANDARD1_3 || NETSTANDARD1_6)
        => CheckDeclaringType(instanceType.GetTypeInfo(), declaringType.GetTypeInfo());
    /// <summary>
    /// 判断是否兼容声明类型
    /// </summary>
    /// <param name="instanceType"></param>
    /// <param name="declaringType"></param>
    /// <returns></returns>
    public static bool CheckDeclaringType(TypeInfo instanceType, TypeInfo declaringType)
#endif
        => instanceType == declaringType || declaringType.IsAssignableFrom(instanceType);
    /// <summary>
    /// 判断是否兼容类型
    /// </summary>
    /// <param name="fromType"></param>
    /// <param name="toType"></param>
    /// <returns></returns>
    public static bool CheckValueType(Type fromType, Type toType)
#if (NETSTANDARD1_1 || NETSTANDARD1_3 || NETSTANDARD1_6)
        => CheckValueType(fromType.GetTypeInfo(), toType.GetTypeInfo());
    /// <summary>
    /// 判断是否兼容声明类型
    /// </summary>
    /// <param name="fromType"></param>
    /// <param name="toType"></param>
    /// <returns></returns>
    public static bool CheckValueType(TypeInfo fromType, TypeInfo toType)
#endif
    {
        if (fromType == toType)
            return true;
        if (toType.IsValueType)
        {
            if (fromType.IsValueType)
                return toType.IsAssignableFrom(fromType);
            return false;
        }
        if (fromType.IsValueType)
            return false;
        return toType.IsAssignableFrom(fromType);
    }
    /// <summary>
    /// 处理实例类型
    /// </summary>
    /// <param name="instance"></param>
    /// <param name="instanceType"></param>
    /// <param name="declaringType"></param>
    /// <returns></returns>
    public static Expression CheckInstanceType(Expression instance, Type instanceType, Type declaringType)
    => CheckDeclaringType(instanceType, declaringType) ? instance : Expression.Convert(instance, declaringType);
    /// <summary>
    /// 处理值类型转换
    /// </summary>
    /// <param name="value"></param>
    /// <param name="from"></param>
    /// <param name="to"></param>
    /// <returns></returns>
    public static Expression CheckValueType(Expression value, Type from, Type to)
        => CheckValueType(from, to) ? value : Expression.Convert(value, to);
}
