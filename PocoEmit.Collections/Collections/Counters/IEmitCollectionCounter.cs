using PocoEmit.Configuration;

namespace PocoEmit.Collections.Counters;

/// <summary>
/// 子元素数量获取器
/// </summary>
public interface IEmitElementCounter
    : IEmitCollection, IEmitCounter, ICompileInfo
{
}
