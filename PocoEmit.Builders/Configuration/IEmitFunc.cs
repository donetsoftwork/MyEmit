using Hand.Creational;
using System.Linq.Expressions;

namespace PocoEmit.Configuration;

/// <summary>
/// Func委托构建器
/// </summary>
public interface IEmitFunc
    : ICreator<LambdaExpression>, ICompileInfo
{
}
