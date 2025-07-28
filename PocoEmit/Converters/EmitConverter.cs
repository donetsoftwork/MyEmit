using System;
using System.Linq.Expressions;

namespace PocoEmit.Converters;

/// <summary>
/// Emit类型转化
/// </summary>
/// <param name="destType"></param>
public class EmitConverter(Type destType)
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
    public virtual bool Compiled
        => false;
    #endregion
    /// <inheritdoc />
    public virtual Expression Convert(Expression value)
        => Expression.Convert(value, _destType);
}
