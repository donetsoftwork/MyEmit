using Hand.Creational;
using Hand.Reflection;
using PocoEmit.Builders;
using PocoEmit.Cachers;
using PocoEmit.Collections;
using PocoEmit.Members;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace PocoEmit.Configuration;

/// <summary>
/// 默认值构造器
/// </summary>
public class DefaultValueProvider
{
    #region 配置
    /// <summary>
    /// 映射配置
    /// </summary>
    protected readonly IMapperOptions _options;
    /// <summary>
    /// 映射配置
    /// </summary>
    public IMapperOptions Options
        => _options;
    /// <summary>
    /// 类型默认值缓存
    /// </summary>
    private readonly TypeDefaultValueCacher _typeCacher;
    /// <summary>
    /// 属性默认值缓存
    /// </summary>
    private readonly MemberDefaultValueCacher _memberCacher;
    /// <summary>
    /// 参数默认值缓存
    /// </summary>
    private readonly ParameterDefaultValueCacher _parameterCacher;
    #endregion
    /// <summary>
    /// 默认值构造器
    /// </summary>
    /// <param name="options"></param>
    public DefaultValueProvider(IMapperOptions options)
    {
        _options = options;
        _typeCacher = new TypeDefaultValueCacher(this);
        _memberCacher = new MemberDefaultValueCacher(this);
        _parameterCacher = new ParameterDefaultValueCacher(this);
    }
    #region Build
    /// <summary>
    /// 构造类型默认值
    /// </summary>
    /// <param name="entityType"></param>
    /// <returns></returns>
    public ICreator<Expression> Build(Type entityType)
        => _typeCacher.Get(entityType);
    /// <summary>
    /// 构造属性默认值
    /// </summary>
    /// <param name="member"></param>
    /// <returns></returns>
    public ICreator<Expression> Build(IEmitMemberWriter member)
        => _memberCacher.Get(member);
    /// <summary>
    /// 构造属性默认值
    /// </summary>
    /// <param name="parameter"></param>
    /// <returns></returns>
    public ICreator<Expression> Build(ConstructorParameterMember parameter)
        => _parameterCacher.Get(parameter);
    #endregion
    #region BuildCore
    /// <summary>
    /// 属性默认值
    /// </summary>
    /// <param name="member"></param>
    /// <returns></returns>
    internal protected virtual ICreator<Expression> BuildCore(IEmitMemberWriter member)
    {
        // 属性(字段)注入配置
        if (_options.TryGetConfig(member.Info, out var memberBuilder))
            return memberBuilder;
        return BuildCore(member.ValueType);
    }
    /// <summary>
    /// 构造函数参数默认值
    /// </summary>
    /// <param name="parameter"></param>
    /// <returns></returns>
    internal protected virtual ICreator<Expression> BuildCore(ConstructorParameterMember parameter)
        => BuildCore(parameter.ValueType);
    /// <summary>
    /// 类型默认值
    /// </summary>
    /// <param name="entityType"></param>
    /// <returns></returns>
    internal protected virtual ICreator<Expression> BuildCore(Type entityType)
    {
        var builder = new DefaultValueBuilder(this);
        return builder.Check(entityType);
    }
    #endregion
    /// <summary>
    /// 检查属性(字段)默认值
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="entity"></param>
    public void CheckMembers(IEmitBuilder builder, Expression entity)
    {
        var entityType = entity.Type;
        // 基元、数组、集合类型忽略属性检查
        if (_options.CheckPrimitive(entityType) || entityType.IsArray || ReflectionType.HasGenericType(entityType, typeof(IEnumerable<>)))
            return;
        var members = _options.MemberCacher.Get(entityType).EmitWriters.Values;
        CheckMembers(builder, entity, members);
    }
    /// <summary>
    /// 检查属性(字段)默认值
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="entity"></param>
    /// <param name="members"></param>
    internal void CheckMembers(IEmitBuilder builder, Expression entity, IEnumerable<IEmitMemberWriter> members)
    {
        foreach (var memberWriter in members)
            CheckMember(builder, entity, memberWriter);
    }
    /// <summary>
    /// 检查属性(字段)默认值
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="entity"></param>
    /// <param name="memberWriter"></param>
    private void CheckMember(IEmitBuilder builder, Expression entity, IEmitMemberWriter memberWriter)
    {
        var memberType = memberWriter.ValueType;
        Expression expression;          
        if (builder is DefaultValueBuilder defaultValueBuilder)
        {
            // 避免死循环
            // DefaultValueBuilder能检测循环引用
            expression = defaultValueBuilder.Build(memberType);
        }
        else
        {
            expression = Build(memberWriter)?.Create();
        }
        if (expression is null)
            return;
        if (PairTypeKey.CheckNullCondition(memberType))
        {
            // 读取器缓存
            var readerCacher = MemberContainer.Instance.MemberReaderCacher;
            // 如果存在读取器
            if (readerCacher.Get(memberWriter.Info) is var memberReader)
            {
                // 为Default才注入属性(字段)
                builder.IfDefault(
                    memberReader.Read(entity),
                    memberWriter.Write(entity, expression));
                return;
            }
        }
        // 直接注入属性(字段)
        builder.Add(memberWriter.Write(entity, expression));
    }
}
