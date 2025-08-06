using PocoEmit.Members;
using System.Collections.Generic;

namespace PocoEmit.Maping;

/// <summary>
/// 成员匹配
/// </summary>
public interface IMemberMatch
{
    /// <summary>
    /// 名称匹配规则
    /// </summary>
    INameMatch NameMatch { get; }
    /// <summary>
    /// 筛选
    /// </summary>
    /// <param name="recognizer"></param>
    /// <param name="sources"></param>
    /// <param name="dest"></param>
    /// <returns></returns>
    IEnumerable<IEmitMemberReader> Select(IRecognizer recognizer, IEnumerable<IEmitMemberReader> sources, IMember dest);
}
