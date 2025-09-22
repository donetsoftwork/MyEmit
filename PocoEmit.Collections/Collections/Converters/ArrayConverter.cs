using PocoEmit.Builders;
using PocoEmit.Collections.Activators;
using PocoEmit.Collections.Counters;
using PocoEmit.Complexes;
using PocoEmit.Configuration;
using PocoEmit.Converters;
using PocoEmit.Resolves;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace PocoEmit.Collections.Converters;

/// <summary>
/// 数组转数组
/// </summary>
/// <param name="sourceType"></param>
/// <param name="sourceElementType"></param>
/// <param name="destType"></param>
/// <param name="destElementType"></param>
/// <param name="elementConverter"></param>
public class ArrayConverter(Type sourceType, Type sourceElementType, Type destType, Type destElementType, IEmitConverter elementConverter)
    : ArrayActivator(destType, destElementType, ArrayLength.Length)
    , IComplexIncludeConverter
{
    #region 配置
    private readonly PairTypeKey _key = new(sourceType, destType);
    private readonly Type _sourceType = sourceType;
    private readonly Type _sourceElementType = sourceElementType;
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
    #endregion
    /// <inheritdoc />
    public Expression Convert(IBuildContext context, Expression source)
    {
        var dest = Expression.Variable(_collectionType, "dest");
        var count = Expression.Variable(typeof(int), "count");
        var index = Expression.Variable(typeof(int), "index");
        var sourceItem = Expression.Variable(_sourceElementType, "sourceItem");

        var list = new List<Expression>() {
            Expression.Assign(count, Expression.ArrayLength(source)),
            Expression.Assign(index, Expression.Constant(0)),
            Expression.Assign(dest, New(count))
        };
        var cache = context.SetCache(_key, source, dest);
        if (cache is not null)
            list.Add(cache);
        return Expression.Block(
        [count, dest, index, sourceItem],
        [
            ..list,
            EmitHelper.For(index, count, i => CopyElement(context, source, dest, i, sourceItem, _elementConverter)),
            dest
        ]
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
    /// <param name="converter"></param>
    /// <returns></returns>
    public static Expression CopyElement(IBuildContext context, Expression source, Expression dest, Expression index, ParameterExpression sourceItem, IEmitConverter converter)
        => Expression.Block(
            Expression.Assign(sourceItem, Expression.ArrayIndex(source, index)),
            Expression.Assign(Expression.ArrayAccess(dest, index), context.Convert(converter, sourceItem))
            );
    /// <inheritdoc />
    IEnumerable<ComplexBundle> IComplexPreview.Preview(IComplexBundle parent)
        => parent.Visit(_elementConverter);
}
