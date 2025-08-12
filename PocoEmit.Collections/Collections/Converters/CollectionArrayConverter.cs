using PocoEmit.Collections.Activators;
using PocoEmit.Collections.Visitors;
using PocoEmit.Converters;
using System;
using System.Linq.Expressions;

namespace PocoEmit.Collections.Converters;

/// <summary>
/// 集合转数组
/// </summary>
/// <param name="elementType"></param>
/// <param name="collectionType"></param>
/// <param name="length"></param>
/// <param name="visitor"></param>
/// <param name="elementConverter"></param>
public class CollectionArrayConverter(Type elementType, Type collectionType, IEmitCollectionCounter length, ICollectionVisitor visitor, IEmitConverter elementConverter)
    : ArrayActivator(elementType, collectionType, length)
    , IEmitConverter
{
    #region 配置
    private readonly ICollectionVisitor _visitor = visitor;
    private readonly IEmitConverter _elementConverter = elementConverter;
    /// <summary>
    /// 集合访问者
    /// </summary>
    public ICollectionVisitor Visitor
        => _visitor;
    /// <summary>
    /// 子元素转化
    /// </summary>
    public IEmitConverter ElementConverter
        => _elementConverter;
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
            _visitor.Travel(source, item => CopyElement(dest, index, item, _elementConverter)),
            // Expression.Return(returnTarget, dest),
            Expression.Label(destTarget, dest)
        );
    }
    /// <summary>
    /// 复制子元素
    /// </summary>
    /// <param name="dest"></param>
    /// <param name="index"></param>
    /// <param name="item"></param>
    /// <param name="converter"></param>
    /// <returns></returns>
    public static Expression CopyElement(Expression dest, Expression index, Expression item, IEmitConverter converter)
        => Expression.Assign(Expression.ArrayAccess(dest, Expression.PostIncrementAssign(index)), converter.Convert(item));
}