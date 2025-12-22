using Hand.Reflection;
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
public class EmitConverter(bool isPrimitiveSource, in PairTypeKey key)
    : IEmitConverter
    , IArgumentExecuter
{
    /// <summary>
    /// Emit类型转化
    /// </summary>
    /// <param name="key"></param>
    public EmitConverter(in PairTypeKey key)
        : this(true, key)
    {
    }
    #region 配置
    /// <summary>
    /// 源类型是否为基础类型
    /// </summary>
    protected bool _isPrimitiveSource = isPrimitiveSource;
    private readonly PairTypeKey _key = key;
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
    public Expression Convert(Expression source)
    {
        var builder = new EmitBuilder();
        builder.Add(Execute(builder, source));
        return builder.Create();
    }
    /// <summary>
    /// 核心转化
    /// </summary>
    /// <param name="source"></param>
    /// <param name="destType"></param>
    /// <returns></returns>
    protected virtual Expression ConvertCore(Expression source, Type destType)
        => Expression.Convert(source, destType);
    #region IArgumentExecuter
    /// <inheritdoc />
    public virtual Expression Execute(IEmitBuilder builder, Expression argument)
    {
        var sourceType = argument.Type;
        if (PairTypeKey.CheckNullCondition(sourceType))
        {
            var source = argument;
            if (EmitHelper.CheckComplexSource(argument, _isPrimitiveSource))
                source = builder.Temp(sourceType, argument);

            return EmitHelper.IfDefault(
                source,
                Expression.Default(_destType),
                ConvertCore(source, _destType),
                _destType
                );
        }
        else
        {
            return ConvertCore(argument, _destType);
        }
    }
    #endregion
}
