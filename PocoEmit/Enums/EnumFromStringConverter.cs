using Hand.Reflection;
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
    private readonly PairTypeKey _key = new(typeof(string), enumType);
    private readonly Type _enumType = enumType;
    private readonly IEnumField[] _fields = fields;
    /// <inheritdoc />
    public PairTypeKey Key
        => _key;
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
        var builder = new SwitchBuilder(Expression.Constant(true), _enumType);
        var memberCheck = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        foreach (var field in _fields)
        {
            var member = field.Member;
            if (string.IsNullOrWhiteSpace(member) || memberCheck.Contains(member))
                continue;
            // case StringComparer.OrdinalIgnoreCase.Equals(source, "First") :
            //   return Enum.First;
            builder.Case(field.Expression, CreateCondition(source, member));
            memberCheck.Add(member);
        }
        foreach (var field in _fields)
        {
            var member = field.Name;
            if (memberCheck.Contains(member))
                continue;
            // case StringComparer.OrdinalIgnoreCase.Equals(source, "One") :
            //   return Enum.First;
            builder.Case(field.Expression, CreateCondition(source, member));
            memberCheck.Add(member);
        }
        return builder.Build(Expression.Default(_enumType));
    }
    /// <summary>
    /// 构造条件
    /// </summary>
    /// <param name="source"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    public static Expression CreateCondition(Expression source, string name)
        => Expression.Call(_comparer, _comparisonMethod, source, Expression.Constant(name));
    /// <summary>
    /// 枚举判断相同
    /// </summary>
    private static readonly Expression _comparer = Expression.Constant(StringComparer.OrdinalIgnoreCase);
    /// <summary>
    /// 枚举判断相同方法
    /// </summary>
    private static readonly MethodInfo _comparisonMethod = EmitHelper.GetActionMethodInfo<string, string>((x, y) => StringComparer.OrdinalIgnoreCase.Equals(x, y));
}
