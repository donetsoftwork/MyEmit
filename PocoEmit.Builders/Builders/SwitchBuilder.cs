using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace PocoEmit.Builders;

/// <summary>
/// 构建Switch
/// </summary>
public class SwitchBuilder(Expression value, Type type = null, MethodInfo comparison = null)
{
    private readonly Expression _value = value;
    private readonly Type _type = type;
    private readonly MethodInfo _comparison = comparison;
    private readonly List<SwitchCase> _cases = [];

    /// <summary>
    /// 增加分支
    /// </summary>
    /// <param name="body"></param>
    /// <param name="testValues"></param>
    public SwitchBuilder Case(Expression body, params IEnumerable<Expression> testValues)
    {
        _cases.Add(Expression.SwitchCase(body, testValues));
        return this;
    }
    /// <summary>
    /// 增加分支
    /// </summary>
    /// <param name="case"></param>
    /// <returns></returns>
    public SwitchBuilder Case(SwitchCase @case)
    {
        _cases.Add(@case);
        return this;
    }
    /// <summary>
    /// 组装
    /// </summary>
    /// <param name="defaultBody"></param>
    /// <returns></returns>
    public SwitchExpression Build(Expression defaultBody = null)
        => Expression.Switch(_type, _value, defaultBody, _comparison, _cases);
}
