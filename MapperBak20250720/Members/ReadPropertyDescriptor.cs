using PocoEmit.Mapper.Services;
using System.Linq.Expressions;
using System.Reflection;

namespace PocoEmit.Mapper.Members;

/// <summary>
/// 可读属性
/// </summary>
/// <param name="property"></param>
internal sealed class ReadPropertyDescriptor(PropertyInfo property) 
    : PropertyDescriptor(property), IReadMember
{
    #region 方法
    /// <inheritdoc />
    public Expression EmitRead(Expression instance)
        => InstancePropertyHelper.EmitGetter(instance, _property);
    /// <inheritdoc />
    public override bool Accept(SourceTypeDescriptor source)
    {
        source.Add(this);
        return true;
    }
    /// <inheritdoc />
    public override bool Accept(DestTypeDescriptor dest)
        => false;
    #endregion
}
