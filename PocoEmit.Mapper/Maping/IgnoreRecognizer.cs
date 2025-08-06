using PocoEmit.Members;
using System.Collections.Generic;

namespace PocoEmit.Maping;

/// <summary>
/// 可忽略识别器
/// </summary>
/// <param name="inner"></param>
public class IgnoreRecognizer(IRecognizer inner)
    : InheritRecognizer(inner)
{
    #region 配置
    private readonly HashSet<string> _ignoreNames = [];
    /// <summary>
    /// 忽略名
    /// </summary>
    public IEnumerable<string> IgnoreNames
        => _ignoreNames;
    #endregion
    /// <summary>
    /// 忽略
    /// </summary>
    /// <param name="name"></param>
    public void Ignore(string name)
    {
        if(_ignoreNames.Contains(name))
            return;
        _ignoreNames.Add(name);
    }
    /// <summary>
    /// 清空前缀
    /// </summary>
    public void ClearPrefix()
    {
        _prefixes.Clear();
    }
    #region IRecognizer
    /// <inheritdoc />
    public override IEnumerable<IMember> Recognize(IMember member)
    {
        foreach (var item in base.Recognize(member))
        {
            if (_ignoreNames.Contains(item.Name))
                continue;
            yield return item;
        }
    }
    /// <inheritdoc />
    public override IEnumerable<IEmitMemberReader> Recognize(IEnumerable<IEmitMemberReader> readers)
    {
        foreach (var item in base.Recognize(readers))
        {
            if (_ignoreNames.Contains(item.Name))
                continue;
            yield return item;
        }
    }
    #endregion
}
