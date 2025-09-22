using PocoEmit.Builders;
using PocoEmit.Complexes;
using PocoEmit.Configuration;
using PocoEmit.Visitors;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace PocoEmit.Collections.Converters;

/// <summary>
/// 包装转化器
/// </summary>
public class WrapConverter(IMapperOptions options, Type sourceType, Type destType, IComplexIncludeConverter original)
    : IWrapper<IComplexIncludeConverter>
    , IEmitComplexConverter
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
    #region IEmitComplexConverter
    /// <inheritdoc />
    public LambdaExpression Build(IBuildContext context)
    {
        if (context.TryGetLambda(_key, out LambdaExpression lambda))
            return lambda;
        var contextLambda = BuildWithContext(context);
        if (contextLambda.Parameters.Count == 1)
            return contextLambda;
        var destType = _key.RightType;
        var source = Expression.Variable(_sourceType, "source");

        Expression body;
        var convertContextParameter = context.ConvertContextParameter;
        if (convertContextParameter is null)
        {
            body = CleanVisitor.Clean(_options.Call(contextLambda, source));
        }
        else
        {
            var dest = Expression.Variable(destType, "dest");
            body = Expression.Block(
                [convertContextParameter, dest],
                context.InitContext(convertContextParameter),
                Expression.Assign(dest, Expression.Invoke(contextLambda, convertContextParameter, source)),
                EmitDispose.Dispose(convertContextParameter),
                dest
            );
        }
        var funcType = Expression.GetFuncType(_sourceType, destType);
        return Expression.Lambda(funcType, body, source);
    }
    /// <inheritdoc />
    public LambdaExpression BuildWithContext(IBuildContext context)
    {
        var achieved = context.GetAchieve(_key);
        var contextLambda = achieved?.Lambda;
        if (contextLambda is not null)
            return contextLambda;
        var destType = _key.RightType;
        var source = Expression.Variable(_sourceType, "source");
        var dest = Expression.Variable(destType, "dest");
        var currentContext = context.Enter(_key);

        var original = _original.Convert(currentContext, source);
        var convertCore = CleanVisitor.Clean(Expression.Assign(dest, original));
        List<Expression> list = [];
        if (PairTypeKey.CheckNullCondition(_sourceType))
        {
            list.Add(Expression.IfThen(
                    Expression.NotEqual(source, Expression.Constant(null, _sourceType)),
                    convertCore
                )
            );
        }
        else
        {
            list.Add(convertCore);
        }
        var convertContextParameter = currentContext.ConvertContextParameter;
        if (convertContextParameter is null)
        {
            var body = Expression.Block([dest], [.. list, dest]);
            return Expression.Lambda(Expression.GetFuncType([_sourceType, destType]), body, [source]);
        }
        var bundle = context.GetBundle(_key);
        if (bundle.HasCircle)
        {
            var funcType = Expression.GetFuncType(convertContextParameter.Type, _sourceType, destType);
            var body = CleanVisitor.Clean(Expression.Block([dest], [.. list, dest]));
            return Expression.Lambda(funcType, body, [convertContextParameter, source]);
        }
        else
        {
            var body = Expression.Block([dest], [.. list, dest]);
            return Expression.Lambda(Expression.GetFuncType([_sourceType, destType]), body, [source]);
        }
    }
    #endregion
    #region IBuilder<LambdaExpression>
    /// <inheritdoc />
    public LambdaExpression Build()
    {
        var context = BuildContext.WithPrepare(_options, this);
        var source = Expression.Variable(_sourceType, "source");
        var funcType = Expression.GetFuncType(_sourceType, _destType);

        var convertContextParameter = context.ConvertContextParameter;
        if (convertContextParameter is null)
            return Expression.Lambda(funcType, CleanVisitor.Clean(_original.Convert(context, source)), source);

        var dest = Expression.Variable(_destType, "dest");
        var body = Expression.Block(
            [convertContextParameter, dest],
            context.InitContext(convertContextParameter),
            CleanVisitor.Clean(Expression.Assign(dest, _original.Convert(context, source))),
            EmitDispose.Dispose(convertContextParameter),
            dest
            );
        return Expression.Lambda(funcType, body, source);
    }
    #endregion
    /// <inheritdoc />
    IEnumerable<ComplexBundle> IComplexPreview.Preview(IComplexBundle parent)
    {
        var bundle = parent.Accept(_key, this, true);
        if (bundle is null)
            yield break;
        foreach (var item in _original.Preview(bundle))
            yield return item;
    }
    Expression IComplexIncludeConverter.Convert(IBuildContext context, Expression source)
        => _original.Convert(context, source);
}
