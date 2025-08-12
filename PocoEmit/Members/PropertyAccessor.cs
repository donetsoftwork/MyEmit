using PocoEmit.Configuration;
using System;
using System.Linq.Expressions;
using System.Reflection;

namespace PocoEmit.Members;

/// <summary>
/// 读取属性
/// </summary>
/// <param name="property"></param>
public class PropertyAccessor(PropertyInfo property)
#if (NETSTANDARD1_1 || NETSTANDARD1_3 || NETSTANDARD1_6)
    : MemberAccessor<PropertyInfo>(property.DeclaringType, property, property.Name, property.PropertyType)
#else
    : MemberAccessor<PropertyInfo>(property.ReflectedType, property, property.Name, property.PropertyType)
#endif
    , IEmitMemberReader, IEmitMemberWriter
{
    #region 配置
    /// <inheritdoc />
    MemberInfo IEmitMemberReader.Info
        => _member;
    MemberInfo IEmitMemberWriter.Info
        => _member;
    /// <inheritdoc />
    bool ICompileInfo.Compiled
        => false;
    #endregion
    /// <inheritdoc />
    public Expression Read(Expression instance)
    {
#if (NETSTANDARD1_1 || NETSTANDARD1_3 || NETSTANDARD1_6)
        var method = _member.GetMethod
#else
        var method = _member.GetGetMethod()
#endif
            ?? throw new ArgumentException($"Property '{_member.Name}' does not have a reader method.");
        return Expression.Property(instance, method);
    }
    /// <inheritdoc />
    public Expression Write(Expression instance, Expression value)
    {
#if (NETSTANDARD1_1 || NETSTANDARD1_3)
        var method = _member.SetMethod
#else
        var method = _member.GetSetMethod()
#endif
            ?? throw new ArgumentException($"Property '{_member.Name}' does not have a writer method.");
        return Expression.Assign(Expression.Property(instance, method), value);
    }
}
