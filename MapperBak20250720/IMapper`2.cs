using PocoEmit.Mapper.Services;

namespace PocoEmit.Mapper;

/// <summary>
/// 映射
/// </summary>
/// <typeparam name="TFrom"></typeparam>
/// <typeparam name="TTo"></typeparam>
public interface IMapper<TFrom, TTo> : IActivatorService<TTo>, IMapper
{
    /// <summary>
    /// 复制
    /// </summary>
    /// <param name="from"></param>
    /// <param name="to"></param>
    void CopyTo(TFrom from, TTo to);
}
