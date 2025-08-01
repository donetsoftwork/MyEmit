using PocoEmit.Members;

namespace PocoEmit.Maping;

/// <summary>
/// 成员匹配
/// </summary>
public interface IMemberMatch
{
    /// <summary>
    /// 匹配
    /// </summary>
    /// <param name="source"></param>
    /// <param name="dest"></param>
    /// <returns></returns>
    bool Match(IMember source, IMember dest);
}
