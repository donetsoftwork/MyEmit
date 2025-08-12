using PocoEmit.Configuration;
using System.Linq.Expressions;
using System.Reflection;

namespace PocoEmit.Members;

/// <summary>
/// 读写字段
/// </summary>
/// <param name="field"></param>
public class FieldAccessor(FieldInfo field)
#if (NETSTANDARD1_1 || NETSTANDARD1_3 || NETSTANDARD1_6)
    : MemberAccessor<FieldInfo>(field.DeclaringType, field, field.Name, field.FieldType)
#else
    : MemberAccessor<FieldInfo>(field.ReflectedType, field, field.Name, field.FieldType)
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
    #region IEmitMemberReader
    /// <inheritdoc />
    public Expression Read(Expression instance)
        => Expression.Field(instance, _member);
    #endregion
    #region IEmitMemberWriter
    /// <inheritdoc />
    public Expression Write(Expression instance, Expression value)
        => Expression.Assign(Expression.Field(instance, _member), value);
    #endregion
}
