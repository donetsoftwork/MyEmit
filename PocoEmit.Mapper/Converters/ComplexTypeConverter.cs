using PocoEmit.Activators;
using PocoEmit.Builders;
using PocoEmit.Complexes;
using PocoEmit.Configuration;
using PocoEmit.Copies;
using PocoEmit.Resolves;
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
        var parameters = context.GetConvertContexts(_key);
        return _options.Call(lambda, [.. parameters, source]);
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
        List<Expression> list = [];   
        var convertContexts = context.ConvertContexts;
        if (convertContexts.Count == 1)
            list.Add(context.InitContext(convertContexts[0]));
        list.Add(_options.Call(contextLambda, [.. convertContexts, source]));

        var funcType = Expression.GetFuncType(_sourceType, destType);
        return Expression.Lambda(funcType, Expression.Block([.. convertContexts], list), source);
    }
    /// <summary>
    /// 构造上下文
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public LambdaExpression BuildWithContext(IBuildContext context)
    {
        if (context.TryGetContextLambda(_key, out LambdaExpression contextLambda))
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
        list.Add(dest);
        var convertContexts = currentContext.ConvertContexts;
        if (convertContexts.Count > 0)
        {
            var convertContext = convertContexts[0];
            var bundle = context.GetBundle(_key);
            if (bundle.IsCircle)
            {
                var funcType = Expression.GetFuncType(convertContext.Type, _sourceType, destType);
                return Expression.Lambda(funcType, Expression.Block([dest], list), [convertContext, source]);
            }
            else
            {
                var body = Expression.Block(
                    [convertContext, dest],
                    [context.InitContext(convertContext), .. list]
                    );
                return Expression.Lambda(Expression.GetFuncType([_sourceType, destType]), body, [source]);
            }
        }
        else
        {
            return Expression.Lambda(Expression.GetFuncType([_sourceType, destType]), Expression.Block([dest], list), [source]);
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
    private List<Expression> ConvertCore(IBuildContext context, Expression source, Expression dest)
    {
        var list = new List<Expression>
        {
            Expression.Assign(dest, _destActivator.New(context, source))
        };
        var convertContexts = context.ConvertContexts;
        if (convertContexts.Count == 1)
            list.Add(ConvertContext.CallSetCache(convertContexts[0], _key, source, dest));

        if (_copier is not null)
            list.AddRange(_copier.Copy(context, source, dest));
        return list;
    }
    /// <inheritdoc />
    public IEnumerable<ComplexBundle> Preview(IComplexBundle parent)
    {
        var bundle = parent.Accept(_key, this);
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
