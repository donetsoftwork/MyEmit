using System;
using System.Reflection;

namespace PocoEmit.Services;

/// <summary>
/// 属性写入委托缓存
/// </summary>
public class PropertySetCacher 
    : CacheBase<PropertyInfo, Action<object, object>>
{
    /// <summary>
    /// 构造属性写入委托
    /// </summary>
    /// <param name="property"></param>
    /// <returns></returns>
    protected override Action<object, object> CreateNew(PropertyInfo property)
        => InstancePropertyHelper.EmitSetter<object, object>(property);
}
