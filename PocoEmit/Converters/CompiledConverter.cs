using PocoEmit.Builders;
using PocoEmit.Configuration;
using System;
using System.Linq.Expressions;

namespace PocoEmit.Converters;

/// <summary>
/// 已编译转化器
/// </summary>
/// <typeparam name="TSource"></typeparam>
/// <typeparam name="TDest"></typeparam>
/// <param name="options"></param>
/// <param name="key"></param>
/// <param name="lambda"></param>
/// <param name="convertFunc"></param>
public sealed class CompiledConverter<TSource, TDest>(IPocoOptions options, PairTypeKey key, LambdaExpression lambda, Func<TSource, TDest> convertFunc)
    : ArgumentFuncCallBuilder(options, key, lambda)
    , ICompiledConverter<TSource, TDest>
{
    #region 配置
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
        => Call(source);
    /// <inheritdoc />
    TDest IPocoConverter<TSource, TDest>.Convert(TSource source)
        => _convertFunc(source);
    /// <inheritdoc />
    object IObjectConverter.ConvertObject(object source)
        => _convertFunc((TSource)source);
}
