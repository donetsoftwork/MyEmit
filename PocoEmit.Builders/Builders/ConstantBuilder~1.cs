using Hand.Creational;
using System.Linq.Expressions;

namespace PocoEmit.Builders;

/// <summary>
/// 常量表达式构造器
/// </summary>
/// <typeparam name="TValue"></typeparam>
/// <param name="value"></param>
/// <param name="constant"></param>
public class ConstantBuilder<TValue>(TValue value, ConstantExpression constant)
    : ConstantBuilder(constant)
    , ICreator<TValue>
{
    /// <summary>
    /// 常量表达式构造器
    /// </summary>
    /// <param name="value"></param>
    public ConstantBuilder(TValue value)
        : this(value, Expression.Constant(value, typeof(TValue)))
    {
    }
    #region 配置
    private readonly TValue _value = value;
    /// <summary>
    /// 常量值
    /// </summary>
    public TValue Value
        => _value;
    #endregion
    #region ICreator<TValue>
    /// <inheritdoc />
    TValue ICreator<TValue>.Create()
        => _value;
    #endregion
}
