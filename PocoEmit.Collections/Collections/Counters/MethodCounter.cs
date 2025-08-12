using PocoEmit.Builders;
using System;
using System.Linq.Expressions;
using System.Reflection;

namespace PocoEmit.Collections.Counters;

/// <summary>
/// 
/// </summary>
/// <param name="collectionType"></param>
/// <param name="elementType"></param>
/// <param name="target"></param>
/// <param name="countMethod"></param>
public class MethodCounter(Type collectionType, Type elementType, Expression target, MethodInfo countMethod)
    : EmitCollectionBase(collectionType, elementType)
    , IEmitCollectionCounter
{
    #region 配置
    private readonly Expression _target = target;
    private readonly MethodInfo _countMethod = countMethod;
    /// <summary>
    /// 调用实例
    /// </summary>
    public Expression Target
        => _target;
    /// <summary>
    /// 方法
    /// </summary>
    public MethodInfo CountMethod
        => _countMethod;
    /// <inheritdoc />
    bool IEmitCounter.CountByProperty
        => false;
    #endregion
    /// <inheritdoc />
    Expression IEmitCounter.Count(Expression collection)
        => Count(collection);
    /// <summary>
    /// 获取数量
    /// </summary>
    /// <param name="instance"></param>
    /// <returns></returns>
    public Expression Count(Expression instance)
        => Expression.Call(_target, _countMethod, CheckInstance(instance));
}
