using PocoEmit.Builders;
using PocoEmit.Collections.Activators;
using PocoEmit.Collections.Counters;
using PocoEmit.Complexes;
using PocoEmit.Configuration;
using PocoEmit.Converters;
using PocoEmit.Indexs;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace PocoEmit.Collections.Converters;

/// <summary>
/// 索引转数组
/// </summary>
/// <param name="sourceType"></param>
/// <param name="sourceElementType"></param>
/// <param name="destType"></param>
/// <param name="destElementType"></param>
/// <param name="length"></param>
/// <param name="indexReader"></param>
/// <param name="elementConverter"></param>
public class IndexArrayConverter(Type sourceType, Type sourceElementType, Type destType, Type destElementType, IEmitElementCounter length, IEmitIndexMemberReader indexReader, IEmitConverter elementConverter)
    : ArrayActivator(destType, destElementType, length)
    , IComplexIncludeConverter
{
    #region 配置
    private readonly PairTypeKey _key = new(sourceType, destType);
    private readonly Type _sourceType = sourceType;
    private readonly Type _sourceElementType = sourceElementType;
    private readonly IEmitIndexMemberReader _indexReader = indexReader;
    private readonly IEmitConverter _elementConverter = elementConverter;
    /// <inheritdoc />
    public PairTypeKey Key
        => _key;
    /// <summary>
    /// 源类型
    /// </summary>
    public Type SourceType
        => _sourceType;
    /// <summary>
    /// 源子元素类型
    /// </summary>
    public Type SourceElementType
        => _sourceElementType;
    /// <summary>
    /// 子元素转化
    /// </summary>
    public IEmitConverter ElementConverter
        => _elementConverter;
    /// <summary>
    /// 源索引读取器
    /// </summary>
    public IEmitIndexMemberReader IndexReader
        => _indexReader;
    #endregion
    /// <inheritdoc />
    IEnumerable<ComplexBundle> IComplexPreview.Preview(IComplexBundle parent)
        => parent.Visit(_elementConverter);
    /// <inheritdoc />
    public Expression Convert(IBuildContext context, Expression source)
    {
        var dest = Expression.Variable(_collectionType, "dest");
        var count = Expression.Variable(typeof(int), "count");
        var index = Expression.Variable(typeof(int), "index");
        var sourceItem = Expression.Variable(_sourceElementType, "sourceItem");
        return Expression.Block(
            [count, dest, index, sourceItem],
            Expression.Assign(count, _length.Count(source)),
            Expression.Assign(index, Expression.Constant(0)),
            Expression.Assign(dest, New(count)),
            //Expression.Assign(index, Expression.Constant(0)),
            EmitHelper.For(index, count, i => CopyElement(context, source, dest, i, sourceItem, _indexReader, _elementConverter)),
            dest
        );
    }
    /// <summary>
    /// 复制子元素
    /// </summary>
    /// <param name="context"></param>
    /// <param name="source"></param>
    /// <param name="dest"></param>
    /// <param name="index"></param>
    /// <param name="sourceItem"></param>
    /// <param name="sourceReader"></param>
    /// <param name="converter"></param>
    /// <returns></returns>
    public static Expression CopyElement(IBuildContext context, Expression source, Expression dest, Expression index, ParameterExpression sourceItem, IEmitIndexMemberReader sourceReader, IEmitConverter converter)
        => Expression.Block(
            Expression.Assign(sourceItem, sourceReader.Read(source, index)),
            Expression.Assign(Expression.ArrayAccess(dest, index), context.Convert(converter, sourceItem))
            );
}
