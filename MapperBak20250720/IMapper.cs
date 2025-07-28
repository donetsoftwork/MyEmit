namespace PocoEmit.Mapper;

/// <summary>
/// 映射
/// </summary>
public interface IMapper
{
    /// <summary>
    /// 复制
    /// </summary>
    /// <param name="from"></param>
    /// <param name="to"></param>
    void CopyTo(object from, object to);
}