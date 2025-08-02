using System;
using System.Collections.Generic;

namespace PocoEmit.Maping;

/// <summary>
/// 成员名匹配
/// </summary>
/// <param name="comparer"></param>
public class NameMatcher(IEqualityComparer<string> comparer)
    : INameMatch
{
    /// <summary>
    /// 成员名匹配
    /// </summary>
    public NameMatcher()
        : this(StringComparer.OrdinalIgnoreCase)
    {
    }
    #region 配置
    /// <summary>
    /// 比较规则
    /// </summary>
    private readonly IEqualityComparer<string> _comparer = comparer;
    /// <summary>
    /// 比较规则
    /// </summary>
    public IEqualityComparer<string> Comparer
        => _comparer;
    #endregion
    /// <inheritdoc />
    public virtual bool Match(string sourceName, string destName)
        => _comparer.Equals(sourceName, destName);
}
