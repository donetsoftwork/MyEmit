namespace PocoEmit.Builders;

/// <summary>
/// 池化接口
/// </summary>
/// <typeparam name="TResource"></typeparam>
public interface IPool<TResource>
{
    /// <summary>
    /// 获取对象
    /// </summary>
    /// <returns></returns>
    TResource Get();
    /// <summary>
    /// 归还对象
    /// </summary>
    /// <param name="resource"></param>
    void Return(TResource resource);
}
