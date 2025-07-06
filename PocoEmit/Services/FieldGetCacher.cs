using System;
using System.Reflection;

namespace PocoEmit.Services;

/// <summary>
/// 字段读取委托缓存
/// </summary>
public class FieldGetCacher
    : CacheBase<FieldInfo, Func<object, object>>
{
    /// <summary>
    /// 构造字段读取委托
    /// </summary>
    /// <param name="field"></param>
    /// <returns></returns>
    protected override Func<object, object> CreateNew(FieldInfo field)
        => InstanceFieldHelper.EmitGetter<object, object>(field);
}
