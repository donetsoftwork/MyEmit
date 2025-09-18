using PocoEmit.Builders;
using PocoEmit.Complexes;
using PocoEmit.Configuration;
using PocoEmit.Converters;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace PocoEmit.Collections.Converters;

/// <summary>
/// 包装转化器
/// </summary>
public class WrapConverter(IMapperOptions options, Type sourceType, Type destType, IComplexIncludeConverter original)
    : IWrapper<IComplexIncludeConverter>
    , IEmitConverter
    , IComplexIncludeConverter    
    , IBuilder<LambdaExpression>
{
    #region 配置
    /// <summary>
    /// Emit配置
    /// </summary>
    protected readonly IMapperOptions _options = options;
    private readonly Type _sourceType = sourceType;
    private readonly Type _destType = destType;
    private readonly PairTypeKey _key = new(sourceType, destType);
    private readonly IComplexIncludeConverter _original = original;
    /// <summary>
    /// Emit配置
    /// </summary>
    public IMapperOptions Options
        => _options;
    /// <inheritdoc />
    public PairTypeKey Key
        => _key;
    /// <inheritdoc />
    public IComplexIncludeConverter Original
        => _original;
    /// <inheritdoc />
    bool ICompileInfo.Compiled
        => false;
    #endregion
    #region IEmitConverter
    /// <inheritdoc />
    public Expression Convert(Expression source)
        => _original.Convert(BuildContext.WithPrepare(_options, this), source);
    #endregion
    #region IBuilder<LambdaExpression>
    /// <inheritdoc />
    public LambdaExpression Build()
    {
        var context = BuildContext.WithPrepare(_options, this);
        var source = Expression.Variable(_sourceType, "source");
        List<Expression> list = [];
        var convertContexts = context.ConvertContexts;
        if (convertContexts.Count == 1)
            list.Add(context.InitContext(convertContexts[0]));
        var funcType = Expression.GetFuncType(_sourceType, _destType);
        var expression = _original.Convert(context, source);
        list.Add(expression);
        return Expression.Lambda(funcType, Expression.Block([.. convertContexts], list), source);
    }
    #endregion
    /// <inheritdoc />
    IEnumerable<ComplexBundle> IComplexPreview.Preview(IComplexBundle parent)
        => _original.Preview(parent);
    Expression IComplexIncludeConverter.Convert(IBuildContext context, Expression source)
        => _original.Convert(context, source);    

}
