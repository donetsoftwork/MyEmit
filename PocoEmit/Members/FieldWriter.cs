using PocoEmit.Configuration;
using System.Linq.Expressions;
using System.Reflection;

namespace PocoEmit.Members;

/// <summary>
/// 写入字段
/// </summary>
/// <param name="field"></param>
public class FieldWriter(FieldInfo field)
#if (NETSTANDARD1_1 || NETSTANDARD1_3 || NETSTANDARD1_6)
    : MemberAccessor<FieldInfo>(field.DeclaringType, field, field.Name, field.FieldType)
#else
    : MemberAccessor<FieldInfo>(field.ReflectedType, field, field.Name, field.FieldType)
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
        => Expression.Assign(Expression.Field(instance, _member), value);
}
