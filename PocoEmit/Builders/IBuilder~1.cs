namespace PocoEmit.Builders;

/// <summary>
/// 构建器
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IBuilder<out T>
{
    /// <summary>
    /// 构建
    /// </summary>
    T Build();
}
