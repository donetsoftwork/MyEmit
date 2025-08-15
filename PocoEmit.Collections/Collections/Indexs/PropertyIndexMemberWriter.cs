using PocoEmit.Indexs;
using System;
using System.Linq.Expressions;
using System.Reflection;

namespace PocoEmit.Collections.Indexs;

/// <summary>
/// 索引方法写入器
/// </summary>
public class PropertyIndexMemberWriter(PropertyInfo item)
    : IEmitIndexMemberWriter
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
    public Expression Write(Expression instance, Expression index, Expression value)
        => Expression.Assign(Expression.Property(instance, _item, index), value);

    /// <summary>
    /// 获取Item索引器属性
    /// </summary>
    public static PropertyInfo GetItemProperty(Type listType)
        => ReflectionHelper.GetPropery(listType, property => property.Name == "Item" && property.CanWrite);
    /// <summary>
    /// 构造索引方法写入器
    /// </summary>
    /// <param name="listType"></param>
    /// <returns></returns>
    public static PropertyIndexMemberWriter Create(Type listType)
    {
        var itemProperty = GetItemProperty(listType);
        if(itemProperty is null)
            return null;
        return new PropertyIndexMemberWriter(itemProperty);
    }
}
