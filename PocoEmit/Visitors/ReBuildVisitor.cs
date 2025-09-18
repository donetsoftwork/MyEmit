using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;

namespace PocoEmit.Visitors;

/// <summary>
/// 重建(重建Variable和Block)
/// </summary>
public class ReBuildVisitor(IDictionary<ParameterExpression, ParameterExpression> parameters)
    : CheckVisitor
{
    /// <summary>
    /// 重建(重建Variable和Block)
    /// </summary>
    private ReBuildVisitor()
        : this(new Dictionary<ParameterExpression, ParameterExpression>())
    {
    }
    /// <summary>
    /// 重建(重建Variable和Block)
    /// </summary>
    /// <param name="parameters"></param>
    public ReBuildVisitor(ReadOnlyCollection<ParameterExpression> parameters)
        : this(parameters.ToDictionary(parameter => Expression.Parameter(parameter.Type, parameter.Name)))
    { 
    }
    /// <summary>
    /// 重建(重建Variable和Block)
    /// </summary>
    /// <param name="lambda"></param>
    public ReBuildVisitor(LambdaExpression lambda)
        : this(lambda.Parameters)
    {
    }
    #region 配置
    private readonly IDictionary<ParameterExpression, ParameterExpression> _parameters = parameters;
    /// <summary>
    /// 参数
    /// </summary>
    public IEnumerable<ParameterExpression> Parameters 
        => _parameters.Values;
    #endregion
    /// <summary>
    /// 替换变量
    /// </summary>
    /// <param name="variable"></param>
    /// <param name="variable2"></param>
    /// <returns></returns>
    private bool TryReplaceVariable(ParameterExpression variable, out ParameterExpression variable2)
    {
        if (_parameters.TryGetValue(variable, out variable2))
            return true;
        variable2 = variable;
        return false;
    }
    /// <inheritdoc />
    protected internal override IEnumerable<ParameterExpression> CheckVariables(IEnumerable<ParameterExpression> variables)
    {
        foreach (var variable in variables)
        {
            if(_parameters.TryGetValue(variable, out var parameter))
                yield return parameter;
            yield return variable;
        }
    }
    /// <inheritdoc />
    protected internal override bool TryReplaceParameter(ParameterExpression node, out Expression replacement)
    {
        if(node.NodeType == ExpressionType.Parameter && node is ParameterExpression variable)
        {
            if(TryReplaceVariable(variable, out var parameter))
            {
                replacement = parameter;
                return true;
            }
        }
        replacement = node;
        return false;
    }
    /// <inheritdoc />
    protected override Expression VisitBlock(BlockExpression node)
    {
        var variables = node.Variables;
        var count = variables.Count;
        if (count == 0)
            return Expression.Block(CheckExpressions(node.Expressions));
        var parameters = new Dictionary<ParameterExpression, ParameterExpression>(_parameters);
        var reBuildParameters = new List<ParameterExpression>(count);
        foreach (var variable in variables)
        {
            var reBuildParameter = Expression.Parameter(variable.Type, variable.Name);
            reBuildParameters.Add(reBuildParameter);
            parameters[variable] = reBuildParameter;            
        }            
        var reBuild = new ReBuildVisitor(parameters);
        return Expression.Block(reBuildParameters, reBuild.CheckExpressions(node.Expressions));
    }
    /// <inheritdoc />
    internal protected override Expression VisitCore(Expression node)
    {
        if (node.NodeType == ExpressionType.Lambda && node is LambdaExpression lambda)
            return Expression.Lambda(Visit(lambda.Body), lambda.Name, lambda.TailCall, CheckVariables(lambda.Parameters));
        return base.VisitCore(node);
    }
    /// <summary>
    /// 空参重建
    /// </summary>
    public static readonly ReBuildVisitor Empty = new();
}
