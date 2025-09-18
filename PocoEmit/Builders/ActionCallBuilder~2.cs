using PocoEmit.Configuration;
using System.Linq.Expressions;

namespace PocoEmit.Builders;

/// <summary>
/// 执行Action表达式
/// </summary>
/// <param name="poco"></param>
/// <param name="action"></param>
public class ActionCallBuilder(IPocoOptions poco, LambdaExpression action)
{
    #region 配置
    private readonly IPocoOptions _poco = poco;
    private readonly LambdaExpression _action = action;
    /// <summary>
    /// 对象处理
    /// </summary>
    public IPocoOptions Poco
        => _poco;
    /// <summary>
    /// Action表达式
    /// </summary>
    public LambdaExpression Action
        => _action;
    #endregion
    /// <summary>
    /// 通过替换参数来调用Action表达式
    /// </summary>
    /// <param name="argument1"></param>
    /// <param name="argument2"></param>
    /// <returns></returns>
    public Expression Call(Expression argument1, Expression argument2)
        => _poco.Call(_action, argument1, argument2);
}
