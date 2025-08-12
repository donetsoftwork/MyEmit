using PocoEmit.Builders;
using PocoEmit.Collections.Activators;
using PocoEmit.Converters;
using PocoEmit.Indexs;
using PocoEmit.Members;
using System;
using System.Linq.Expressions;

namespace PocoEmit.Collections.Converters;

/// <summary>
/// 索引转数组
/// </summary>
/// <param name="elementType"></param>
/// <param name="collectionType"></param>
/// <param name="length"></param>
/// <param name="indexReader"></param>
/// <param name="elementConverter"></param>
public class IndexArrayConverter(Type elementType, Type collectionType, IEmitCollectionCounter length, IEmitIndexMemberReader indexReader, IEmitConverter elementConverter)
    : ArrayActivator(elementType, collectionType, length)
    , IEmitConverter
{
    #region 配置
    private readonly IEmitIndexMemberReader _indexReader = indexReader;
    private readonly IEmitConverter _elementConverter = elementConverter;
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
    public Expression Convert(Expression source)
    {
        var dest = Expression.Variable(_collectionType, "dest");
        var count = Expression.Variable(typeof(int), "count");
        var index = Expression.Variable(typeof(int), "index");
        LabelTarget destTarget = Expression.Label(_collectionType, "returndest");

        return Expression.Block(
            [count, dest, index],
            Expression.Assign(count, _length.Count(source)),
            Expression.Assign(dest, New(count)),
            Expression.Assign(index, Expression.Constant(0)),
            EmitHelper.For(index, count, i => CopyElement(source, dest, i, _indexReader, _elementConverter)),
            // Expression.Return(returnTarget, dest),
            Expression.Label(destTarget, dest)
        );
    }
    /// <summary>
    /// 复制子元素
    /// </summary>
    /// <param name="source"></param>
    /// <param name="dest"></param>
    /// <param name="index"></param>
    /// <param name="sourceReader"></param>
    /// <param name="converter"></param>
    /// <returns></returns>
    public static Expression CopyElement(Expression source, Expression dest, Expression index, IEmitIndexMemberReader sourceReader, IEmitConverter converter)
        => Expression.Assign(Expression.ArrayAccess(dest, index), converter.Convert(sourceReader.Read(source, index)));
}
