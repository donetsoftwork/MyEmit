namespace PocoEmit.Mapper.Services;

/// <summary>
/// 生成对象
/// </summary>
public interface IActivatorService<TInstance>
{
    /// <summary>
    /// 构造新对象
    /// </summary>
    /// <returns></returns>
    TInstance Create();
    /// <summary>
    /// 按参数构造新对象
    /// </summary>
    /// <param name="parameters"></param>
    /// <returns></returns>
    TInstance CreateByParameters(object[] parameters);
}
