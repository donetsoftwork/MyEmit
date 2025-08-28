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
    /// <summary>
    /// 迭代器类型
    /// </summary>
    public Type EnumeratorType
        => _enumeratorType;
    /// <summary>
    /// MoveNext方法
    /// </summary>
    public MethodInfo MoveNextMethod
        => _moveNextMethod;
    /// <summary>
    /// Current属性
    /// </summary>
    public PropertyInfo CurrentProperty
        => _currentProperty;
    #endregion
    /// <inheritdoc />
    public Expression Travel(Expression collection, Func<Expression, Expression> callback)
    {
        var enumerator = Expression.Variable(_enumeratorType, "enumerator");

        var breakLabel = Expression.Label("breakLabel");
        
        return Expression.Block(
            [enumerator],
            Expression.Assign(enumerator, Expression.Call(CheckInstance(collection), _getEnumeratorMethod)),
            Expression.TryFinally(
                Expression.Loop(
                     Expression.IfThenElse(
                         Expression.Equal(Expression.Call(enumerator, _moveNextMethod), Expression.Constant(true)),
                         callback(Expression.Property(enumerator, _currentProperty)),
                         Expression.Break(breakLabel)
                     ),
                    breakLabel),
                Dispose(EnumeratorType, enumerator)
            )
        );
    }
    /// <summary>
    /// 释放迭代器
    /// </summary>
    /// <param name="enumeratorType"></param>
    /// <param name="enumerator"></param>
    /// <returns></returns>
    private static Expression Dispose(Type enumeratorType, Expression enumerator)
    {
#if (NETSTANDARD1_1 || NETSTANDARD1_3 || NETSTANDARD1_6)
        var isDispose = enumeratorType.GetTypeInfo().IsAssignableFrom(typeof(IDisposable).GetTypeInfo());
#else
        var isDispose = enumeratorType.IsAssignableFrom(typeof(IDisposable));
#endif
        if ( isDispose )
        {
            return Expression.Call(enumerator, _disposeMethod);
        }
        else
        {
            var disposable = Expression.Variable(typeof(IDisposable), "disposable");
            return Expression.Block(
                [disposable],
                Expression.Assign(disposable, Expression.TypeAs(enumerator, typeof(IDisposable))),
                Expression.IfThen(Expression.NotEqual(disposable, Expression.Constant(null)), Expression.Call(disposable, _disposeMethod))
            );
        }
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
    private static MethodInfo GetMoveNextCore(Type collectionType)
        => ReflectionHelper.GetMethod(collectionType, "MoveNext");
    /// <summary>
    /// 获取Dispose方法
    /// </summary>
    private static readonly MethodInfo _disposeMethod = ReflectionHelper.GetMethod(typeof(IDisposable), "Dispose");
    /// <summary>
    /// 获取Current
    /// </summary>
    /// <param name="enumeratorType"></param>
    /// <returns></returns>
    public static PropertyInfo GetCurrent(Type enumeratorType)
        => ReflectionHelper.GetPropery(enumeratorType, property => property.Name == "Current");
    #endregion
}
