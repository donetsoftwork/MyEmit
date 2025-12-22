namespace PocoEmit.Configuration;

/// <summary>
/// 编译信息
/// </summary>
public interface ICompileInfo
{
    /// <summary>
    /// 是否已编译
    /// </summary>
    bool Compiled { get; }
}
