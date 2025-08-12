using PocoEmit.Configuration;
using System;

namespace PocoEmit.Collections.Counters;

/// <summary>
/// 数组长度获取
/// </summary>
/// <param name="arrayType"></param>
public class ArrayCounter(Type arrayType)
    : ArrayLength
    , IEmitCollectionCounter
{
    #region 配置
    private readonly Type _arrayType = arrayType;
    /// <summary>
    /// 数组类型
    /// </summary>
    public Type ArrayType
        => _arrayType;

    Type IEmitCollectionCounter.CollectionType
        => _arrayType;
    bool ICompileInfo.Compiled
        => false;
    #endregion
}
