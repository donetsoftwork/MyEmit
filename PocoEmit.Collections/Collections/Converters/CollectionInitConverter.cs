using PocoEmit.Collections.Saves;
using PocoEmit.Configuration;
using PocoEmit.Converters;
using System;
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
    , IEmitComplexConverter
    , IEmitConverter
{
    #region 配置
    private readonly IEmitElementSaver _saver = saver;
    private readonly IEmitConverter _elementConverter = elementConverter;
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
    /// <inheritdoc />
    bool ICompileInfo.Compiled
        => false;
    #endregion
    /// <inheritdoc />
    Expression IEmitConverter.Convert(Expression source)
        => Convert(new(), source);
    /// <inheritdoc />
    public Expression Convert(ComplexContext cacher, Expression source)
        => Expression.ListInit(Expression.New(_collectionType), Expression.ElementInit(_saver.AddMethod, CheckElement(cacher, source)));
    /// <summary>
    /// 检查子元素
    /// </summary>
    /// <param name="cacher"></param>
    /// <param name="source"></param>
    /// <returns></returns>
    private Expression CheckElement(ComplexContext cacher, Expression source)
        => cacher.Convert(_elementConverter, source, _elementType);
}
