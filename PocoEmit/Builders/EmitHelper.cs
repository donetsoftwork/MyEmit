using PocoEmit.Configuration;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace PocoEmit.Builders;

/// <summary>
/// Emit工具
/// </summary>
public static class EmitHelper
{
    #region CheckMethodCallInstance
    /// <summary>
    /// 检查调用委托目标
    /// </summary>
    /// <param name="delegate"></param>
    /// <returns></returns>
    public static Expression CheckMethodCallInstance(Delegate @delegate)
        => CheckMethodCallInstance(@delegate.Target);
    /// <summary>
    /// 检查调用对象
    /// </summary>
    /// <param name="instance"></param>
    /// <returns></returns>
    public static Expression CheckMethodCallInstance(object instance)
    {
        if (instance is null)
            return null;
        return Expression.Constant(instance);
    }
    #endregion
    #region For
    /// <summary>
    /// for循环
    /// </summary>
    /// <param name="count"></param>
    /// <param name="callBack"></param>
    /// <returns></returns>
    public static BlockExpression For(Expression count, Func<Expression, Expression> callBack)
    {
        var index = Expression.Variable(typeof(int), "index");
        return Expression.Block(
            [index],
            Expression.Assign(index, Expression.Constant(0)),
            For(index, count, callBack)
            );
    }
    /// <summary>
    /// for循环
    /// </summary>
    /// <param name="index"></param>
    /// <param name="count"></param>
    /// <param name="callBack"></param>
    /// <returns></returns>
    public static LoopExpression For(Expression index, Expression count, Func<Expression, Expression> callBack)
    {
        var forLabel = Expression.Label("forLabel");
        return Expression.Loop(
            Expression.IfThenElse(
                Expression.LessThan(index, count),
                Expression.Block(callBack(index), Expression.PostIncrementAssign(index)),
                Expression.Break(forLabel)
            ),
            forLabel);
    }
    #endregion
    #region GetMethodInfo
    /// <summary>
    /// 从表达式提取方法
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="expression"></param>
    /// <returns></returns>
    public static MethodInfo GetMethodInfo<T>(Expression<Func<T>> expression)
        => GetBodyMethod(expression);
    /// <summary>
    /// 从表达式提取方法
    /// </summary>
    /// <typeparam name="TArgument"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="expression"></param>
    /// <returns></returns>
    public static MethodInfo GetMethodInfo<TArgument, TResult>(Expression<Func<TArgument, TResult>> expression)
        => GetBodyMethod(expression);
    /// <summary>
    /// 从表达式提取方法
    /// </summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="expression"></param>
    /// <returns></returns>
    public static MethodInfo GetMethodInfo<T1, T2, TResult>(Expression<Func<T1, T2, TResult>> expression)
        => GetBodyMethod(expression);
    /// <summary>
    /// 从表达式提取方法
    /// </summary>
    /// <param name="expression"></param>
    /// <returns></returns>
    public static MethodInfo GetActionMethodInfo(Expression<Action> expression)
        => GetBodyMethod(expression);
    /// <summary>
    /// 从表达式提取方法
    /// </summary>
    /// <typeparam name="TArgument"></typeparam>
    /// <param name="expression"></param>
    /// <returns></returns>
    public static MethodInfo GetActionMethodInfo<TArgument>(Expression<Action<TArgument>> expression)
        => GetBodyMethod(expression);
    /// <summary>
    /// 从表达式提取方法
    /// </summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <param name="expression"></param>
    /// <returns></returns>
    public static MethodInfo GetActionMethodInfo<T1, T2>(Expression<Action<T1, T2>> expression)
        => GetBodyMethod(expression);
    /// <summary>
    /// 从表达式提取方法
    /// </summary>
    /// <param name="expression"></param>
    /// <returns></returns>
    private static MethodInfo GetBodyMethod(LambdaExpression expression)
    {
        if(expression.Body is MethodCallExpression callExpression)
            return callExpression.Method;
        return null;
    }
    ///// <summary>
    ///// 从表达式提取成员
    ///// </summary>
    ///// <param name="expression"></param>
    ///// <returns></returns>
    //private static MemberInfo GetBodyMember(LambdaExpression expression)
    //{
    //    if (expression.Body is MemberExpression memberExpression)
    //        return memberExpression.Member;
    //    return null;
    //}
    #endregion
    #region CheckComplexSource
    /// <summary>
    /// 是否为复杂类型
    /// </summary>
    /// <param name="value"></param>
    /// <param name="isPrimitive"></param>
    /// <returns></returns>
    public static bool CheckComplexSource(Expression value, bool isPrimitive = false)
    {
        ExpressionType type = value.NodeType;
        return type switch
        {
            ExpressionType.Constant => false,
            ExpressionType.Parameter => false,
            ExpressionType.MemberAccess => !isPrimitive,
            _ => true,
        };
    }
    #endregion
    #region BuildConditions
    /// <summary>
    /// 构造条件分支
    /// </summary>
    /// <param name="valueType"></param>
    /// <param name="conditions"></param>
    /// <returns></returns>
    public static Expression BuildConditions(Type valueType, List<KeyValuePair<Expression, Expression>> conditions)
        => BuildConditions(valueType, conditions, conditions.Count - 1, Expression.Default(valueType));
    /// <summary>
    /// 构造条件分支
    /// </summary>
    /// <param name="conditions"></param>
    /// <returns></returns>
    public static Expression BuildConditions(List<KeyValuePair<Expression, Expression>> conditions)
        => BuildConditions(typeof(void), conditions, conditions.Count - 1, Expression.Empty());
    /// <summary>
    /// 构造条件分支
    /// </summary>
    /// <param name="conditionType"></param>
    /// <param name="conditions"></param>
    /// <param name="index"></param>
    /// <param name="ifFalse"></param>
    /// <returns></returns>
    public static Expression BuildConditions(Type conditionType, List<KeyValuePair<Expression, Expression>> conditions, int index, Expression ifFalse)
    {
        var expression = Expression.Condition(
            conditions[index].Key,
            conditions[index].Value,
            ifFalse,
            conditionType
        );
        if (index == 0)
            return expression;
        return BuildConditions(conditionType, conditions, index - 1, expression);
    }
    #endregion
    #region CheckType
    /// <summary>
    /// 检查类型
    /// </summary>
    /// <param name="value"></param>
    /// <param name="valueType"></param>
    /// <returns></returns>
    public static Expression CheckType(Expression value, Type valueType)
    {
        if (PairTypeKey.CheckValueType(value.Type, valueType))
            return value;
        return Expression.Convert(value, valueType);
    }
    #endregion
    //public static ParameterExpression Copy(ParameterExpression parameter)
    //    => Expression.Parameter(parameter.Type, parameter.Name);
}
