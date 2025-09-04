using PocoEmit.Collections;
using System;
using System.Collections.Generic;

namespace PocoEmit.Dictionaries;

/// <summary>
/// 字典基类
/// </summary>
/// <param name="dictionaryType"></param>
/// <param name="keyType"></param>
/// <param name="elementType"></param>
public abstract class EmitDictionaryBase(Type dictionaryType, Type keyType, Type elementType)
    : EmitCollectionBase(dictionaryType, elementType)
{
    #region 配置
    /// <summary>
    /// 键类型
    /// </summary>
    protected readonly Type _keyType = keyType;
    /// <summary>
    /// 键类型
    /// </summary>
    public Type KeyType 
        => _keyType;
    #endregion
    /// <summary>
    /// 键值对类型
    /// </summary>
    /// <param name="keyType"></param>
    /// <param name="elementType"></param>
    /// <returns></returns>
    public static Type MakePairType(Type keyType, Type elementType)
        => typeof(KeyValuePair<,>).MakeGenericType(keyType, elementType);
    /// <summary>
    /// 键值对类型
    /// </summary>
    /// <param name="types"></param>
    /// <returns></returns>
    public static Type MakePairType(Type[] types)
        => typeof(KeyValuePair<,>).MakeGenericType(types);
}
