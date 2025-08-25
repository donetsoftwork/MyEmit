using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace PocoEmit.Collections.Visitors;

/// <summary>
/// 迭代器访问者
/// </summary>
public class EnumerableVisitor
    : EmitCollectionBase
    , IEmitElementVisitor
{
    /// <summary>
    /// 迭代器访问者
    /// </summary>
    /// <param name="elementType"></param>
    public EnumerableVisitor(Type elementType)
        : base(typeof(IEnumerable<>).MakeGenericType(elementType), elementType)
    {
        _getEnumeratorMethod = GetGetEnumerator(_collectionType);
        _enumeratorType = _getEnumeratorMethod.ReturnType;
        _moveNextMethod = GetMoveNext(_enumeratorType);
        _currentProperty = GetCurrent(_enumeratorType);
    }
    #region 配置
    private readonly MethodInfo _getEnumeratorMethod;
    private readonly Type _enumeratorType;
    private readonly MethodInfo _moveNextMethod;
    private readonly PropertyInfo _currentProperty;
    /// <summary>
    /// GetEnumerator方法
    /// </summary>
    public MethodInfo GetEnumeratorMethod
        => _getEnumeratorMethod;
    #endregion
    /// <inheritdoc />
    public Expression Travel(Expression collection, Func<Expression, Expression> callback)
    {
        var enumerator = Expression.Variable(_enumeratorType, "enumerator");
        var breakLabel = Expression.Label("breakLabel");
        return Expression.Block(
            [enumerator],
            Expression.Assign(enumerator, Expression.Call(CheckInstance(collection), _getEnumeratorMethod)),
            Expression.Loop(
                 Expression.IfThenElse(
                     Expression.Equal(Expression.Call(enumerator, _moveNextMethod), Expression.Constant(true)),
                     callback(Expression.Property(enumerator, _currentProperty)),
                     Expression.Break(breakLabel)
                 ),
             breakLabel)
        );
    }

    #region MethodInfo
    /// <summary>
    /// 获取MoveNext方法
    /// </summary>
    public static MethodInfo GetMoveNext(Type enumeratorType)
        => GetMoveNextCore(enumeratorType) ?? GetMoveNextCore(typeof(IEnumerator));

    /// <summary>
    /// 获取GetEnumerator方法
    /// </summary>
    /// <param name="collectionType"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public static MethodInfo GetGetEnumerator(Type collectionType)
        => ReflectionHelper.GetMethod(collectionType, "GetEnumerator")
        ?? throw new ArgumentException($"type '{collectionType.Name}' does not have GetEnumerator.");
    /// <summary>
    /// 获取MoveNext方法
    /// </summary>
    /// <param name="collectionType"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    private static MethodInfo GetMoveNextCore(Type collectionType)
        => ReflectionHelper.GetMethod(collectionType, "MoveNext");
    /// <summary>
    /// 获取Current
    /// </summary>
    /// <param name="enumeratorType"></param>
    /// <returns></returns>
    public static PropertyInfo GetCurrent(Type enumeratorType)
        => ReflectionHelper.GetPropery(enumeratorType, property => property.Name == "Current");
    #endregion
}
