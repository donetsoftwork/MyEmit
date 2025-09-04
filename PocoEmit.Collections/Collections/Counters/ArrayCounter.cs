using PocoEmit.Configuration;
using System;

namespace PocoEmit.Collections.Counters;

/// <summary>
/// 数组长度获取
/// </summary>
/// <param name="arrayType"></param>
/// <param name="elementType"></param>
public class ArrayCounter(Type arrayType, Type elementType)
    : ArrayLength
    , IEmitElementCounter
{
    /// <summary>
    /// 数组长度获取
    /// </summary>
    /// <param name="arrayType"></param>
    public ArrayCounter(Type arrayType)
        : this(arrayType, arrayType.GetElementType())
    {
    }
    #region 配置
    private readonly Type _arrayType = arrayType;
    /// <summary>
    /// 数组类型
    /// </summary>
    public Type ArrayType
        => _arrayType;
    private readonly Type _elementType = elementType;
    /// <inheritdoc />
    public Type ElementType
        => _elementType;
    bool ICompileInfo.Compiled
        => false;
    #endregion
}
