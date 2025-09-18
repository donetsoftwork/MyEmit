using PocoEmit.Configuration;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace PocoEmit.Complexes;

/// <summary>
/// 当前构建器上下文
/// </summary>
public class CurrentContext(BuildContext context, List<ParameterExpression> convertContexts)
    : IBuildContext
{
    #region 配置
    private readonly BuildContext _context = context;
    private readonly List<ParameterExpression> _convertContexts = convertContexts;
    /// <summary>
    /// 构建器上下文
    /// </summary>
    public BuildContext Context
        => _context;
    /// <summary>
    /// 执行上下文
    /// </summary>
    public List<ParameterExpression> ConvertContexts
        => _convertContexts;
    #endregion
    #region IBuildContext
    /// <inheritdoc />
    Expression IBuildContext.InitContext(ParameterExpression context)
        =>_context.InitContext(context);
    /// <inheritdoc />
    ComplexBundle IBuildContext.GetBundle(PairTypeKey key)
        => _context.GetBundle(key);
    /// <inheritdoc />
    public Expression Call(LambdaExpression lambda, params Expression[] arguments)
        => _context.Call(lambda, arguments);
    /// <inheritdoc />
    bool IBuildContext.TryGetLambda(PairTypeKey key, out LambdaExpression lambda)
        => _context.TryGetLambda(key, out lambda);
    /// <inheritdoc />
    bool IBuildContext.TryGetContextLambda(PairTypeKey key, out LambdaExpression lambda)
        => _context.TryGetContextLambda(key, out lambda);
    /// <inheritdoc />
    bool IBuildContext.SetContextLambda(PairTypeKey key, LambdaExpression lambda)
        => _context.SetContextLambda(key, lambda);
    #endregion
}
