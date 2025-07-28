using PocoEmit.Mapper.Services;
using System.Linq.Expressions;
using System.Reflection;

namespace PocoEmit.Mapper.Members;

/// <summary>
/// 字段描述
/// </summary>
/// <param name="field"></param>
internal sealed class FieldDescriptor(FieldInfo field)
    : MemberDescriptor(field.Name, field.FieldType), IReadMember, IWriteMember

{
    #region 配置
    private readonly FieldInfo _field = field;
    /// <summary>
    /// 字段
    /// </summary>
    public FieldInfo Field
        => _field;
    #endregion
    #region 方法
    /// <inheritdoc />
    public Expression EmitRead(Expression instance)
        => Expression.Field(instance, _field);
    /// <inheritdoc />
    public Expression EmitWrite(Expression instance, Expression value)
        => InstanceFieldHelper.EmitSetter(instance, _field, value);
    /// <inheritdoc />
    public override bool Accept(SourceTypeDescriptor source)
    {
        source.Add(this);
        return true;
    }
    /// <inheritdoc />
    public override bool Accept(DestTypeDescriptor dest)
    {
        dest.Add(this);
        return true;
    }
    #endregion
}
