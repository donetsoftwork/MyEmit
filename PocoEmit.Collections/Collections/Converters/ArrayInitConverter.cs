using PocoEmit.Complexes;
using PocoEmit.Converters;
using System;
using System.Collections.Generic;
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
    , IComplexIncludeConverter
{
    #region 配置
    private readonly IEmitConverter _elementConverter = elementConverter;
    /// <summary>
    /// 子元素转化
    /// </summary>
    public IEmitConverter ElementConverter
        => _elementConverter;
    #endregion
    /// <inheritdoc />
    public Expression Convert(IBuildContext context, Expression source)
        => Expression.NewArrayInit(_elementType, CheckElement(context, source));
    /// <summary>
    /// 检查子元素
    /// </summary>
    /// <param name="context"></param>
    /// <param name="source"></param>
    /// <returns></returns>
    private Expression CheckElement(IBuildContext context, Expression source)
        => context.Convert(_elementConverter, source);
    /// <inheritdoc />
    IEnumerable<ComplexBundle> IComplexPreview.Preview(IComplexBundle parent)
        => parent.Visit(_elementConverter);
}
