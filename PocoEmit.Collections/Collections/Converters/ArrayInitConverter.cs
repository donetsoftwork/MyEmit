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
    , IEmitComplexConverter
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
    Expression IEmitConverter.Convert(Expression source)
        => Convert(new(), source);
    /// <inheritdoc />
    public Expression Convert(ComplexContext cacher, Expression source)
        => Expression.NewArrayInit(_elementType, CheckElement(cacher, source));
    /// <summary>
    /// 检查子元素
    /// </summary>
    /// <param name="cacher"></param>
    /// <param name="source"></param>
    /// <returns></returns>
    private Expression CheckElement(ComplexContext cacher, Expression source)
        => cacher.Convert(_elementConverter, source, _elementType);
}
