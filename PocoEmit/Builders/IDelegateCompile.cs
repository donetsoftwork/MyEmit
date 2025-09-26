using System.Linq.Expressions;

namespace PocoEmit.Builders;

/// <summary>
/// 委托编译
/// </summary>
public interface IDelegateCompile
{
    /// <summary>
    /// 编译
    /// </summary>
    /// <param name="lambda"></param>
    bool CompileDelegate(LambdaExpression lambda);
}
