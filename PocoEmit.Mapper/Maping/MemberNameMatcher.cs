using PocoEmit.Members;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PocoEmit.Maping;

/// <summary>
/// 成员名匹配
/// </summary>
/// <param name="nameMatch"></param>
public sealed class MemberNameMatcher(INameMatch nameMatch)
    : IMemberMatch
{
    /// <summary>
    /// 成员名匹配
    /// </summary>
    /// <param name="comparer"></param>
    public MemberNameMatcher(IEqualityComparer<string> comparer)
        : this(new NameMatcher(comparer))
    {
    }
    /// <summary>
    /// 成员名匹配
    /// </summary>
    public MemberNameMatcher()
    : this(new NameMatcher(StringComparer.OrdinalIgnoreCase))
    {
    }
    #region 配置
    private readonly INameMatch _nameMatch = nameMatch;
    /// <summary>
    /// 名称匹配规则
    /// </summary>
    public INameMatch NameMatch
        => _nameMatch;
    #endregion
    /// <inheritdoc />
    public bool Match(IMember source, IMember dest)
        => _nameMatch.Match(source.Name, dest.Name);
    /// <summary>
    /// 筛选
    /// </summary>
    /// <param name="sources"></param>
    /// <param name="dest"></param>
    /// <returns></returns>
    public IEnumerable<TSource> Select<TSource>(IEnumerable<TSource> sources, IMember dest)
        where TSource : IMember
    {
        var name = dest.Name;
        return sources.Where(source => _nameMatch.Match(source.Name, name));
    }
    /// <summary>
    /// 默认实例
    /// </summary>
    public static MemberNameMatcher Default
        => Inner.Instance;
    /// <summary>
    /// 内部延迟初始化
    /// </summary>
    class Inner
    {
        public static readonly MemberNameMatcher Instance = new();
    }
}
