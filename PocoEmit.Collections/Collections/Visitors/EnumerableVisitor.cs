using PocoEmit.Builders;
using PocoEmit.Collections.Bundles;
using System;
using System.Linq.Expressions;
using System.Reflection;

namespace PocoEmit.Collections.Visitors;

/// <summary>
/// 迭代器访问者
/// </summary>
public class EnumerableVisitor(Type collectionType, Type elementType, MethodInfo getEnumeratorMethod, Type enumeratorType, MethodInfo moveNextMethod, PropertyInfo currentProperty)
    : EmitCollectionBase(collectionType, elementType)
    , IEmitElementVisitor
{
    #region 配置
    private readonly MethodInfo _getEnumeratorMethod = getEnumeratorMethod;
    private readonly Type _enumeratorType = enumeratorType;
    private readonly MethodInfo _moveNextMethod = moveNextMethod;
    private readonly PropertyInfo _currentProperty = currentProperty;
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
    Expression IEmitElementVisitor.Travel(Expression collection, Func<Expression, Expression> callback)
        => Travel(CheckInstance(collection), _elementType, _enumeratorType,_getEnumeratorMethod, _moveNextMethod, _currentProperty, callback);
    /// <summary>
    /// 遍历
    /// </summary>
    /// <param name="collection"></param>
    /// <param name="callback"></param>
    /// <returns></returns>
    public static Expression Travel(Expression collection, Func<Expression, Expression> callback)
        => Travel(collection, CollectionContainer.Instance.EnumerableCacher.Get(collection.Type), callback);
    /// <summary>
    /// 遍历
    /// </summary>
    /// <param name="collection"></param>
    /// <param name="bundle"></param>
    /// <param name="callback"></param>
    /// <returns></returns>
    public static Expression Travel(Expression collection, EnumerableBundle bundle, Func<Expression, Expression> callback)
    {
        if (bundle is null)
            return Expression.Empty();
        return Travel(collection, bundle.ElementType, bundle.EnumeratorType, bundle.GetEnumeratorMethod, bundle.MoveNextMethod, bundle.CurrentProperty, callback);
    }
    /// <summary>
    /// 遍历
    /// </summary>
    /// <param name="collection"></param>
    /// <param name="elementType"></param>
    /// <param name="enumeratorType"></param>
    /// <param name="getEnumerator"></param>
    /// <param name="moveNext"></param>
    /// <param name="current"></param>
    /// <param name="callback"></param>
    /// <returns></returns>
    public static Expression Travel(Expression collection, Type elementType, Type enumeratorType, MethodInfo getEnumerator, MethodInfo moveNext, PropertyInfo current, Func<Expression, Expression> callback)
    {
        var enumerator = Expression.Variable(enumeratorType, "enumerator");
        var breakLabel = Expression.Label("breakLabel");

        return Expression.Block(
            [enumerator],
            Expression.Assign(enumerator, Expression.Call(collection, getEnumerator)),
            Expression.TryFinally(
                Expression.Loop(
                     Expression.IfThenElse(
                         Expression.Equal(Expression.Call(enumerator, moveNext), Expression.Constant(true)),
                         callback(EmitHelper.CheckType(Expression.Property(enumerator, current), elementType)),
                         Expression.Break(breakLabel)
                     ),
                    breakLabel),
                EmitDispose.Dispose(enumerator)
            )
        );
    }
}
