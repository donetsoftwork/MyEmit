using PocoEmit.Reflection;
using System;

namespace PocoEmit.Collections;

/// <summary>
/// 类型成员缓存
/// </summary>
/// <param name="cacher"></param>
/// <param name="reflection"></param>
public class TypeMemberCacher(ISettings<Type, MemberBundle> cacher, IReflectionMember reflection)
    : CacheBase<Type, MemberBundle>(cacher)
{
    #region 配置
    private readonly IReflectionMember _reflection = reflection;
    /// <summary>
    /// 成员反射
    /// </summary>
    public IReflectionMember Reflection
        => _reflection;
    #endregion
    #region CacheBase
    /// <inheritdoc />
    protected override MemberBundle CreateNew(Type key)
        => _reflection.GetMembers(key);
    #endregion
    
}
