using PocoEmit.Builders;
using PocoEmit.Collections.Activators;
using PocoEmit.Collections.Counters;
using PocoEmit.Collections.Visitors;
using PocoEmit.Converters;
using System;
using System.Linq.Expressions;

namespace PocoEmit.Collections.Converters;

/// <summary>
/// 集合转数组
/// </summary>
/// <param name="sourceType"></param>
/// <param name="sourceElementType"></param>
/// <param name="destType"></param>
/// <param name="destElementType"></param>
/// <param name="length"></param>
/// <param name="visitor"></param>
/// <param name="elementConverter"></param>
public class CollectionArrayConverter(Type sourceType, Type sourceElementType, Type destType, Type destElementType, IEmitElementCounter length, IEmitElementVisitor visitor, IEmitConverter elementConverter)
    : ArrayActivator(destType, destElementType,  length)
    , IEmitComplexConverter
    , IEmitConverter
{
    #region 配置
    private readonly Type _sourceType = sourceType;
    private readonly Type _sourceElementType = sourceElementType;
    private readonly IEmitElementVisitor _visitor = visitor;
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
    /// 集合访问者
    /// </summary>
    public IEmitElementVisitor Visitor
        => _visitor;
    /// <summary>
    /// 子元素转化
    /// </summary>
    public IEmitConverter ElementConverter
        => _elementConverter;
    #endregion

    /// <inheritdoc />
    Expression IEmitConverter.Convert(Expression source)
        => Convert(new(), source);
    /// <inheritdoc />
    public Expression Convert(ComplexContext cacher, Expression source)
    {
        var dest = Expression.Variable(_collectionType, "dest");
        var count = Expression.Variable(typeof(int), "count");
        var index = Expression.Variable(typeof(int), "index");
        var sourceItem = Expression.Variable(_sourceElementType, "sourceItem");

        return Expression.Block(
            [count, dest, index, sourceItem],
            Expression.Assign(count, _length.Count(source)),
            Expression.Assign(dest, New(count)),
            //Expression.Assign(index, Expression.Constant(0)),
            _visitor.Travel(source, item => CopyElement(cacher, dest, index, item, sourceItem, _elementConverter)),
            dest
        );
    }
    /// <summary>
    /// 复制子元素
    /// </summary>
    /// <param name="cacher"></param>
    /// <param name="dest"></param>
    /// <param name="index"></param>
    /// <param name="item"></param>
    /// <param name="sourceItem"></param>
    /// <param name="converter"></param>
    /// <returns></returns>
    public Expression CopyElement(ComplexContext cacher, Expression dest, Expression index, Expression item, ParameterExpression sourceItem, IEmitConverter converter)
        => Expression.Block(
            Expression.Assign(sourceItem, item),
            Expression.Assign(Expression.ArrayAccess(dest, Expression.PostIncrementAssign(index)), cacher.Convert(converter, sourceItem, _elementType))
            );
}