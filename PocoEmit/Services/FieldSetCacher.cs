using System;
using System.Reflection;

namespace PocoEmit.Services;

/// <summary>
/// 字段写入委托缓存
/// </summary>
public class FieldSetCacher
    : CacheBase<FieldInfo, Action<object, object>>
{
    /// <summary>
    /// 构造字段写入委托
    /// </summary>
    /// <param name="field"></param>
    /// <returns></returns>
    protected override Action<object, object> CreateNew(FieldInfo field)
        => InstanceFieldHelper.EmitSetter<object, object>(field);
}
