namespace PocoEmit;

/// <summary>
/// 复制器
/// </summary>
/// <typeparam name="TSource"></typeparam>
/// <typeparam name="TDest"></typeparam>
public interface IPocoCopier<TSource, TDest>
{
    /// <summary>
    /// 复制
    /// </summary>
    /// <param name="from"></param>
    /// <param name="to"></param>
    void Copy(TSource from, TDest to);
}
