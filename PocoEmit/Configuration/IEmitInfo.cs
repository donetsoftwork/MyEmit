namespace PocoEmit.Configuration;

/// <summary>
/// Emit信息
/// </summary>
public interface IEmitInfo
{
    /// <summary>
    /// 是否已编译
    /// </summary>
    bool Compiled { get; }
}
