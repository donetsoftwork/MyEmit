using PocoEmit.Configuration;
using PocoEmit.Converters;
using System;
using System.Linq.Expressions;

namespace PocoEmit.Collections.Converters;

/// <summary>
/// 数组直接初始化转化
/// </summary>
/// <param name="arrayType"></param>
/// <param name="elementType"></param>
/// <param name="elementConverter"></param>
public sealed class ArrayInitConverter(Type arrayType, Type elementType, IEmitConverter elementConverter)
    : EmitCollectionBase(arrayType, elementType)
    , IEmitConverter
{
    #region 配置
    private readonly IEmitConverter _elementConverter = elementConverter;
    /// <summary>
    /// 子元素转化
    /// </summary>
    public IEmitConverter ElementConverter
        => _elementConverter;
    /// <inheritdoc />
    bool ICompileInfo.Compiled
        => false;
    #endregion
    /// <inheritdoc />
    public Expression Convert(Expression source)
        => Expression.NewArrayInit(_elementType, CheckElement(source));
    /// <summary>
    /// 检查子元素
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    private Expression CheckElement(Expression source)
        => _elementConverter.Convert(source);
}
