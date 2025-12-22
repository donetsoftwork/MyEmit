using System.Collections.Generic;
using System.Linq.Expressions;

namespace PocoEmit.Builders;

/// <summary>
/// 表达式构造器基类
/// </summary>
/// <param name="expressions"></param>
public abstract class EmitBuilderBase(List<Expression> expressions)
{
    #region 配置
    /// <summary>
    /// 表达式
    /// </summary>
    protected readonly List<Expression> _expressions = expressions;
    /// <summary>
    /// 表达式
    /// </summary>
    public IEnumerable<Expression> Expressions
        => _expressions;
    /// <summary>
    /// 表达式数量
    /// </summary>
    public int Count
        => _expressions.Count;
    #endregion
    #region 功能
    /// <summary>
    /// 增加表达式
    /// </summary>
    /// <param name="expression">表达式</param>
    public void Add(Expression expression)
    {
        if (expression is null)
            return;
        _expressions.Add(expression);
    }
    #endregion
}
