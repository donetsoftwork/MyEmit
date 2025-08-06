using PocoEmit.Members;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PocoEmit.Maping;

/// <summary>
/// 成员名匹配
/// </summary>
/// <param name="nameMatch"></param>
public class MemberNameMatcher(INameMatch nameMatch)
    : IMemberMatch
{
    /// <summary>
    /// 成员名匹配
    /// </summary>
    /// <param name="comparison"></param>
    public MemberNameMatcher(StringComparison comparison)
        : this(new NameMatcher(comparison))
    {
    }
    /// <summary>
    /// 成员名匹配
    /// </summary>
    public MemberNameMatcher()
    : this(new NameMatcher(StringComparison.OrdinalIgnoreCase))
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
    public IEnumerable<IEmitMemberReader> Select(IRecognizer recognizer, IEnumerable<IEmitMemberReader> sources, IMember dest)
    {
        var sourceVariants = RecognizeSource(recognizer, sources).ToArray();
        var destVariants = RecognizeDest(recognizer, dest);
        return destVariants.SelectMany(destVariant => SelectCore(destVariant, sourceVariants));
    }
    /// <summary>
    /// 识别成员
    /// </summary>
    /// <param name="recognizer"></param>
    /// <param name="dest"></param>
    /// <returns></returns>
    protected virtual IEnumerable<IMember> RecognizeDest(IRecognizer recognizer, IMember dest)
        => recognizer.Recognize(dest);
    /// <summary>
    /// 识别读取器
    /// </summary>
    /// <param name="recognizer"></param>
    /// <param name="sources"></param>
    /// <returns></returns>
    protected virtual IEnumerable<IEmitMemberReader> RecognizeSource(IRecognizer recognizer, IEnumerable<IEmitMemberReader> sources)
        => recognizer.Recognize(sources);
    /// <summary>
    /// 按成员名筛选
    /// </summary>
    /// <param name="dest"></param>
    /// <param name="sources"></param>
    /// <returns></returns>
    private IEnumerable<IEmitMemberReader> SelectCore(IMember dest, IEnumerable<IEmitMemberReader> sources)
    {
        var name = dest.Name;
        return sources.Where(source => MatchName(source.Name, name));
    }
    /// <summary>
    /// 按成员名匹配
    /// </summary>
    /// <param name="sourceName"></param>
    /// <param name="destName"></param>
    /// <returns></returns>
    protected virtual bool MatchName(string sourceName, string destName)
        => _nameMatch.Match(sourceName, destName);
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
