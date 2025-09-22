using PocoEmit.Activators;
using PocoEmit.Builders;
using PocoEmit.Complexes;
using PocoEmit.Configuration;
using PocoEmit.Copies;
using PocoEmit.Visitors;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace PocoEmit.Converters;

/// <summary>
/// 复合类型转化器
/// </summary>
/// <param name="options"></param>
/// <param name="key"></param>
/// <param name="destActivator"></param>
/// <param name="copier"></param>
public class ComplexTypeConverter(IMapperOptions options, PairTypeKey key, IEmitActivator destActivator, IEmitCopier copier)
    : IEmitComplexConverter
    , IBuilder<LambdaExpression>
{
    #region 配置
    /// <summary>
    /// Emit配置
    /// </summary>
    protected readonly IMapperOptions _options = options;
    private readonly PairTypeKey _key = key;
    private readonly Type _sourceType = key.LeftType; 
    private readonly IEmitActivator _destActivator = destActivator;
    private readonly IEmitCopier _copier = copier;

    /// <summary>
    /// Emit配置
    /// </summary>
    public IMapperOptions Options
        => _options;
    /// <inheritdoc />
    public PairTypeKey Key
        => _key;
    /// <summary>
    /// 映射源类型
    /// </summary>
    public Type SourceType
        => _sourceType;
    /// <summary>
    /// 激活映射目标
    /// </summary>
    public IEmitActivator DestActivator 
        => _destActivator;
    /// <summary>
    /// 复制
    /// </summary>
    public IEmitCopier Copier 
        => _copier;
    /// <inheritdoc />
    bool ICompileInfo.Compiled
        => false;
    #endregion
    #region IEmitConverter
    /// <inheritdoc />
    Expression IEmitConverter.Convert(Expression source)
        => _options.Call(Build(), source);
    #endregion
    #region IBuilder<LambdaExpression>
    /// <summary>
    /// 构造表达式
    /// </summary>
    /// <returns></returns>
    public LambdaExpression Build()
        => Build(BuildContext.WithPrepare(_options, this));
    #endregion
    #region IEmitComplexConverter
    /// <inheritdoc />
    public Expression Convert(IBuildContext context, Expression source)
    {
        var lambda = BuildWithContext(context);
        var parameter = context.GetConvertContextParameter(_key);
        if (parameter is null)
            return _options.Call(lambda, source);
        else
            return Expression.Invoke(lambda, parameter, source);
    }
    /// <inheritdoc />
    public LambdaExpression Build(IBuildContext context)
    {
        if(context.TryGetLambda(_key, out LambdaExpression lambda))
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
    /// <summary>
    /// 构造上下文
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
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
        
        List<Expression> list = [];
        if (PairTypeKey.CheckNullCondition(_sourceType))
        {
            list.Add(Expression.IfThen(
                    Expression.NotEqual(source, Expression.Constant(null, _sourceType)),
                    Expression.Block(ConvertCore(currentContext, source, dest))
                )
            );
        }
        else
        {
            list.AddRange(ConvertCore(currentContext, source, dest));
        }
        var convertContextParameter = currentContext.ConvertContextParameter;
        if (convertContextParameter is null)
        {
            var body = Expression.Block([dest], [..list, dest]);
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
    /// <summary>
    /// 转化核心方法
    /// </summary>
    /// <param name="context"></param>
    /// <param name="source"></param>
    /// <param name="dest"></param>
    /// <returns></returns>
    protected List<Expression> ConvertCore(IBuildContext context, Expression source, Expression dest)
    {
        var list = new List<Expression>
        {
            Expression.Assign(dest, _destActivator.New(context, source))
        };
        var cache = context.SetCache(_key, source, dest);
        if(cache is not null)
            list.Add(cache);
        if (_copier is not null)
            list.AddRange(CleanVisitor.Clean(_copier.Copy(context, source, dest)));
        return list;
    }
    /// <inheritdoc />
    public IEnumerable<ComplexBundle> Preview(IComplexBundle parent)
    {
        var bundle = parent.Accept(_key, this, false);
        if (bundle is null)
            yield break;
        yield return bundle;
        if (_destActivator is IComplexPreview previewActivator)
        {
            foreach (var item in previewActivator.Preview(bundle))
                yield return item;
        }
        if (_copier is not null)
        {
            foreach (var item in _copier.Preview(bundle))
                yield return item;
        }
    }
}
