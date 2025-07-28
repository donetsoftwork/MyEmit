using System.Reflection;

namespace PocoEmit.Mapper.Members;

/// <summary>
/// 属性描述
/// </summary>
/// <param name="property"></param>
internal abstract class PropertyDescriptor(PropertyInfo property)
    : MemberDescriptor(property.Name, property.PropertyType)
{
    #region 配置
    /// <summary>
    /// 属性
    /// </summary>
    protected readonly PropertyInfo _property = property;
    /// <summary>
    /// 属性
    /// </summary>
    public PropertyInfo Property 
        => _property;
    #endregion
}
