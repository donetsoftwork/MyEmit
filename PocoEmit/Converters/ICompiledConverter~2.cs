using PocoEmit.Builders;
using System.Linq.Expressions;

namespace PocoEmit.Converters;

/// <summary>
/// 已编译转化
/// </summary>
public interface ICompiledConverter
    : IEmitConverter
    , IBuilder<LambdaExpression>
    , IWrapper<IEmitConverter>
    , IDelegateCompile
{
}
