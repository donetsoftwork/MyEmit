using PocoEmit.Configuration;
using System;
using System.Linq.Expressions;
using System.Reflection;

namespace PocoEmit.Members;

/// <summary>
/// 读取字段
/// </summary>
/// <param name="field"></param>
public class FieldReader(FieldInfo field)
#if (NETSTANDARD1_1 || NETSTANDARD1_3 || NETSTANDARD1_6)
    : MemberAccessor<FieldInfo>(field.DeclaringType, field, field.Name, field.FieldType)
#else
    : MemberAccessor<FieldInfo>(field.ReflectedType, field, field.Name, field.FieldType)
#endif
    , IEmitMemberReader
{
    /// <inheritdoc />
    bool IEmitInfo.Compiled
        => false;
    /// <inheritdoc />
    public Expression Read(Expression instance)
        => Expression.Field(instance, _member);
}


