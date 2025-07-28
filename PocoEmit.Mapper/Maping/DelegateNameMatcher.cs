using System;

namespace PocoEmit.Maping;

/// <summary>
/// 委托成员名匹配
/// </summary>
/// <param name="matchFunc"></param>
public class DelegateNameMatcher(Func<string, string, bool> matchFunc)
    : INameMatch
{
    private readonly Func<string, string, bool> _matchFunc = matchFunc;
    /// <inheritdoc />
    public bool Match(string sourceName, string targetName)
        => _matchFunc(sourceName, targetName);
}

