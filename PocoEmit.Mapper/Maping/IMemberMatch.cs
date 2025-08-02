using PocoEmit.Members;
using System.Collections.Generic;

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
    /// <summary>
    /// 筛选
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <param name="sources"></param>
    /// <param name="dest"></param>
    /// <returns></returns>
    IEnumerable<TSource> Select<TSource>(IEnumerable<TSource> sources, IMember dest)
        where TSource : IMember;
}
