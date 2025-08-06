using PocoEmit.Configuration;
using System;
using System.Linq.Expressions;

namespace PocoEmit.Converters;

/// <summary>
/// 已编译转化器
/// </summary>
/// <typeparam name="TSource"></typeparam>
/// <typeparam name="TDest"></typeparam>
/// <param name="inner"></param>
/// <param name="convertFunc"></param>
public sealed class CompiledConverter<TSource, TDest>(IEmitConverter inner, Func<TSource, TDest> convertFunc)
    : ICompiledConverter<TSource, TDest>
{
    #region 配置
    private readonly IEmitConverter _inner = inner;
    /// <summary>
    /// 原始转化器
    /// </summary>
    public IEmitConverter Inner
        => _inner;
    private readonly Func<TSource, TDest> _convertFunc = convertFunc;
    /// <summary>
    /// 类型转化方法
    /// </summary>
    public Func<TSource, TDest> ConvertFunc
        => _convertFunc;
    /// <inheritdoc />
    bool ICompileInfo.Compiled
        => true;
    #endregion
    /// <inheritdoc />
    public Expression Convert(Expression source)
        => _inner.Convert(source);
    /// <inheritdoc />
    TDest IPocoConverter<TSource, TDest>.Convert(TSource source)
        => _convertFunc(source);
    /// <inheritdoc />
    object IObjectConverter.ConvertObject(object source)
        => _convertFunc((TSource)source);
}
