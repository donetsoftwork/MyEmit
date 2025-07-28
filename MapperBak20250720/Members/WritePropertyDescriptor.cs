using PocoEmit.Mapper.Services;
using System.Linq.Expressions;
using System.Reflection;

namespace PocoEmit.Mapper.Members;

/// <summary>
/// 可写属性
/// </summary>
/// <param name="property"></param>
internal class WritePropertyDescriptor(PropertyInfo property)
    : PropertyDescriptor(property), IWriteMember
{
    #region 方法
    /// <inheritdoc />
    public Expression EmitWrite(Expression instance, Expression value)
        => InstancePropertyHelper.EmitSetter(instance, _property, value);
    /// <inheritdoc />
    public override bool Accept(SourceTypeDescriptor source)
        => false;
    /// <inheritdoc />
    public override bool Accept(DestTypeDescriptor dest)
    {
        dest.Add(this);
        return true;
    }
    #endregion
}
