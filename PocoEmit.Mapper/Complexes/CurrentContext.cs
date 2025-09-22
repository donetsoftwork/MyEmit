using PocoEmit.Configuration;
using System.Linq.Expressions;

namespace PocoEmit.Complexes;

/// <summary>
/// 当前构建器上下文
/// </summary>
/// <param name="context"></param>
/// <param name="convertContextParameter"></param>
public class CurrentContext(BuildContext context, ParameterExpression convertContextParameter)
    : IBuildContext
{
    #region 配置
    private readonly BuildContext _context = context;
    private readonly ParameterExpression _convertContextParameter = convertContextParameter;
    /// <summary>
    /// 构建器上下文
    /// </summary>
    public BuildContext Context
        => _context;
    /// <summary>
    /// 执行上下文
    /// </summary>
    public ParameterExpression ConvertContextParameter
        => _convertContextParameter;
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
    ContextAchieved IBuildContext.GetAchieve(PairTypeKey key)
        =>_context.GetAchieve(key);
    #endregion
}
