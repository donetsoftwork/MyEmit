using PocoEmit.Indexs;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace PocoEmit.Collections.Indexs;

/// <summary>
/// 索引方法读取器
/// </summary>
public class PropertyIndexMemberReader(PropertyInfo item)
    : IEmitIndexMemberReader
{
    #region 配置
    private readonly PropertyInfo _item = item;
    /// <summary>
    /// 索引方法
    /// </summary>
    public PropertyInfo Item
        => _item;
    #endregion
    /// <inheritdoc />
    public Expression Read(Expression instance, Expression index)
        => Expression.Property(instance, _item, index);

    /// <summary>
    /// 获取Item索引器属性
    /// </summary>
    public static PropertyInfo GetItemProperty(Type listType)
        => ReflectionHelper.GetPropery(listType, property => property.Name == "Item" && property.CanRead);

    /// <summary>
    /// 构造索引方法读取器
    /// </summary>
    /// <param name="listType"></param>
    /// <returns></returns>
    public static PropertyIndexMemberReader Create(Type listType)
    {
        var itemProperty = GetItemProperty(listType);
        if (itemProperty is null)
            return null;
        return new PropertyIndexMemberReader(itemProperty);
    }
}
