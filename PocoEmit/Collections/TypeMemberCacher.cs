using Hand.Cache;
using PocoEmit.Configuration;
using System;

namespace PocoEmit.Collections;

/// <summary>
/// 类型成员缓存
/// </summary>
/// <param name="poco"></param>
public sealed class TypeMemberCacher(IPocoOptions poco)
    : CacheFactoryBase<Type, MemberBundle>(poco)
{
    #region 配置
    private readonly IPocoOptions _poco = poco;
    /// <summary>
    /// 配置
    /// </summary>
    public IPocoOptions Poco
        => _poco;
    #endregion
    #region CacheBase
    /// <inheritdoc />
    protected override MemberBundle CreateNew(in Type key)
        => _poco.ReflectionMember.GetMembers(key);
    #endregion    
}
