//using System;
//using System.Collections.Generic;
//using System.Linq.Expressions;

//namespace PocoEmit.Builders;

///// <summary>
///// 表达式构建器
///// </summary>
///// <param name="variables"></param>
///// <param name="expressions"></param>
//public class ExpressionBuilder(IEnumerable<ParameterExpression> variables, IEnumerable<Expression> expressions)
//    : BlockExpression
//{
//    #region 配置
//    private readonly IEnumerable<ParameterExpression> _variables = variables;
//    private readonly IEnumerable<Expression> _expressions = expressions;
//    /// <summary>
//    /// 变量
//    /// </summary>
//    public IEnumerable<ParameterExpression> Variables 
//        => _variables;
//    /// <summary>
//    /// 表达式
//    /// </summary>
//    public IEnumerable<Expression> Expressions 
//        => _expressions;
//    /// <summary>
//    /// 缓存
//    /// </summary>
//    private Lazy<BlockExpression> _cached = new(() => Block(variables, expressions));
//    #endregion

//    protected override Expression Accept(ExpressionVisitor visitor)
//    {
//        return _cached.Value.Accept(visitor);
//    }
//}
