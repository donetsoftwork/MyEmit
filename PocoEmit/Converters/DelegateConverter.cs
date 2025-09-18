using PocoEmit.Builders;
using PocoEmit.Configuration;
using System.Linq.Expressions;

namespace PocoEmit.Converters;

/// <summary>
/// 委托表达式类型转化
/// </summary>
/// <param name="poco"></param>
/// <param name="key"></param>
/// <param name="convertFunc"></param>
public class FuncConverter(IPocoOptions poco, PairTypeKey key, LambdaExpression convertFunc)
    : ArgumentFuncCallBuilder(poco, key, convertFunc)
    , IEmitConverter
{
    #region 配置
    /// <inheritdoc />
    bool ICompileInfo.Compiled
        => false;
    #endregion
    /// <inheritdoc />
    public Expression Convert(Expression source)
        => Call(source);
}
