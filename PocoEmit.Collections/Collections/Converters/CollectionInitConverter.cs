using PocoEmit.Collections.Saves;
using PocoEmit.Complexes;
using PocoEmit.Configuration;
using PocoEmit.Converters;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace PocoEmit.Collections.Converters;

/// <summary>
/// 集合直接初始化转化
/// </summary>
/// <param name="collectionType"></param>
/// <param name="elementType"></param>
/// <param name="saver"></param>
/// <param name="elementConverter"></param>
public sealed class CollectionInitConverter(Type collectionType, Type elementType, IEmitElementSaver saver, IEmitConverter elementConverter)
    : EmitCollectionBase(collectionType, elementType)
    , IComplexIncludeConverter
{
    #region 配置
    private readonly PairTypeKey _key = new(elementType, collectionType);
    private readonly IEmitElementSaver _saver = saver;
    private readonly IEmitConverter _elementConverter = elementConverter;
    /// <inheritdoc />
    public PairTypeKey Key
        => _key;
    /// <summary>
    /// 集合元素保存器
    /// </summary>
    public IEmitElementSaver Saver
        => _saver;
    /// <summary>
    /// 子元素转化
    /// </summary>
    public IEmitConverter ElementConverter
        => _elementConverter;
    #endregion
    /// <inheritdoc />
    IEnumerable<ComplexBundle> IComplexPreview.Preview(IComplexBundle parent)
        => parent.Visit(_elementConverter);
    /// <inheritdoc />
    public Expression Convert(IBuildContext context, Expression source)
        => Expression.ListInit(Expression.New(_collectionType), Expression.ElementInit(_saver.AddMethod, CheckElement(context, source)));
    /// <summary>
    /// 检查子元素
    /// </summary>
    /// <param name="context"></param>
    /// <param name="source"></param>
    /// <returns></returns>
    private Expression CheckElement(IBuildContext context, Expression source)
        => context.Convert(_elementConverter, source);
}
