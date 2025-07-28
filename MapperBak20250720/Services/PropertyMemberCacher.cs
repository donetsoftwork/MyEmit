using PocoEmit.Mapper.Members;
using PocoEmit.Services;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace PocoEmit.Mapper.Services;

/// <summary>
/// 属性成员缓存
/// </summary>
internal sealed class PropertyMemberCacher
    : CacheBase<PropertyInfo, IMember>
{
    /// <inheritdoc />
    protected override IMember CreateNew(PropertyInfo property)
    {
        if(property.CanRead)
        {
            if (property.CanWrite)
                return new ReadWritePropertyDescriptor(property);
            return new ReadPropertyDescriptor(property);
        }
        return new WritePropertyDescriptor(property);
    }
    /// <summary>
    /// 属性成员缓存对象
    /// </summary>
    private static readonly PropertyMemberCacher Instance = new();
    /// <summary>
    /// 获取可读成员列表
    /// </summary>
    /// <param name="mapType"></param>
    /// <returns></returns>
    internal static IEnumerable<IMember> GetReadMembers(Type mapType)
    {
        foreach (var item in ReflectionHelper.GetReadProperties(mapType))
            yield return Instance.Get(item);
    }
    /// <summary>
    /// 获取可写成员列表
    /// </summary>
    /// <param name="mapType"></param>
    /// <returns></returns>
    internal static IEnumerable<IMember> GetWriteMembers(Type mapType)
    {
        foreach (var item in ReflectionHelper.GetWriteProperties(mapType))
            yield return Instance.Get(item);
    }
}
