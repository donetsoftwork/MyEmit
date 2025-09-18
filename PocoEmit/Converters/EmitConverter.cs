using PocoEmit.Builders;
using PocoEmit.Configuration;
using System;
using System.Linq.Expressions;

namespace PocoEmit.Converters;

/// <summary>
/// Emit类型转化
/// </summary>
/// <param name="isPrimitiveSource"></param>
/// <param name="key"></param>
public class EmitConverter(bool isPrimitiveSource, PairTypeKey key)
    : IEmitConverter
{
    /// <summary>
    /// Emit类型转化
    /// </summary>
    /// <param name="key"></param>
    public EmitConverter(PairTypeKey key)
        : this(true, key)
    {
    }
    #region 配置
    /// <summary>
    /// 源类型是否为基础类型
    /// </summary>
    protected bool _isPrimitiveSource = isPrimitiveSource;
    private PairTypeKey _key = key;
    /// <summary>
    /// 映射目标类型
    /// </summary>
    protected readonly Type _destType = key.RightType;
    /// <summary>
    /// 源类型是否为基础类型
    /// </summary>
    public bool IsPrimitiveSource
        => _isPrimitiveSource;
    /// <inheritdoc />
    public PairTypeKey Key
        => _key;
    /// <inheritdoc />
    bool ICompileInfo.Compiled
        => false;
    #endregion
    /// <inheritdoc />
    public virtual Expression Convert(Expression value)
        => Convert(value, _isPrimitiveSource, _destType, ConvertCore);
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
    /// <param name="isPrimitive"></param>
    /// <param name="destType"></param>
    /// <param name="converter"></param>
    /// <returns></returns>
    public static Expression Convert(Expression value, bool isPrimitive, Type destType, Func<Expression, Type, Expression> converter)
    {
        var sourceType = value.Type;
        if (PairTypeKey.CheckNullCondition(sourceType))
        {
            if (EmitHelper.CheckComplexSource(value, isPrimitive))
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
