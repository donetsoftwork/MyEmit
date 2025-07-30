using PocoEmit.Configuration;
using System;
using System.Linq.Expressions;

namespace PocoEmit.Converters;

/// <summary>
/// Emit类型转化
/// </summary>
/// <param name="destType"></param>
public sealed class EmitConverter(Type destType)
    : IEmitConverter
{
    #region 配置
    private readonly Type _destType = destType;
    /// <summary>
    /// 映射目标类型
    /// </summary>
    public Type DestType
        => _destType;
    /// <inheritdoc />
    bool IEmitInfo.Compiled
        => false;
    #endregion
    /// <inheritdoc />
    public Expression Convert(Expression value)
        => Expression.Convert(value, _destType);
}
