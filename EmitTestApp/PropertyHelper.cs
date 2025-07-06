using System.Globalization;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;

namespace PocoEmit;

/// <summary>
/// Emit实例属性帮助类
/// </summary>
public class PropertyHelper
{
    private static int _next = 0;
#if (NETSTANDARD1_1 || NETSTANDARD1_3 || NETSTANDARD1_6)
    /// <summary>
    /// 获取属性信息
    /// </summary>
    /// <param name="type"></param>
    /// <param name="propertyName"></param>
    /// <returns></returns>
    public static PropertyInfo GetProperty(Type type, string propertyName)
        => type.GetTypeInfo().GetDeclaredProperty(propertyName);
#else
    /// <summary>
    /// 获取属性信息
    /// </summary>
    /// <param name="type"></param>
    /// <param name="propertyName"></param>
    /// <returns></returns>
    public static PropertyInfo? GetProperty(Type type, string propertyName)
        => type.GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Public);
#endif
    /// <summary>
    /// 读属性
    /// </summary>
    /// <typeparam name="TInstance"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="propertyName"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public static Func<TInstance, TValue> EmitGetter<TInstance, TValue>(string propertyName)
        => EmitGetter<TInstance, TValue>(GetProperty(typeof(TInstance), propertyName)
            ?? throw new ArgumentException($"Property '{propertyName}' not found on type '{typeof(TInstance)}'."));
    /// <summary>
    /// 写属性
    /// </summary>
    /// <typeparam name="TInstance"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="propertyName"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public static Action<TInstance, TValue> EmitSetter<TInstance, TValue>(string propertyName)
        => EmitSetter<TInstance, TValue>(GetProperty(typeof(TInstance), propertyName)
            ?? throw new ArgumentException($"Property '{propertyName}' not found on type '{typeof(TInstance)}'."));
    /// <summary>
    /// 读属性
    /// </summary>
    /// <typeparam name="TInstance"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="property"></param>
    /// <returns></returns>
    public static Func<TInstance, TValue> EmitGetter<TInstance, TValue>(PropertyInfo property)
    {
#if (NETSTANDARD1_1 || NETSTANDARD1_3 || NETSTANDARD1_6)
        var method = property.GetMethod
#else
        var method = property.GetGetMethod()
#endif
            ?? throw new ArgumentException($"Property '{property.Name}' does not have a getter method.");
        int uniqueIdentifier = Interlocked.Increment(ref _next);

        var instanceType = typeof(TInstance);
#if (NETSTANDARD1_1 || NETSTANDARD1_3 || NETSTANDARD1_6)
        var declaringType = property.DeclaringType;
#else
        var declaringType = property.ReflectedType!;
#endif
        var valueType = typeof(TValue);
        var dynamicMethod = new DynamicMethod("dynamicGet_" + uniqueIdentifier.ToString(CultureInfo.InvariantCulture), valueType, [instanceType]);
        var il = dynamicMethod.GetILGenerator();
        il.Emit(OpCodes.Ldarg_0);
        if (instanceType != declaringType)
        {
            il.Emit(OpCodes.Castclass, declaringType);
        }
        if (method.IsVirtual)
            il.Emit(OpCodes.Callvirt, method);
        else
            il.Emit(OpCodes.Call, method);
        var propertyType = property.PropertyType;
        if (valueType != propertyType)
        {
            if (propertyType.IsValueType)
            {
                if (valueType.IsValueType)
                {
                    il.Emit(OpCodes.Castclass, valueType);
                }
                else
                {
                    // 如果是引用类型或object，则需要装箱
                    il.Emit(OpCodes.Box, propertyType);
                }
            }
            else
            {
                il.Emit(OpCodes.Castclass, valueType);
            }
        }
        il.Emit(OpCodes.Ret);
        // 创建一个委托
        var getPropertyDelegate = (Func<TInstance, TValue>)dynamicMethod.CreateDelegate(typeof(Func<TInstance, TValue>));
        return getPropertyDelegate;
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
#if (NETSTANDARD1_1 || NETSTANDARD1_3)
        var method = property.SetMethod
#else
        var method = property.GetSetMethod()
#endif
            ?? throw new ArgumentException($"Property '{property.Name}' does not have a setter method.");
        var instanceType = typeof(TInstance);
        var instance = Expression.Parameter(instanceType, "instance");
        var valueType = typeof(TValue);
        var value = Expression.Parameter(valueType, "value");
#if (NETSTANDARD1_1 || NETSTANDARD1_3 || NETSTANDARD1_6)
        var declaringType = property.DeclaringType;
#else
        var declaringType = property.ReflectedType;
#endif
        var propertyType = property.PropertyType;

        var setProperty = Expression.Property(instanceType == declaringType ? instance : Expression.Convert(instance, declaringType), method);
        var assign = Expression.Assign(setProperty, valueType == propertyType ? value : Expression.Convert(value, propertyType));
        var lambda = Expression.Lambda<Action<TInstance, TValue>>(assign, instance, value);
        return lambda.Compile();
    }
}
