using PocoEmit.Builders;
using System;
using System.Linq.Expressions;
using System.Reflection;

namespace PocoEmit;

/// <summary>
/// 构建扩展简便方法
/// </summary>
public static class BuildServices
{
    #region Declare
    /// <summary>
    /// 定义变量
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="type">类型</param>
    /// <param name="name">变量名</param>
    /// <returns></returns>
    public static ParameterExpression Declare(this IEmitBuilder builder, Type type, string name = null)
    {
        var variable = Expression.Variable(type, name);
        builder.AddVariable(variable);
        return variable;
    }
    /// <summary>
    /// 定义变量
    /// </summary>
    /// <typeparam name="T">类型</typeparam>
    /// <param name="builder"></param>
    /// <param name="name">变量名</param>
    /// <returns></returns>
    public static ParameterExpression Declare<T>(this IEmitBuilder builder, string name = null)
        => Declare(builder, typeof(T), name);
    #endregion
    #region Temp
    /// <summary>
    /// 临时变量
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="type">类型</param>
    /// <returns></returns>
    public static ParameterExpression Temp(this IEmitBuilder builder, Type type)
        => builder.Temp(type, Expression.Default(type));
    /// <summary>
    /// 临时变量
    /// </summary>
    /// <typeparam name="T">类型</typeparam>
    /// <param name="builder"></param>
    /// <returns></returns>
    public static ParameterExpression Temp<T>(this IEmitBuilder builder)
        => Temp<T>(builder, Expression.Default(typeof(T)));
    /// <summary>
    /// 临时变量
    /// </summary>
    /// <typeparam name="T">类型</typeparam>
    /// <param name="builder"></param>
    /// <param name="initial">初始值</param>
    /// <returns></returns>
    public static ParameterExpression Temp<T>(this IEmitBuilder builder, Expression initial)
        => builder.Temp(typeof(T), initial);
    #endregion
    #region If
    /// <summary>
    /// If
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="test"></param>
    /// <param name="ifTrue"></param>
    public static void IfThen(this IEmitBuilder builder, Expression test, Expression ifTrue)
        => builder.Add(Expression.IfThen(test, ifTrue));
    /// <summary>
    /// IfElse
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="test"></param>
    /// <param name="ifTrue"></param>
    /// <param name="ifFalse"></param>
    public static void IfThenElse(this IEmitBuilder builder, Expression test, Expression ifTrue, Expression ifFalse)
        => builder.Add(Expression.IfThenElse(test, ifTrue, ifFalse));
    /// <summary>
    /// IfElse
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="test"></param>
    /// <param name="ifTrue"></param>
    /// <param name="ifFalse"></param>
    /// <param name="type"></param>
    public static void IfThenElse(this IEmitBuilder builder, Expression test, Expression ifTrue, Expression ifFalse, Type type)
        => builder.Add(Expression.Condition(test, ifTrue, ifFalse, type));
    #endregion
    #region IfDefault
    /// <summary>
    /// IfDefault
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="test"></param>
    /// <param name="ifTrue"></param>
    public static void IfDefault(this IEmitBuilder builder, Expression test, Expression ifTrue)
        => builder.Add(EmitHelper.IfDefault(test, ifTrue));
    /// <summary>
    /// IfDefault
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="test"></param>
    /// <param name="ifTrue"></param>
    /// <param name="ifFalse"></param>
    public static void IfDefault(this IEmitBuilder builder, Expression test, Expression ifTrue, Expression ifFalse)
        => builder.Add(EmitHelper.IfDefault(test, ifTrue, ifFalse));
    /// <summary>
    /// IfDefault
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="test"></param>
    /// <param name="ifTrue"></param>
    /// <param name="ifFalse"></param>
    /// <param name="type"></param>
    public static void IfDefault(this IEmitBuilder builder, Expression test, Expression ifTrue, Expression ifFalse, Type type)
        => builder.Add(EmitHelper.IfDefault(test, ifTrue, ifFalse, type));
    /// <summary>
    /// IfNotDefault
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="test"></param>
    /// <param name="ifTrue"></param>
    public static void IfNotDefault(this IEmitBuilder builder, Expression test, Expression ifTrue)
        => builder.Add(EmitHelper.IfNotDefault(test, ifTrue));
    /// <summary>
    /// IfDefault
    /// </summary>
    /// <typeparam name="TBuilder"></typeparam>
    /// <param name="builder"></param>
    /// <param name="ifTrue"></param>
    public static void IfDefault<TBuilder>(this IVariableBuilder builder, Expression ifTrue)
        => IfDefault(builder, builder.Current, ifTrue);
    /// <summary>
    /// IfNotDefault
    /// </summary>
    /// <typeparam name="TBuilder"></typeparam>
    /// <param name="builder"></param>
    /// <param name="ifTrue"></param>
    public static void IfNotDefault<TBuilder>(this IVariableBuilder builder, Expression ifTrue)
        => IfNotDefault(builder, builder.Current, ifTrue);
    #endregion
    #region Assign
    /// <summary>
    /// 赋值
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="variable"></param>
    /// <param name="value"></param>
    public static void Assign(this IEmitBuilder builder, Expression variable, Expression value)
        => builder.Add(Expression.Assign(variable, value));
    /// <summary>
    /// 赋值
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="value"></param>
    public static void Assign(this IVariableBuilder builder, Expression value)
        => builder.Assign(builder.Current, value);
    #endregion
    #region AssignIfNull
    /// <summary>
    /// AssignIfNull
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="test"></param>
    /// <param name="ifTrue"></param>
    /// <param name="ifFalse"></param>
    public static void AssignIfDefault(this IVariableBuilder builder, Expression test, Expression ifTrue, Expression ifFalse)
      => IfDefault(builder, test, Expression.Assign(builder.Current, ifTrue), Expression.Assign(builder.Current, ifFalse));
    /// <summary>
    /// 为null才赋值
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="expression"></param>
    public static void AssignIfDefault(this IVariableBuilder builder, Expression expression)
        => IfDefault(builder, builder.Current, Expression.Assign(builder.Current, expression));
    #endregion
    #region Property
    /// <summary>
    /// 获取属性
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="property"></param>
    /// <returns></returns>
    public static MemberExpression Property(this IVariableBuilder builder, PropertyInfo property)
        => Expression.Property(builder.Current, property);
    /// <summary>
    /// 获取属性
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="propertyName"></param>
    /// <returns></returns>
    public static MemberExpression Property(this IVariableBuilder builder, string propertyName)
        => Expression.Property(builder.Current, propertyName);
    #endregion
    #region Field
    /// <summary>
    /// 获取字段
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="field"></param>
    /// <returns></returns>
    public static MemberExpression Field(this IVariableBuilder builder, FieldInfo field)
        => Expression.Field(builder.Current, field);
    /// <summary>
    /// 获取字段
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="fieldName"></param>
    /// <returns></returns>
    public static MemberExpression Field(this IVariableBuilder builder, string fieldName)
        => Expression.Field(builder.Current, fieldName);
    #endregion
    #region CreateScope
    /// <summary>
    /// 构造作用域
    /// </summary>
    /// <param name="builder"></param>
    /// <returns></returns>
    public static EmitScopeBuilder CreateScope(this IEmitBuilder builder)
        => builder.CreateScope([]);
    /// <summary>
    /// 构造变量作用域
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="current"></param>
    /// <returns></returns>
    public static ScopeVariableBuilder CreateScope(this IEmitBuilder builder, ParameterExpression current)
        => builder.CreateScope(current, []);
    #endregion
}
