using Hand.Reflection;
using PocoEmit.Configuration;
using PocoEmit.Converters;
using System.Linq.Expressions;

namespace PocoEmit.Complexes;

/// <summary>
/// 当前构建器上下文
/// </summary>
/// <param name="context"></param>
/// <param name="bundle"></param>
/// <param name="convertContextParameter"></param>
public class CurrentContext(BuildContext context, ComplexBundle bundle, ParameterExpression convertContextParameter)
    : IBuildContext
{
    #region 配置
    private readonly BuildContext _context = context;
    private readonly bool _hasCache = convertContextParameter != null && bundle.HasCache;
    private readonly ParameterExpression _convertContextParameter = convertContextParameter;
    /// <summary>
    /// 构建器上下文
    /// </summary>
    public BuildContext Context
        => _context;
    private ComplexBundle _bundle = bundle;
    /// <summary>
    /// 执行上下文
    /// </summary>
    public ParameterExpression ConvertContextParameter
        => _convertContextParameter;
    /// <inheritdoc />
    public bool HasCache 
        => _hasCache; 
    /// <inheritdoc />
    public IComplexBundle Bundle
         => _bundle;
    #endregion
    #region IBuildContext
    /// <inheritdoc />
    Expression IBuildContext.InitContext(ParameterExpression context)
        =>_context.InitContext(context);
    /// <inheritdoc />
    ComplexBundle IBuildContext.GetBundle(in PairTypeKey key)
        => _context.GetBundle(key);
    /// <inheritdoc />
    public Expression Call(bool isCircle, LambdaExpression lambda, params Expression[] arguments)
        => _context.Call(isCircle, lambda, arguments);
    /// <inheritdoc />
    ICompiledConverter IBuildContext.GetAchieve(in PairTypeKey key)
        => _context.GetAchieve(key);
    /// <inheritdoc />
    IEmitContextConverter IBuildContext.GetContexAchieve(in PairTypeKey key)
        =>_context.GetContexAchieve(key);
    #endregion
}
