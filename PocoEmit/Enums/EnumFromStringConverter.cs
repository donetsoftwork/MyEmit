using PocoEmit.Builders;
using PocoEmit.Configuration;
using PocoEmit.Converters;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace PocoEmit.Enums;

/// <summary>
/// 字符串转化为枚举
/// </summary>
/// <param name="enumType"></param>
/// <param name="fields"></param>
public class EnumFromStringConverter(Type enumType, IEnumField[] fields)
     : IEmitConverter
{
    /// <summary>
    /// 枚举转化为字符串
    /// </summary>
    /// <param name="bundle"></param>
    public EnumFromStringConverter(IEnumBundle bundle)
        : this(bundle.EnumType, [.. bundle.Fields])
    {
    }
    #region 配置
    private readonly Type _enumType = enumType;
    private readonly IEnumField[] _fields = fields;

    /// <summary>
    /// 枚举类型
    /// </summary>
    public Type EnumType
        => _enumType;
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
        var conditions = CreateConditions(source, _fields);
        return EmitHelper.BuildConditions(_enumType, conditions);
    }    
    #region CreateConditions
    /// <summary>
    /// 构造条件分支
    /// </summary>
    /// <returns></returns>
    public static List<KeyValuePair<Expression, Expression>> CreateConditions(Expression source, IEnumField[] fields)
    {
        var conditions = new List<KeyValuePair<Expression, Expression>>(fields.Length);
        var memberCheck = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        foreach (var field in fields)
        {
            var member = field.Member;
            if (string.IsNullOrWhiteSpace(member) || memberCheck.Contains(member))
                continue;
            conditions.Add(new KeyValuePair<Expression, Expression>(
                CreateCondition(source, member),
                field.Expression
            ));
            memberCheck.Add(member);
        }
        foreach (var field in fields)
        {
            var member = field.Name;
            if (memberCheck.Contains(member))
                continue;
            conditions.Add(new KeyValuePair<Expression, Expression>(
                CreateCondition(source, member),
                field.Expression
            ));
            memberCheck.Add(member);
        }
        return conditions;
    }
    /// <summary>
    /// 构造条件
    /// </summary>
    /// <param name="source"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    public static Expression CreateCondition(Expression source, string name)
        => Expression.Call(_comparer, _comparisonMethod, source, Expression.Constant(name));
    #endregion
    /// <summary>
    /// 枚举判断相同
    /// </summary>
    private static readonly Expression _comparer = Expression.Constant(StringComparer.OrdinalIgnoreCase);
    /// <summary>
    /// 枚举判断相同方法
    /// </summary>
    private static readonly MethodInfo _comparisonMethod = EmitHelper.GetActionMethodInfo<string, string>((x, y) => StringComparer.OrdinalIgnoreCase.Equals(x, y));
}
