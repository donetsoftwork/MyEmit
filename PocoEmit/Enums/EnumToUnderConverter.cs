using PocoEmit.Configuration;
using PocoEmit.Converters;
using System;
using System.Linq.Expressions;

namespace PocoEmit.Enums;

/// <summary>
/// 枚举转化为基础类型
/// </summary>
/// <param name="bundle"></param>
public class EnumToUnderConverter(IEnumBundle bundle)
     : IEmitConverter
{
    #region 配置
    private readonly IEnumBundle _bundle = bundle;
    private readonly Type _underType = bundle.UnderType;
    private readonly IEnumField[] _fields = [.. bundle.Fields];

    /// <summary>
    /// 枚举配置
    /// </summary>
    public IEnumBundle Bundle
    => _bundle;
    /// <inheritdoc />
    bool ICompileInfo.Compiled
        => false;

    #endregion
    /// <inheritdoc />
    public virtual Expression Convert(Expression source)
    {
        if(_bundle.HasFlag)
            return FromFlag(source);
        return FromEnum(source);
    }
    /// <summary>
    /// 通过位域枚举转化
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    public Expression FromFlag(Expression source)
        => Expression.Convert(source, _underType);
    /// <summary>
    /// 通过枚举转化
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    public Expression FromEnum(Expression source)
    {
        var cases = new SwitchCase[_fields.Length];
        var i = 0;
        foreach (var field in _fields)
        {
            cases[i++] = Expression.SwitchCase(                
                field.Under,
                field.Expression
            );
        }
        return Expression.Switch(source,
            Expression.Default(_underType),
            cases
        );
    }
}
