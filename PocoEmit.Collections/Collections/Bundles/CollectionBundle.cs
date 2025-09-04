using System;
using System.Reflection;

namespace PocoEmit.Collections.Bundles;

#if NET7_0_OR_GREATER
/// <summary>
/// 集合成员
/// </summary>
/// <param name="IsInterface">是否接口</param>
/// <param name="ElementType">子元素类型</param>
/// <param name="CapacityConstructor">容量构造函数</param>
/// <param name="AddMethod">添加方法</param>
/// <param name="Count">数量属性</param>
public record CollectionBundle(bool IsInterface, Type ElementType, ConstructorInfo CapacityConstructor, MethodInfo AddMethod, PropertyInfo Count);
#else
/// <summary>
/// 集合成员
/// </summary>
/// <param name="isInterface">是否接口</param>
/// <param name="elementType">子元素类型</param>
/// <param name="capacityConstructor">容量构造函数</param>
/// <param name="addMethod">添加方法</param>
/// <param name="count">数量属性</param>
public class CollectionBundle(bool isInterface, Type elementType, ConstructorInfo capacityConstructor, MethodInfo addMethod, PropertyInfo count)
{
    /// <summary>
    /// 是否接口
    /// </summary>
    public bool IsInterface { get; } = isInterface;
    /// <summary>
    /// 子元素类型
    /// </summary>
    public Type ElementType { get; } = elementType;
    /// <summary>
    /// 容量构造函数
    /// </summary>
    public ConstructorInfo CapacityConstructor { get; } = capacityConstructor;
    /// <summary>
    /// 添加方法
    /// </summary>
    public MethodInfo AddMethod { get; } = addMethod;
    /// <summary>
    /// 数量属性
    /// </summary>
    public PropertyInfo Count { get; } = count;
}
#endif
