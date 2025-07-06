using System;
using System.Reflection;

namespace PocoEmit.Services;

/// <summary>
/// 属性读取委托缓存
/// </summary>
public class PropertyGetCacher
    : CacheBase<PropertyInfo, Func<object, object>>
{
    /// <summary>
    /// 构造属性读取委托
    /// </summary>
    /// <param name="property"></param>
    /// <returns></returns>
    protected override Func<object, object> CreateNew(PropertyInfo property)
        => InstancePropertyHelper.EmitGetter<object, object>(property);
}
