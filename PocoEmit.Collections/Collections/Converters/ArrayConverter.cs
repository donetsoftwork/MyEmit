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
/// <param name="returnType"></param>
/// <param name="elementType"></param>
/// <param name="elementConverter"></param>
public class ArrayConverter(Type returnType, Type elementType, IEmitConverter elementConverter)
    : ArrayActivator(returnType, elementType, ArrayLength.Length), IEmitConverter
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
    public Expression Convert(Expression source)
    {
        var dest = Expression.Variable(_collectionType, "dest");        
        var count = Expression.Variable(typeof(int), "count");
        var index = Expression.Variable(typeof(int), "index");
        LabelTarget destTarget = Expression.Label(_collectionType, "returndest");

        return Expression.Block(
            [count, dest, index],
            Expression.Assign(count, Expression.ArrayLength(source)),
            Expression.Assign(dest, New(count)),
            Expression.Assign(index, Expression.Constant(0)),
            EmitHelper.For(index, count, i => CopyElement(source, dest, i, _elementConverter)),
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
    /// <param name="converter"></param>
    /// <returns></returns>
    public static Expression CopyElement(Expression source, Expression dest, Expression index, IEmitConverter converter)
        => Expression.Assign(Expression.ArrayAccess(dest, index), converter.Convert(Expression.ArrayIndex(source, index)));
}
