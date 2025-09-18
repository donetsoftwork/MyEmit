using System;
using System.Collections.Generic;

namespace PocoEmit.Resolves;

/// <summary>
/// 单一类型转化上下文
/// </summary>
/// <typeparam name="TSource"></typeparam>
/// <typeparam name="TDest"></typeparam>
/// <param name="convertFunc"></param>
public class SingleContext<TSource, TDest>(Func<IConvertContext, TSource, TDest> convertFunc)
    : IConvertContext
{
    private readonly Dictionary<TSource, TDest> _cacher = [];
    private readonly Func<IConvertContext, TSource, TDest> _convertFunc = convertFunc;
    /// <summary>
    /// 转化
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    public TDest Convert(TSource source)
    {
        if(_cacher.TryGetValue(source, out var dest))
            return dest;
        return _convertFunc(this, source);
    }

    #region IConvertContext
    /// <inheritdoc />
    T IConvertContext.Convert<S, T>(S s)
    {
        if(s is TSource source && Convert(source) is T t)
            return t;
        return default;
    }
    /// <inheritdoc />
    void IConvertContext.SetCache<S, T>(S s, T t)
    {
        if(s is TSource source && t is TDest dest)
            _cacher[source] = dest;
    }
    #endregion
}
