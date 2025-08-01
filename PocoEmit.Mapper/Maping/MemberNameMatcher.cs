using PocoEmit.Members;

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
    public MemberNameMatcher()
        : this(new NameMatcher())
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
