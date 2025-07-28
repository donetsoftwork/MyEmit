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
}
