using System.Collections.Generic;

namespace PocoEmit.Maping;

/// <summary>
/// 继承识别器
/// </summary>
/// <param name="inner"></param>
public class InheritRecognizer(IRecognizer inner)
    : Recognizer(inner)
{
    #region 配置
    private readonly IRecognizer _inner = inner;
    /// <summary>
    /// 内部识别器
    /// </summary>
    public IRecognizer Inner 
        => _inner;
    /// <inheritdoc />
    public override IEnumerable<string> Prefixes
        => Concat(_inner.Prefixes, _prefixes);
    /// <inheritdoc />
    public override IEnumerable<string> Suffixes
         => Concat(_inner.Suffixes, _suffixes);
    #endregion
    /// <summary>
    /// 合并
    /// </summary>
    /// <param name="list0"></param>
    /// <param name="list"></param>
    /// <returns></returns>
    private static IEnumerable<string> Concat(IEnumerable<string> list0, HashSet<string> list)
    {
        foreach (var item in list0)
        {
            if(list.Contains(item))
                continue;
            yield return item;
        }
        foreach (var item in list)
            yield return item;
    }
}
