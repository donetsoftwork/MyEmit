using PocoEmit.Builders;
using PocoEmit.Configuration;
using PocoEmit.Converters;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace PocoEmit.Enums;

/// <summary>
/// 枚举转化为基础类型
/// </summary>
/// <param name="bundle"></param>
public class EnumFromUnderConverter(IEnumBundle bundle)
     : IEmitConverter
{
    #region 配置
    private readonly IEnumBundle _bundle = bundle;
    private readonly Type _enumType = bundle.EnumType;
    private readonly Type _underType = bundle.UnderType;
    private readonly IEnumField[] _fields = [.. bundle.Fields];
    /// <summary>
    /// ToObject方法
    /// </summary>    
    private readonly MethodInfo _toObjectMethod = bundle.HasFlag ? ReflectionHelper.GetMethod(typeof(Enum), nameof(Enum.ToObject), [typeof(Type), bundle.UnderType]) : null;
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
            return ToFlag(source);
        return ToEnum(source);
    }

    /// <summary>
    /// 通过位域枚举转化
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    public Expression ToFlag(Expression source)
    {
        return Expression.Convert(
            Expression.Call(_toObjectMethod,
                Expression.Constant(_enumType, typeof(Type)),
                source
            ),
            _enumType
        );
    }
    /// <summary>
    /// 通过枚举转化
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    public Expression ToEnum(Expression source)
    {
        var cases = new SwitchCase[_fields.Length];
        var i = 0;
        foreach (var field in _fields)
        {
            cases[i++] = Expression.SwitchCase(
                field.Expression,
                field.Under
            );
        }
        return Expression.Switch(source,
            Expression.Default(_enumType),
            cases
        );
    }
    #region CreateConditions
    /// <summary>
    /// 构造条件分支
    /// </summary>
    /// <returns></returns>
    public static List<KeyValuePair<Expression, Expression>> CreateConditions(Expression source, Expression dest, IEnumField[] destFields)
    {
        var conditions = new List<KeyValuePair<Expression, Expression>>(destFields.Length);
        foreach (var field in destFields)
        {
            var under = field.Under;
            if (System.Convert.ToUInt64(under.Value) == 0UL)
                continue;
            conditions.Add(new KeyValuePair<Expression, Expression>(
                CreateFlagCondition(source, under),
                Expression.OrAssign(dest, under)
            ));
        }
        return conditions;
    }
    /// <summary>
    /// 构造位域判断条件
    /// </summary>
    /// <param name="source"></param>
    /// <param name="under"></param>
    /// <returns></returns>
    public static Expression CreateFlagCondition(Expression source, Expression under)
        => Expression.Equal(under, Expression.And(source, under));
    #endregion
}
