namespace PocoEmit.Maping;

/// <summary>
/// 名称匹配接口
/// </summary>
public interface INameMatch
{
    /// <summary>
    /// 匹配
    /// </summary>
    /// <param name="sourceName"></param>
    /// <param name="destName"></param>
    /// <returns></returns>
    bool Match(string sourceName, string destName);
    /// <summary>
    /// 前缀匹配
    /// </summary>
    /// <param name="name"></param>
    /// <param name="prefix"></param>
    /// <returns></returns>
    bool StartsWith(string name, string prefix);
    /// <summary>
    /// 后缀匹配
    /// </summary>
    /// <param name="name"></param>
    /// <param name="suffix"></param>
    /// <returns></returns>
    bool EndsWith(string name, string suffix);
}
