using System;

namespace PocoEmit.Mapper.Services;

/// <summary>
/// 委托映射
/// </summary>
/// <typeparam name="TFrom"></typeparam>
/// <typeparam name="TTo"></typeparam>
/// <param name="mapper">映射委托</param>
public class DelegateMapper<TFrom, TTo>(Action<TFrom, TTo> mapper)
    : MapperBase<TFrom, TTo>
{
    #region 配置
    private readonly Action<TFrom, TTo> _mapper = mapper;
    /// <summary>
    /// 映射委托
    /// </summary>
    public Action<TFrom, TTo> Mapper 
        => _mapper;
    #endregion

    /// <inheritdoc />
    public override void CopyTo(TFrom from, TTo to)
        => _mapper.Invoke(from, to);
}
