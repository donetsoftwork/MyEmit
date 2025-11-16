using Hand.Creational;
using Hand.Structural;
using PocoEmit.Builders;
using System.Linq.Expressions;

namespace PocoEmit.Converters;

/// <summary>
/// 已编译转化
/// </summary>
public interface ICompiledConverter
    : IEmitConverter
    , ICreator<LambdaExpression>
    , IWrapper<IEmitConverter>
    , IDelegateCompile
{
}
