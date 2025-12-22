using Hand.Creational;
using Hand.Reflection;
using PocoEmit.Configuration;
using PocoEmit.Members;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
#if (NETSTANDARD1_1 || NETSTANDARD1_3 || NETSTANDARD1_6)
using System.Reflection;
#endif

namespace PocoEmit.Builders;

/// <summary>
/// 复杂类型默认值构造器
/// </summary>
/// <param name="provider"></param>
public class DefaultValueBuilder(DefaultValueProvider provider)
    : EmitBuilder
{
    #region 配置
    /// <summary>
    /// 映射配置
    /// </summary>
    private readonly IMapperOptions _options = provider.Options;
    /// <summary>
    /// 默认值构造器
    /// </summary>
    private readonly DefaultValueProvider _provider = provider;
    #endregion
    #region Check
    /// <summary>
    /// 类型默认值
    /// </summary>
    /// <param name="entityType"></param>
    /// <returns></returns>
    public virtual ICreator<Expression> Check(Type entityType)
    {
        if (_options.TryGetConfig(entityType, out var config))
            return config;
#if (NETSTANDARD1_1 || NETSTANDARD1_3 || NETSTANDARD1_6)
        var entityTypeInfo = entityType.GetTypeInfo();
        var isAbstract = entityTypeInfo.IsAbstract || entityTypeInfo.IsInterface;
#else
        var isAbstract = entityType.IsAbstract || entityType.IsInterface;
#endif
        if (isAbstract)
            return null;
        var expr = Build(entityType);
        if (expr is null)
            return null;
        Add(expr);
        return this;
    }
    #endregion
    /// <summary>
    /// 构建对象表达式
    /// </summary>
    /// <returns></returns>
    public Expression Build(Type entityType)
    {
        if (_options.TryGetConfig(entityType, out var memberBuilder))
            return this.Execute(memberBuilder);
        // 基元、数组、集合类型不处理
        if (_options.CheckPrimitive(entityType) || entityType.IsArray || ReflectionType.HasGenericType(entityType, typeof(IEnumerable<>)))
            return null;
        var newExpr = New(entityType);
        if (newExpr is null)
            return null;
        var members = _options.MemberCacher.Get(entityType).EmitWriters.Values;
        if (members.Count == 0)
            return newExpr;
        var entity = CreateTemp(entityType);
        if (entity is null)
            return null;
        this.Assign(entity, newExpr);
        _provider.CheckMembers(this, entity, members);
        return entity;
    }
    /// <summary>
    /// Emit新对象表达式
    /// </summary>
    /// <returns></returns>
    private NewExpression New(Type entityType)
    {
        var constructor = _options.GetConstructor(entityType);
        if (constructor is not null)
        {
            var parameters = constructor.GetParameters();
            if (parameters.Length == 0)
                return Expression.New(constructor);
            var args = CreateParameters(ConstructorParameterMember.Convert(constructor, parameters));
            if (args is null)
                return null;
            return Expression.New(constructor, args);
        }
#if (NETSTANDARD1_1 || NETSTANDARD1_3 || NETSTANDARD1_6)
        var isValueType = entityType.GetTypeInfo().IsValueType;
#else
        var isValueType = entityType.IsValueType;
#endif
        if (isValueType)
            return Expression.New(entityType);
        return null;
    }
    /// <summary>
    /// 构造参数
    /// </summary>
    /// <param name="parameters"></param>
    /// <returns></returns>
    private Expression[] CreateParameters(ConstructorParameterMember[] parameters)
    {
        var arguments = new Expression[parameters.Length];
        var index = 0;
        foreach (var parameter in parameters)
        {
            Expression expression;
            var parameterType = parameter.ValueType; 
            if (_options.TryGetConfig(parameterType, out var builder))
            {                
                expression = this.Execute(builder);
            }
            else
            {
                expression = parameter.DefaultValue ?? Build(parameterType);
            }
            if (expression is null)
                return null;
            arguments[index++] = expression;
        }
        return arguments;
    }
    #region EmitBuilder
    /// <inheritdoc />
    protected internal override ParameterExpression CreateTemp(Type type)
    {
        // 循环引用检测
        if (_temps.ContainsKey(type))
            return null;
        var temp = this.Declare(type, "t");
        _temps.Add(type, temp);
        return temp;
    }
    #endregion
}
