using System.Collections.Generic;

namespace PocoEmit.Mapper.Configuration;

/// <summary>
/// 配置基类
/// </summary>
/// <param name="memberComparer"></param>
public abstract class OptionBase(IEqualityComparer<string> memberComparer)
{
    #region 配置
    private IEqualityComparer<string> _memberComparer = memberComparer;
    /// <summary>
    /// 成员匹配器
    /// </summary>
    public IEqualityComparer<string> MemberComparer
    {
        get => _memberComparer;
        set => _memberComparer = value;
    }
    private HashSet<string> _ignoreMembers = [];
    /// <summary>
    /// 忽略
    /// </summary>
    public IEnumerable<string> IgnoreMembers
        => _ignoreMembers;
    #endregion
    /// <summary>
    /// 忽略
    /// </summary>
    /// <param name="memberName"></param>
    public void Ignore(string memberName)
        => _ignoreMembers.Add(memberName);
}
