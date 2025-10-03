namespace PocoEmit.Collections;

/// <summary>
/// 配置设置(用户行为)
/// </summary>
/// <typeparam name="TKey"></typeparam>
/// <typeparam name="TConfig"></typeparam>
public interface ISettings<TKey, TConfig>
    : IConfigure<TKey, TConfig>
{
    /// <summary>
    /// 获取
    /// </summary>
    /// <param name="key"></param>
    /// <param name="config"></param>
    /// <returns></returns>
    bool TryGetConfig(TKey key, out TConfig config);
}
