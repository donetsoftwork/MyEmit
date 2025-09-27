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
/// <param name="original"></param>
/// <param name="lambda"></param>
/// <param name="convertFunc"></param>
public sealed class CompiledConverter<TSource, TDest>(IPocoOptions options,in PairTypeKey key, IEmitConverter original, LambdaExpression lambda, Func<TSource, TDest> convertFunc)
    : ArgumentFuncCallBuilder(options, key, lambda)
    , IPocoConverter<TSource, TDest>
    , IWrapper<IEmitConverter>
    , ICompiledConverter  
{
    /// <summary>
    /// 已编译转化器
    /// </summary>
    /// <param name="options"></param>
    /// <param name="key"></param>
    /// <param name="original"></param>
    public CompiledConverter(IPocoOptions options, in PairTypeKey key, IEmitConverter original)
        : this(options, key, original, null, null)
    {
    }
    #region 配置
    private readonly IEmitConverter _original = original;
    /// <summary>
    /// 原始转化器
    /// </summary>
    public IEmitConverter Original
        => _original;
    private Func<TSource, TDest> _convertFunc = convertFunc;
    /// <summary>
    /// 类型转化方法
    /// </summary>
    public Func<TSource, TDest> ConvertFunc
        => _convertFunc;
    /// <inheritdoc />
    bool ICompileInfo.Compiled
        => _convertFunc is not null;
    #endregion
    /// <inheritdoc />
    Expression IEmitConverter.Convert(Expression source)
        => Call(source);
    /// <inheritdoc />
    public TDest Convert(TSource source)
        => _convertFunc(source);
    /// <inheritdoc />
    object IObjectConverter.ConvertObject(object source)
        => _convertFunc((TSource)source);
    /// <inheritdoc />
    public bool CompileDelegate(LambdaExpression lambda)
    {
        Build(lambda);
        _convertFunc = Compiler._instance.CompileDelegate(lambda) as Func<TSource, TDest>;
        if(_convertFunc is null)
            return false;
        return true;
    }
}
