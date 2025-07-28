using System;

namespace PocoEmit.Copies;

/// <summary>
/// 已编译复制器
/// </summary>
/// <typeparam name="TSource"></typeparam>
/// <typeparam name="TDest"></typeparam>
public interface ICompiledCopier<TSource, TDest>
    : IEmitCopier
    , IPocoCopier<TSource, TDest>
{
    /// <summary>
    /// 复制器
    /// </summary>
    Action<TSource, TDest> CopyAction { get; }
}
