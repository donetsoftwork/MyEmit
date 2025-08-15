namespace PocoEmit.Collections;

/// <summary>
/// 获取集合元素数量
/// </summary>
/// <typeparam name="TCollection"></typeparam>
public interface ICounter<TCollection>
{
    /// <summary>
    /// 获取集合元素数量
    /// </summary>
    /// <param name="collection"></param>
    /// <returns></returns>
    int Count(TCollection collection);
}
