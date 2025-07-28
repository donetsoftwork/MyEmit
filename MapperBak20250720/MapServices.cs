namespace PocoEmit.Mapper;

/// <summary>
/// 映射扩展方法
/// </summary>
public static class MapServices
{
    /// <summary>
    /// 映射
    /// </summary>
    /// <typeparam name="TFrom"></typeparam>
    /// <typeparam name="TTo"></typeparam>
    /// <param name="mapper">映射服务</param>
    /// <param name="from">映射源</param>
    /// <returns></returns>
    public static TTo MapFrom<TFrom, TTo>(IMapper<TFrom, TTo> mapper, TFrom from)
    {
        var to = mapper.Create();
        mapper.CopyTo(from, to);
        return to;
    }
    /// <summary>
    /// 映射
    /// </summary>
    /// <typeparam name="TFrom"></typeparam>
    /// <typeparam name="TTo"></typeparam>
    /// <param name="mapper">映射服务</param>
    /// <param name="from">映射源</param>
    /// <param name="parameters">参数</param>
    /// <returns></returns>
    public static TTo MapFrom<TFrom, TTo>(IMapper<TFrom, TTo> mapper, TFrom from, params object[] parameters)
    {
        var to = mapper.CreateByParameters(parameters);
        mapper.CopyTo(from, to);
        return to;
    }
}
