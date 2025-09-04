using System;
using System.Reflection;

namespace PocoEmit.Collections.Bundles;

#if NET7_0_OR_GREATER
/// <summary>
/// 列表成员
/// </summary>
/// <param name="Items">索引器属性</param>
/// <param name="IsInterface">是否接口</param>
/// <param name="ElementType">子元素类型</param>
/// <param name="CapacityConstructor">容量构造函数</param>
/// <param name="AddMethod">添加方法</param>
/// <param name="Count">数量属性</param>
public record ListBundle(PropertyInfo Items, bool IsInterface, Type ElementType, ConstructorInfo CapacityConstructor, MethodInfo AddMethod, PropertyInfo Count)
    : CollectionBundle(IsInterface, ElementType, CapacityConstructor, AddMethod, Count);
#else
/// <summary>
/// 列表成员
/// </summary>
/// <param name="items">索引器属性</param>
/// <param name="isInterface">是否接口</param>
/// <param name="elementType">子元素类型</param>
/// <param name="capacityConstructor">容量构造函数</param>
/// <param name="addMethod">添加方法</param>
/// <param name="count">数量属性</param>
public class ListBundle(PropertyInfo items, bool isInterface, Type elementType, ConstructorInfo capacityConstructor, MethodInfo addMethod, PropertyInfo count)
    : CollectionBundle(isInterface, elementType, capacityConstructor, addMethod, count)
{
    /// <summary>
    /// 索引器属性
    /// </summary>
    public PropertyInfo Items { get;  } = items;
}
#endif
