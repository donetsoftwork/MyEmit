using PocoEmit.Builders;
using PocoEmit.Configuration;
using PocoEmit.Converters;
using System;
using System.Linq.Expressions;

namespace PocoEmit.Resolves;

/// <summary>
/// 单一类型转化
/// </summary>
/// <typeparam name="TSource"></typeparam>
/// <typeparam name="TDest"></typeparam>
/// <param name="key"></param>
/// <param name="lambda"></param>
/// <param name="convertFunc"></param>
public class SingleConverter<TSource, TDest>(PairTypeKey key, LambdaExpression lambda, Func<IConvertContext, TSource, TDest> convertFunc)
    : IBuilder<IConvertContext>
    //, ICompiledConverter<TSource, TDest>
    , IPocoConverter<TSource, TDest>
{
    #region 配置
    private readonly PairTypeKey _key = key;
    private readonly LambdaExpression _lambda = lambda;
    private readonly Func<IConvertContext, TSource, TDest> _convertFunc = convertFunc;
    /// <summary>
    /// 转化委托
    /// </summary>
    public Func<IConvertContext, TSource, TDest> ConvertFunc
        => _convertFunc;
    /// <summary>
    /// 表达式
    /// </summary>
    public LambdaExpression Lambda 
        => _lambda;
    /// <inheritdoc />
    public PairTypeKey Key 
        => _key;
    #endregion
    ///// <summary>
    ///// 参数赋值初始化
    ///// </summary>
    ///// <param name="context"></param>
    ///// <param name="mapper"></param>
    ///// <returns></returns>
    //public Expression InitParameter(ParameterExpression context, Expression mapper)
    //    => Expression.Assign(context, Expression.New(_constructor, mapper));
    #region IBuilder<IConvertContext>
    /// <inheritdoc />
    IConvertContext IBuilder<IConvertContext>.Build()
        => new SingleContext<TSource, TDest>(_convertFunc);
    #endregion
    #region IPocoConverter<TSource, TDest>
    /// <inheritdoc />
    public TDest Convert(TSource source)
    {
        var context = new SingleContext<TSource, TDest>(_convertFunc);
        return _convertFunc(context, source);
    }
    object IObjectConverter.ConvertObject(object source)
        => Convert((TSource)source);
    #endregion
}
