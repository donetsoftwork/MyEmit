using System;
using System.Reflection;

namespace PocoEmit.Collections.Bundles;

#if NET7_0_OR_GREATER
/// <summary>
/// 字典成员
/// </summary>
/// <param name="KeyType">键类型</param>
/// <param name="ValueType">值类型</param>
/// <param name="Keys">键属性</param>
/// <param name="Values">值属性</param>
/// <param name="Items">索引器属性</param>
/// <param name="Count">数量属性</param>
public record DictionaryBundle(Type KeyType, Type ValueType, PropertyInfo Keys, PropertyInfo Values, PropertyInfo Items, PropertyInfo Count);
#else
/// <summary>
/// 字典成员
/// </summary>
/// <param name="keyType">键类型</param>
/// <param name="valueType">值类型</param>
/// <param name="keys">键属性</param>
/// <param name="values">值属性</param>
/// <param name="items">索引器属性</param>
/// <param name="count">数量属性</param>
public class DictionaryBundle(Type keyType, Type valueType, PropertyInfo keys, PropertyInfo values, PropertyInfo items, PropertyInfo count)
{
    /// <summary>
    /// 键类型
    /// </summary>
    public Type KeyType { get; } = keyType;
    /// <summary>
    /// 值类型
    /// </summary>
    public Type ValueType { get; } = valueType;
    /// <summary>
    /// 键属性
    /// </summary>
    public PropertyInfo Keys { get; } = keys;
    /// <summary>
    /// 值属性
    /// </summary>
    public PropertyInfo Values { get;  } = values;
    /// <summary>
    /// 索引器属性
    /// </summary>
    public PropertyInfo Items { get;  } = items;
    /// <summary>
    /// 数量属性
    /// </summary>
    public PropertyInfo Count { get;  } = count;
}
#endif
