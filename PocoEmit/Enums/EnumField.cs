using System;
using System.Linq.Expressions;

namespace PocoEmit.Enums;

/// <summary>
/// 枚举字段
/// </summary>
public class EnumField(string name, string member, ConstantExpression expression, ConstantExpression under)
    : IEnumField
{    
    #region 配置
    private readonly string _name = name;
    private readonly string _member = member;
    //private readonly TEnum _value = value;
    private readonly ConstantExpression _expression = expression;
    private readonly ConstantExpression _under = under;

    /// <inheritdoc />
    public string Name
        => _name;
    /// <inheritdoc />
    public string Member
        => _member;
    ///// <summary>
    ///// 枚举值
    ///// </summary>
    //public TEnum Value
    //    => _value;
    /// <summary>
    /// 枚举值
    /// </summary>
    public ConstantExpression Expression
        => _expression;
    /// <summary>
    /// 基础值
    /// </summary>
    public ConstantExpression Under 
        => _under;
    #endregion
    /// <inheritdoc />
    public bool Match(string name)
        => string.Equals(name, _name, StringComparison.OrdinalIgnoreCase);
    /// <inheritdoc />
    public bool MatchMember(string member)
        => string.Equals(member, _member, StringComparison.OrdinalIgnoreCase);    
}
