namespace PocoEmit.Collections;

/// <summary>
/// 配置(用户行为)
/// </summary>
/// <typeparam name="TKey"></typeparam>
/// <typeparam name="TConfig"></typeparam>
public interface IConfigure<TKey, TConfig>
{
    /// <summary>
    /// 配置
    /// </summary>
    /// <param name="key"></param>
    /// <param name="config"></param>
    void Configure(in TKey key, TConfig config);
}
