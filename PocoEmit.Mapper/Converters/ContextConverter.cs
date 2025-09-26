using PocoEmit.Builders;
using PocoEmit.Configuration;
using PocoEmit.Resolves;
using System;
using System.Linq.Expressions;

namespace PocoEmit.Converters;

/// <summary>
/// 上下文转化
/// </summary>
/// <typeparam name="TSource"></typeparam>
/// <typeparam name="TDest"></typeparam>
/// <param name="mapper"></param>
/// <param name="key"></param>
/// <param name="lambda"></param>
/// <param name="convertFunc"></param>
public class ContextConverter<TSource, TDest>(IMapperOptions mapper, in PairTypeKey key, LambdaExpression lambda, Func<IConvertContext, TSource, TDest> convertFunc)
    : IContextConverter<TSource, TDest>
    , IEmitContextConverter
{
    /// <summary>
    /// 上下文转化
    /// </summary>
    public ContextConverter(IMapperOptions mapper, in PairTypeKey key)
        : this(mapper, in key, null, null)
    { 
    }
    #region 配置
    private readonly IMapperOptions _mapper = mapper;
    private readonly PairTypeKey _key = key;
    private LambdaExpression _lambda = lambda;
    private Func<IConvertContext, TSource, TDest> _convertFunc = convertFunc;
    /// <summary>
    /// 
    /// </summary>
    public IMapperOptions Mapper
        => _mapper;
    /// <inheritdoc />
    public PairTypeKey Key
        => _key;
    /// <summary>
    /// Func表达式
    /// </summary>
    public LambdaExpression Lambda
        => _lambda;
    /// <summary>
    /// 类型转化方法
    /// </summary>
    public Func<IConvertContext, TSource, TDest> ConvertFunc
        => _convertFunc;
    /// <inheritdoc />
    bool ICompileInfo.Compiled
        => _convertFunc is not null;
    #endregion
    /// <inheritdoc />
    LambdaExpression IBuilder<LambdaExpression>.Build()
        => _lambda;
    /// <inheritdoc />
    public bool CompileDelegate(LambdaExpression lambda)
    {
        _lambda = lambda ?? throw new ArgumentNullException(nameof(lambda));
        _convertFunc = Compiler._instance.CompileDelegate(lambda) as Func<IConvertContext, TSource, TDest>;
        if (_convertFunc is null )
            return false;

        return true;
    }
    /// <inheritdoc />
    public TDest Convert(IConvertContext context, TSource source)
        => _convertFunc(context, source);
    /// <inheritdoc />
    public Expression Convert(Expression context, Expression argument)
        => _mapper.Call(_lambda, context, argument);
}
