using PocoEmit.Services;
using System;

namespace PocoEmit.Mapper.Services;

/// <summary>
/// 源映射类型缓存
/// </summary>
internal class SourceTypeCacher
    : CacheBase<Type, SourceTypeDescriptor>
{
    /// <inheritdoc />
    protected override SourceTypeDescriptor CreateNew(Type key)
        => Create(key);
    /// <summary>
    /// 源映射类型缓存对象
    /// </summary>
    internal static readonly SourceTypeCacher Instance = new();
    /// <summary>
    /// 构造源映射类型
    /// </summary>
    /// <param name="mapType"></param>
    /// <returns></returns>
    private static SourceTypeDescriptor Create(Type mapType)
    {
        SourceTypeDescriptor source = new(mapType);
        foreach (var item in PropertyMemberCacher.GetReadMembers(mapType))
            item.Accept(source);
        foreach (var item in FieldMemberCacher.GetMembers(mapType))
            item.Accept(source);
        return source;
    }
}
