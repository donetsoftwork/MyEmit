using PocoEmit.Builders;
using PocoEmit.Collections.Activators;
using PocoEmit.Collections.Counters;
using PocoEmit.Configuration;
using PocoEmit.Converters;
using System;
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
    : ArrayActivator(destType, destElementType, ArrayLength.Length), IEmitConverter
{
    #region 配置
    private readonly Type _sourceType = sourceType;
    private readonly Type _sourceElementType = sourceElementType;
    private readonly IEmitConverter _elementConverter = elementConverter;
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
    /// <inheritdoc />
    bool ICompileInfo.Compiled
        => false;
    #endregion

    /// <inheritdoc />
    public Expression Convert(Expression source)
    {
        var dest = Expression.Variable(_collectionType, "dest");        
        var count = Expression.Variable(typeof(int), "count");
        var index = Expression.Variable(typeof(int), "index");
        var sourceItem = Expression.Variable(_sourceElementType, "sourceItem");
        return Expression.Block(
            [count, dest, index, sourceItem],
            Expression.Assign(count, Expression.ArrayLength(source)),
            Expression.Assign(dest, New(count)),
            //Expression.Assign(index, Expression.Constant(0)),
            EmitHelper.For(index, count, i => CopyElement(source, dest, i, sourceItem, _elementConverter)),
            dest
        );
    }
    /// <summary>
    /// 复制子元素
    /// </summary>
    /// <param name="source"></param>
    /// <param name="dest"></param>
    /// <param name="index"></param>
    /// <param name="sourceItem"></param>
    /// <param name="converter"></param>
    /// <returns></returns>
    public static Expression CopyElement(Expression source, Expression dest, Expression index, ParameterExpression sourceItem, IEmitConverter converter)
        => Expression.Block(
            Expression.Assign(sourceItem, Expression.ArrayIndex(source, index)),
            Expression.Assign(Expression.ArrayAccess(dest, index), converter.Convert(sourceItem))
            );
}
