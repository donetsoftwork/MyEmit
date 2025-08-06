using PocoEmit.Members;
using System.Collections.Generic;

namespace PocoEmit.Maping;

/// <summary>
/// 识别器
/// </summary>
public class Recognizer(INameMatch matcher, HashSet<string> prefixes, HashSet<string> suffixes)
    : IRecognizer
{
    /// <summary>
    /// 识别器
    /// </summary>
    /// <param name="match"></param>
    public Recognizer(INameMatch match)
        : this(match, [], [])
    {
    }
    /// <summary>
    /// 识别器
    /// </summary>
    /// <param name="recognizer"></param>
    public Recognizer(IRecognizer recognizer)
        : this(recognizer.Matcher, [.. recognizer.Prefixes], [.. recognizer.Suffixes])
    {
    }
    #region 配置
    private readonly INameMatch _matcher = matcher;
    /// <summary>
    /// 前缀
    /// </summary>
    protected readonly HashSet<string> _prefixes = prefixes;
    /// <summary>
    /// 后缀
    /// </summary>
    protected readonly HashSet<string> _suffixes = suffixes;
    /// <inheritdoc />
    public virtual IEnumerable<string> Prefixes
        => _prefixes;
    /// <inheritdoc />
    public virtual IEnumerable<string> Suffixes 
        => _suffixes;
    /// <summary>
    /// 匹配器
    /// </summary>
    public INameMatch Matcher 
        => _matcher;
    #endregion
    /// <inheritdoc />
    public void AddPrefix(string prefix)
    {
        if (_prefixes.Contains(prefix))
            return;
        _prefixes.Add(prefix);
    }
    /// <inheritdoc />
    public void AddSuffix(string suffix)
    {
        if (_suffixes.Contains(suffix))
            return;
        _suffixes.Add(suffix);
    }
    /// <inheritdoc />
    public virtual IEnumerable<IMember> Recognize(IMember member)
    {
        yield return member;
        var name = member.Name;
        foreach (var prefix in Prefixes)
        {
            if (_matcher.StartsWith(name, prefix))
                yield return new RenameMember<IMember>(member, name.Substring(prefix.Length));                
        }
        foreach (var suffix in Suffixes)
        { 
            if (_matcher.EndsWith(name, suffix))
                yield return new RenameMember<IMember>(member, name.Substring(0, name.Length - suffix.Length));
        }
    }
    /// <inheritdoc />
    public virtual IEnumerable<IEmitMemberReader> Recognize(IEnumerable<IEmitMemberReader> readers)
    {
        foreach (var member in readers)
        {
            yield return member;
            var name = member.Name;
            foreach (var prefix in Prefixes)
            {
                if (_matcher.StartsWith(name, prefix))
                    yield return new RenameReader(member, name.Substring(prefix.Length));
            }
            foreach (var suffix in Suffixes)
            {
                if (_matcher.EndsWith(name, suffix))
                    yield return new RenameReader(member, name.Substring(0, name.Length - suffix.Length));
            }
        }
    }
}
