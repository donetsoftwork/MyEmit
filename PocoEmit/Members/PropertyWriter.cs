using PocoEmit.Configuration;
using System;
using System.Linq.Expressions;
using System.Reflection;

namespace PocoEmit.Members;

/// <summary>
/// 写入属性
/// </summary>
/// <param name="property"></param>
public class PropertyWriter(PropertyInfo property)
#if (NETSTANDARD1_1 || NETSTANDARD1_3 || NETSTANDARD1_6)
    : MemberAccessor<PropertyInfo>(property.DeclaringType, property, property.Name, property.PropertyType)
#else
    : MemberAccessor<PropertyInfo>(property.ReflectedType, property, property.Name, property.PropertyType)
#endif
    , IEmitMemberWriter
{
    /// <inheritdoc />
    MemberInfo IEmitMemberWriter.Info
        => Member;
    /// <inheritdoc />
    bool ICompileInfo.Compiled
        => false;
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