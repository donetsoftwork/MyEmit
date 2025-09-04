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
        if (_member.CanRead)
            return Expression.Property(instance, _member);
        throw new ArgumentException($"Property '{_member.Name}' can not Read.");
    }
    /// <inheritdoc />
    public Expression Write(Expression instance, Expression value)
    {
        if (_member.CanWrite)
            return Expression.Assign(Expression.Property(instance, _member), value);
        throw new ArgumentException($"Property '{_member.Name}' can not Write.");
    }
}
