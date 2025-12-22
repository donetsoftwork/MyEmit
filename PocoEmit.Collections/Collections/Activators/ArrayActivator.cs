using PocoEmit.Activators;
using PocoEmit.Builders;
using PocoEmit.Collections.Counters;
using PocoEmit.Complexes;
using System;
using System.Linq.Expressions;

namespace PocoEmit.Collections.Activators;

/// <summary>
/// 数组激活器
/// </summary>
/// <param name="arrayType"></param>
/// <param name="elementType"></param>
/// <param name="length"></param>
public class ArrayActivator(Type arrayType, Type elementType, IEmitCounter length)
    : EmitCollectionBase(arrayType, elementType)
    , IEmitActivator
{
    #region 配置
    /// <summary>
    /// 数组长度
    /// </summary>
    protected readonly IEmitCounter _length = length;
    /// <summary>
    /// 数组长度
    /// </summary>
    public IEmitCounter Length
        => _length;
    /// <inheritdoc />
    Type IEmitActivator.ReturnType
        => _collectionType;
    #endregion
    /// <inheritdoc />
    Expression IEmitActivator.New(IBuildContext context, ComplexBuilder builder, Expression argument)
        => New(_length.Count(argument));
    /// <summary>
    /// 构造数组
    /// </summary>
    /// <param name="len"></param>
    /// <returns></returns>
    public Expression New(Expression len)
        => Expression.NewArrayBounds(_elementType, len);
}
