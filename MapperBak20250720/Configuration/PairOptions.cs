using System;
using System.Collections.Generic;

namespace PocoEmit.Mapper.Configuration;

/// <summary>
/// 
/// </summary>
public class PairOptions(IEqualityComparer<string> memberComparer)
{
    /// <summary>
    /// 
    /// </summary>
    public PairOptions()
        : this(StringComparer.Ordinal)
    {
    }
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
