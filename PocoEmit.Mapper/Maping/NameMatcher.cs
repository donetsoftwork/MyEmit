using System;
using System.Collections.Generic;

namespace PocoEmit.Maping;

/// <summary>
/// 成员名匹配
/// </summary>
/// <param name="comparer"></param>
public sealed class NameMatcher(IEqualityComparer<string> comparer)
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
    private readonly IEqualityComparer<string> _comparer = comparer;
    /// <summary>
    /// 比较规则
    /// </summary>
    public IEqualityComparer<string> Comparer
        => _comparer;
    #endregion
    /// <inheritdoc />
    public bool Match(string sourceName, string destName)
        => _comparer.Equals(sourceName, destName);
}
