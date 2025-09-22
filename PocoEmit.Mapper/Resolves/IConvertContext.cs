using PocoEmit.Converters;
using System;

namespace PocoEmit.Resolves;

/// <summary>
/// 转化执行上下文
/// </summary>
public interface IConvertContext : IDisposable
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
    /// <param name="converter"></param>
    /// <param name="source"></param>
    /// <returns></returns>
    TDest Convert<TSource, TDest>(IContextConverter converter, TSource source);
}
