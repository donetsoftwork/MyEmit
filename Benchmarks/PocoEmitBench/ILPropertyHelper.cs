using System.Globalization;
using System.Reflection;
using System.Reflection.Emit;

namespace PocoEmitBench;

/// <summary>
/// Emit实例属性帮助类
/// </summary>
public class ILPropertyHelper
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
    public static Func<TInstance, TValue> GetReadFunc<TInstance, TValue>(string propertyName)
        => GetReadFunc<TInstance, TValue>(GetProperty(typeof(TInstance), propertyName)
            ?? throw new ArgumentException($"Property '{propertyName}' not found on type '{typeof(TInstance)}'."));
    /// <summary>
    /// 写属性
    /// </summary>
    /// <typeparam name="TInstance"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="propertyName"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public static Action<TInstance, TValue> GetWriteAction<TInstance, TValue>(string propertyName)
        => GetWriteAction<TInstance, TValue>(GetProperty(typeof(TInstance), propertyName)
            ?? throw new ArgumentException($"Property '{propertyName}' not found on type '{typeof(TInstance)}'."));
    /// <summary>
    /// 读属性
    /// </summary>
    /// <typeparam name="TInstance"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="property"></param>
    /// <returns></returns>
    public static Func<TInstance, TValue> GetReadFunc<TInstance, TValue>(PropertyInfo property)
    {
        var method = property.GetGetMethod()
            ?? throw new ArgumentException($"Property '{property.Name}' does not have a getter method.");       

        var instanceType = typeof(TInstance);
        var declaringType = property.ReflectedType!;
        var valueType = typeof(TValue);
        int uniqueIdentifier = Interlocked.Increment(ref _next);
        var dynamicMethod = new DynamicMethod("dynamicGet_" + uniqueIdentifier.ToString(CultureInfo.InvariantCulture), valueType, [instanceType]);
        var il = dynamicMethod.GetILGenerator();
        il.Emit(OpCodes.Ldarg_0);
        if (!CheckDeclaringType(instanceType, declaringType))
            il.Emit(OpCodes.Castclass, declaringType);
        if (method.IsVirtual)
            il.Emit(OpCodes.Callvirt, method);
        else
            il.Emit(OpCodes.Call, method);
        ConvertType(il, property.PropertyType, valueType);        
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
    public static Action<TInstance, TValue> GetWriteAction<TInstance, TValue>(PropertyInfo property)
    {
        var method = property.GetSetMethod()
            ?? throw new ArgumentException($"Property '{property.Name}' does not have a setter method.");
        var instanceType = typeof(TInstance);
        var valueType = typeof(TValue);
        var declaringType = property.ReflectedType!;
        var propertyType = property.PropertyType;
        int uniqueIdentifier = Interlocked.Increment(ref _next);
        var dynamicMethod = new DynamicMethod("dynamicSet_" + uniqueIdentifier.ToString(CultureInfo.InvariantCulture), typeof(void), [instanceType, valueType]);
        var il = dynamicMethod.GetILGenerator();
        il.Emit(OpCodes.Ldarg_0);
        if (!CheckDeclaringType(instanceType, declaringType))
            il.Emit(OpCodes.Castclass, declaringType);
        il.Emit(OpCodes.Ldarg_1);
        ConvertType(il, valueType, propertyType);
        if (method.IsVirtual)
            il.Emit(OpCodes.Callvirt, method);
        else
            il.Emit(OpCodes.Call, method);
        il.Emit(OpCodes.Nop);
        il.Emit(OpCodes.Ret);
        // 创建一个委托
        var getPropertyDelegate = (Action<TInstance, TValue>)dynamicMethod.CreateDelegate(typeof(Action<TInstance, TValue>));
        return getPropertyDelegate;
    }

    static void ConvertType(ILGenerator il, Type fromType, Type toType)
    {
        if (fromType == toType) 
            return;
        
        if (fromType.IsValueType)
        {
            if (toType.IsValueType)
            {
                if (!toType.IsAssignableFrom(fromType))
                    il.Emit(OpCodes.Castclass, toType);
            }
            else
            {
                // 如果是引用类型或object，则需要装箱
                il.Emit(OpCodes.Box, toType);
            }
        }
        else if (toType.IsValueType)
        {
            // 如果是引用类型转值类型，则需要拆箱
            il.Emit(OpCodes.Unbox_Any, toType);
        }
        else if (!toType.IsAssignableFrom(fromType))
        {
            // 如果是引用类型或object，则需要转换
            il.Emit(OpCodes.Castclass, toType);
        }
    }

    /// <summary>
    /// 判断是否兼容定义类型
    /// </summary>
    /// <param name="instanceType"></param>
    /// <param name="declaringType"></param>
    /// <returns></returns>
    public static bool CheckDeclaringType(Type instanceType, Type declaringType)
#if (NETSTANDARD1_1 || NETSTANDARD1_3 || NETSTANDARD1_6)
        => CheckDeclaringType(instanceType.GetTypeInfo(), declaringType.GetTypeInfo());
    /// <summary>
    /// 判断是否兼容定义类型
    /// </summary>
    /// <param name="instanceType"></param>
    /// <param name="declaringType"></param>
    /// <returns></returns>
    public static bool CheckDeclaringType(TypeInfo instanceType, TypeInfo declaringType)
#endif
        => instanceType == declaringType || declaringType.IsAssignableFrom(instanceType);
}
