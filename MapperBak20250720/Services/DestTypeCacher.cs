using PocoEmit.Services;
using System;

namespace PocoEmit.Mapper.Services;

/// <summary>
/// 目标映射类型缓存
/// </summary>
internal class DestTypeCacher
    : CacheBase<Type, DestTypeDescriptor>
{
    /// <inheritdoc />
    protected override DestTypeDescriptor CreateNew(Type key)
        => Create(key);
    /// <summary>
    /// 目标映射类型缓存对象
    /// </summary>
    internal static readonly DestTypeCacher Instance = new();
    /// <summary>
    /// 构造源映射类型
    /// </summary>
    /// <param name="mapType"></param>
    /// <returns></returns>
    private static DestTypeDescriptor Create(Type mapType)
    {
        DestTypeDescriptor dest = new(mapType);
        foreach (var item in PropertyMemberCacher.GetWriteMembers(mapType))
            item.Accept(dest);
        foreach (var item in FieldMemberCacher.GetMembers(mapType))
            item.Accept(dest);
        return dest;
    }
}
