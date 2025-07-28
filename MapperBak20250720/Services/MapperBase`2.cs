using System;

namespace PocoEmit.Mapper.Services;

/// <summary>
/// 映射基类
/// </summary>
/// <typeparam name="TFrom"></typeparam>
/// <typeparam name="TTo"></typeparam>
public abstract class MapperBase<TFrom, TTo> : IMapper<TFrom, TTo>
{
    /// <inheritdoc />
    public abstract void CopyTo(TFrom from, TTo to);

    #region IActivatorService<TTo>
    /// <inheritdoc />
    public virtual TTo Create()
        => Activator.CreateInstance<TTo>();
    /// <inheritdoc />
    public virtual TTo CreateByParameters(object[] parameters)
        => (TTo)Activator.CreateInstance(typeof(TTo), parameters);
    #endregion
    void IMapper.CopyTo(object from, object to)
    {
        if(from is TFrom from1 && to is TTo to1)
            CopyTo(from1, to1);
    }
}
