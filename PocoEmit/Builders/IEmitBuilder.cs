using System.Linq.Expressions;

namespace PocoEmit.Builders;

/// <summary>
/// 表达式构建器
/// </summary>
public interface IEmitBuilder
{
    /// <summary>
    /// 构建
    /// </summary>
    Expression Build();
}
