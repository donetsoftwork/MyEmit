using PocoEmit.Builders;
using PocoEmit.Configuration;
using System;
using System.Linq.Expressions;
#if (NETSTANDARD1_1 || NETSTANDARD1_3 || NETSTANDARD1_6)
using System.Reflection;
#endif

namespace PocoEmit.Converters;

/// <summary>
/// Emit类型转化
/// </summary>
/// <param name="destType"></param>
public class EmitConverter(Type destType)
    : IEmitConverter
{
    #region 配置
    /// <summary>
    /// 映射目标类型
    /// </summary>
    protected readonly Type _destType = destType;
    /// <summary>
    /// 映射目标类型
    /// </summary>
    public Type DestType
        => _destType;
    /// <inheritdoc />
    bool ICompileInfo.Compiled
        => false;
    #endregion
    /// <inheritdoc />
    public virtual Expression Convert(Expression value)
        => Convert(value, _destType, ConvertCore);
    /// <summary>
    /// 核心转化
    /// </summary>
    /// <param name="value"></param>
    /// <param name="destType"></param>
    /// <returns></returns>
    protected virtual Expression ConvertCore(Expression value, Type destType)
        => Expression.Convert(value, destType);
    /// <summary>
    /// 转化类型
    /// </summary>
    /// <param name="value"></param>
    /// <param name="destType"></param>
    /// <param name="converter"></param>
    /// <returns></returns>
    public static Expression Convert(Expression value, Type destType, Func<Expression, Type, Expression> converter)
    {
        var sourceType = value.Type;
        if (PairTypeKey.CheckNullCondition(sourceType))
        {
            if (EmitHelper.CheckComplex(value.NodeType))
            {
                var source = Expression.Variable(sourceType, "source");
                return Expression.Block(
                    [source],
                    Expression.Assign(source, value),
                    Expression.Condition(
                        Expression.Equal(source, Expression.Constant(null, sourceType)),
                        Expression.Default(destType),
                        converter(source, destType)
                        )
                    );
            }
            else
            {
                return Expression.Condition(
                    Expression.Equal(value, Expression.Constant(null, sourceType)),
                    Expression.Default(destType),
                    converter(value, destType)
                    );
            }
        }
        else
        {
            return converter(value, destType);
        }
    }
}
