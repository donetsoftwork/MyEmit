namespace PocoEmit.Resolves;

/// <summary>
/// 转化执行上下文
/// </summary>
public interface IConvertContext
{
    /// <summary>
    /// 设置缓存
    /// </summary>
    /// <param name="source"></param>
    /// <param name="dest"></param>
    void SetCache<TSource, TDest>(TSource source, TDest dest);
    /// <summary>
    /// 转化
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TDest"></typeparam>
    /// <param name="source"></param>
    /// <returns></returns>
    TDest Convert<TSource, TDest>(TSource source);
}
