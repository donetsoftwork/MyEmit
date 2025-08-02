using PocoEmit.Members;
using System.Collections.Generic;

namespace PocoEmit.Maping;

/// <summary>
/// 自定义匹配
/// </summary>
/// <param name="settings"></param>
/// <param name="sourceIgnores"></param>
/// <param name="destIgnores"></param>
/// <param name="comparer"></param>
public class CustomMatcher(IDictionary<string, string> settings, HashSet<string> sourceIgnores, HashSet<string> destIgnores, IEqualityComparer<string> comparer)
    : NameMatcher(comparer), IMemberMatch
{
    #region 配置
    private readonly IDictionary<string, string> _settings = settings;
    private readonly HashSet<string> _sourceIgnores = sourceIgnores;
    private readonly HashSet<string> _destIgnores = destIgnores;
    /// <summary>
    /// 设置
    /// </summary>
    public IDictionary<string, string> Settings 
        => _settings;
    /// <summary>
    /// 忽略源成员
    /// </summary>
    public HashSet<string> SourceIgnores
        => _sourceIgnores;
    /// <summary>
    /// 忽略目标成员
    /// </summary>
    public HashSet<string> DestIgnores 
        => _destIgnores;
    #endregion
    /// <inheritdoc />
    public override bool Match(string sourceName, string destName)
    {
        if(_sourceIgnores.Contains(sourceName) || _destIgnores.Contains(destName))
            return false;       
        if (_settings.TryGetValue(destName, out var sourceNameSetting) && base.Match(sourceNameSetting, sourceName))
            return true;
        return base.Match(sourceName, destName);
    }
    /// <inheritdoc />
    public bool Match(IMember source, IMember dest)
        => Match(source.Name, dest.Name);
    /// <inheritdoc />
    public IEnumerable<TSource> Select<TSource>(IEnumerable<TSource> sources, IMember dest)
        where TSource : IMember
    {
        var destName = dest.Name;
        if(_destIgnores.Contains(destName))
            yield break;
        // 设置规则优先
        if (_settings.TryGetValue(destName, out var sourceNameSetting))
        {
            foreach (var source in sources)
            {
                if (base.Match(source.Name, sourceNameSetting))
                    yield return source;
            }
        }
        // 兜底逻辑
        foreach (var source in sources)
        {
            var sourceName = source.Name;
            if (_sourceIgnores.Contains(sourceName))
                continue;
            if (base.Match(sourceName, destName))
                yield return source;
        }
    }
}
