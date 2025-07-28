namespace PocoEmit;

/// <summary>
/// 类型转化
/// </summary>
/// <typeparam name="TSource"></typeparam>
/// <typeparam name="TDest"></typeparam>
public interface IPocoConverter<TSource, TDest>
{
    /// <summary>
    /// 转化
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    TDest Convert(TSource source);
}
