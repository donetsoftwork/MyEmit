using System.Collections.Generic;
using System.Linq.Expressions;

namespace PocoEmit.Enums;

/// <summary>
/// 位域枚举字段
/// </summary>
/// <param name="name"></param>
/// <param name="member"></param>
/// <param name="expression"></param>
/// <param name="under"></param>
/// <param name="flag"></param>
public class FlagEnumField(string name, string member, ConstantExpression expression, ConstantExpression under, ulong flag)
    : EnumField(name, member, expression, under)
{
    #region 配置
    private readonly ulong _flag = flag;
    /// <summary>
    /// 位域值
    /// </summary>
    public ulong Flag
        => _flag;
    #endregion
    /// <summary>
    /// 按位域获取字段
    /// </summary>
    /// <param name="fields"></param>
    /// <param name="flag"></param>
    /// <returns></returns>
    public static IEnumerable<FlagEnumField> GetFieldsByFlag(IEnumerable<FlagEnumField> fields, ulong flag)
    {
        foreach (var field in fields)
        {
            var value = field._flag;
            if ((value & flag) == value)
                yield return field;
        }
    }
}
