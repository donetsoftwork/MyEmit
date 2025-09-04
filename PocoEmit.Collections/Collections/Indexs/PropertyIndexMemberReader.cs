using PocoEmit.Indexs;
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
}
