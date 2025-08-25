using System;
using System.Linq.Expressions;

namespace PocoEmit.Builders;

/// <summary>
/// 常量表达式构建器
/// </summary>
/// <param name="constant"></param>
public class ConstantBuilder(ConstantExpression constant)
    : IEmitBuilder
{
    #region 配置
    private readonly ConstantExpression _constant = constant;
    /// <summary>
    /// 常量表达式
    /// </summary>
    public ConstantExpression Constant
        => _constant;
    #endregion
    /// <inheritdoc />
    public Expression Build()
        => _constant;
    /// <summary>
    /// 使用常量值
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static ConstantBuilder Use(object value)
        => new(Expression.Constant(value));
    /// <summary>
    /// 使用常量值
    /// </summary>
    /// <param name="value"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    public static ConstantBuilder Use(object value, Type type)
        => new(Expression.Constant(value, type));
}
