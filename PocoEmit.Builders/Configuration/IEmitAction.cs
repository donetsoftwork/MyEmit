using Hand.Creational;
using System.Linq.Expressions;

namespace PocoEmit.Configuration;

/// <summary>
/// Action委托构建器
/// </summary>
public interface IEmitAction
    : ICreator<LambdaExpression>, ICompileInfo
{
}
