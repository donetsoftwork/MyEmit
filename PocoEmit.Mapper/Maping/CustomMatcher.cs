using PocoEmit.Members;
using System.Collections.Generic;

namespace PocoEmit.Maping;

/// <summary>
/// 自定义匹配
/// </summary>
/// <param name="nameMatch"></param>
/// <param name="settings"></param>
/// <param name="sourceRecognizer"></param>
/// <param name="destRecognizer"></param>
public class CustomMatcher(INameMatch nameMatch, IDictionary<string, string> settings, IgnoreRecognizer sourceRecognizer, IgnoreRecognizer destRecognizer)
    : MemberNameMatcher(nameMatch), IMemberMatch
{
    #region 配置
    private readonly IDictionary<string, string> _settings = settings;
    private readonly IgnoreRecognizer _sourceRecognizer = sourceRecognizer;
    private readonly IgnoreRecognizer _destRecognizer = destRecognizer;
    /// <summary>
    /// 设置
    /// </summary>
    public IDictionary<string, string> Settings
        => _settings;
    /// <summary>
    /// 源成员识别器
    /// </summary>
    public IgnoreRecognizer SourceRecognizer
        => _sourceRecognizer;
    /// <summary>
    /// 目标成员识别器
    /// </summary>
    public IgnoreRecognizer DestRecognizer
        => _destRecognizer;
    #endregion
    /// <inheritdoc />
    protected override bool MatchName(string sourceName, string destName)
    {
        if (_settings.TryGetValue(destName, out var sourceNameSetting) && base.MatchName(sourceNameSetting, sourceName))
            return true;
        return base.MatchName(sourceName, destName);
    }
    /// <inheritdoc />
    protected override IEnumerable<IMember> RecognizeDest(IRecognizer recognizer, IMember dest)
        => _destRecognizer.Recognize(dest);
    /// <inheritdoc />
    protected override IEnumerable<IEmitMemberReader> RecognizeSource(IRecognizer recognizer, IEnumerable<IEmitMemberReader> sources)
        => _sourceRecognizer.Recognize(sources);
}
