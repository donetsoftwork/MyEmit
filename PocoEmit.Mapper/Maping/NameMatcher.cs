using System;

namespace PocoEmit.Maping;

/// <summary>
/// 成员名匹配
/// </summary>
/// <param name="comparison"></param>
public sealed class NameMatcher(StringComparison comparison)
    : INameMatch
{
    /// <summary>
    /// 成员名匹配
    /// </summary>
    public NameMatcher()
        : this(StringComparison.OrdinalIgnoreCase)
    {
    }
    #region 配置
    /// <summary>
    /// 比较规则
    /// </summary>
    private readonly StringComparison _comparison = comparison;
    /// <summary>
    /// 比较规则
    /// </summary>
    public StringComparison Comparison
        => _comparison;
    #endregion
    /// <inheritdoc />
    public bool Match(string sourceName, string destName)
        => sourceName.Equals(destName, _comparison);
    /// <inheritdoc />
    public bool StartsWith(string name, string prefix)
        => name.StartsWith(prefix, _comparison);
    /// <inheritdoc />
    public bool EndsWith(string name, string suffix)
        => name.EndsWith(suffix, _comparison);
}
