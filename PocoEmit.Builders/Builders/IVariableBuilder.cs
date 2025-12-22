using System.Linq.Expressions;

namespace PocoEmit.Builders;

/// <summary>
/// 变量构造器
/// </summary>
public interface IVariableBuilder
    : IEmitBuilder
{
    /// <summary>
    /// 当前变量
    /// </summary>
    ParameterExpression Current { get; }
    /// <summary>
    /// 更换变量
    /// </summary>
    /// <param name="variable"></param>
    void Change(ParameterExpression variable);
}
