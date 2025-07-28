using PocoEmit.Mapper.Members;
using PocoEmit.Services;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace PocoEmit.Mapper.Services;

/// <summary>
/// 字段成员缓存
/// </summary>
internal sealed class FieldMemberCacher
    : CacheBase<FieldInfo, FieldDescriptor>
{
    /// <inheritdoc />
    protected override FieldDescriptor CreateNew(FieldInfo field)
        => new(field);
    /// <summary>
    /// 属性成员缓存对象
    /// </summary>
    private static readonly FieldMemberCacher Instance = new();
    /// <summary>
    /// 获取成员列表
    /// </summary>
    /// <param name="mapType"></param>
    /// <returns></returns>
    internal static IEnumerable<FieldDescriptor> GetMembers(Type mapType)
    {
        foreach (var item in ReflectionHelper.GetFields(mapType))
            yield return Instance.Get(item);
    }
}
