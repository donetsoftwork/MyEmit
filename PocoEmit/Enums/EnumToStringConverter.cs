using PocoEmit.Configuration;
using PocoEmit.Converters;
using System.Linq.Expressions;

namespace PocoEmit.Enums;

/// <summary>
/// 枚举转化为字符串
/// </summary>
/// <param name="fields"></param>
public class EnumToStringConverter(IEnumField[] fields)
     : IEmitConverter
{
    /// <summary>
    /// 枚举转化为字符串
    /// </summary>
    /// <param name="bundle"></param>
    public EnumToStringConverter(IEnumBundle bundle)
        : this([.. bundle.Fields])
    {
    }
    #region 配置
    private readonly IEnumField[] _fields = fields;
    /// <summary>
    /// 字段
    /// </summary>
    public IEnumField[] Fields 
        => _fields;
    /// <inheritdoc />
    bool ICompileInfo.Compiled
        => false;
    #endregion
    /// <inheritdoc />
    public Expression Convert(Expression source)
    {
        var cases = new SwitchCase[_fields.Length];
        var i = 0;
        foreach (var field in _fields)
        {
            var member = field.Member;
            if (string.IsNullOrWhiteSpace(member))
                member = field.Name;
            cases[i++] = Expression.SwitchCase(
                Expression.Constant(member),
                field.Expression
            );
        }
        return Expression.Switch(source,
            Expression.Call(source, SelfMethodConverter.ToStringMethod),
            cases
        );
    }
}
