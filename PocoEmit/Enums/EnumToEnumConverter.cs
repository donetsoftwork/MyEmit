using PocoEmit.Builders;
using PocoEmit.Configuration;
using PocoEmit.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace PocoEmit.Enums;

/// <summary>
/// 枚举转化为枚举
/// </summary>
/// <param name="sourceBundle"></param>
/// <param name="destBundle"></param>
public class EnumToEnumConverter(IEnumBundle sourceBundle, IEnumBundle destBundle)
    : IEmitConverter
{
    #region 配置
    private readonly Type _destEnumType = destBundle.EnumType;
    private readonly IEnumBundle _sourceBundle = sourceBundle;
    private readonly IEnumBundle _destBundle = destBundle;
    /// <summary>
    /// 源枚举转化为基础类型
    /// </summary>
    private readonly EnumToUnderConverter _sourceToUnderConverter = new(sourceBundle);
    /// <summary>
    /// 基础类型转化为目标枚举
    /// </summary>
    private readonly EnumFromUnderConverter _destFromUnderConverter = new(destBundle);
    /// <summary>
    /// 目标枚举类型
    /// </summary>
    public Type DestEnumType
        => _destEnumType;
    /// <inheritdoc />
    bool ICompileInfo.Compiled
        => false;
    #endregion
    /// <inheritdoc />
    public Expression Convert(Expression source)
    {
        if(_sourceBundle.HasFlag && _sourceBundle is FlagEnumBundle sourceFlagBundle)
        {
            if(_destBundle.HasFlag && _destBundle is FlagEnumBundle destFlagBundle)
                return FlagToFlag(source, sourceFlagBundle.Fields, destFlagBundle);
            return FlagToEnum(source, sourceFlagBundle.Fields);
        }
        //if (_destBundle.HasFlag && _destBundle is FlagEnumBundle destFlagBundle)
        //    return EnumToFlag(source, destFlagBundle);
        return EnumToEnum(source, [.. _sourceBundle.Fields]);
    }
    /// <summary>
    /// 枚举转化为枚举
    /// </summary>
    /// <param name="source"></param>
    /// <param name="sourceFields"></param>
    /// <returns></returns>
    public Expression EnumToEnum(Expression source, IEnumField[] sourceFields)
    {
        var cases = new List<SwitchCase>(sourceFields.Length);
        foreach (var sourceField in sourceFields)
        {
            var destField = Map(sourceField, _destBundle);
            if (destField == null)
                continue;
            cases.Add(Expression.SwitchCase(
                destField.Expression,
                sourceField.Expression
            ));
        }
        return Expression.Switch(source,
            Expression.Default(_destEnumType),
            [.. cases]
        );
    }
    #region FlagToEnum
    /// <summary>
    /// 位域转化为枚举
    /// </summary>
    /// <param name="source"></param>
    /// <param name="sourceFields"></param>
    /// <returns></returns>
    public Expression FlagToEnum(Expression source, List<FlagEnumField> sourceFields)
    {
        var sourceUnderType = _sourceBundle.UnderType;
        var sourceUnder = Expression.Parameter(sourceUnderType, "sourceUnder");
        var dest = Expression.Parameter(_destEnumType, "dest");
        var conditions = CreateFlagToEnumConditions(sourceUnder, dest, sourceFields, _destBundle);
        var condition = EmitHelper.BuildConditions(conditions);
        return Expression.Block([sourceUnder, dest],
            Expression.Assign(sourceUnder, _sourceToUnderConverter.FromEnum(source)),
            Expression.Assign(dest, Expression.Default(_destEnumType)),
            condition,
            dest
        );
    }
    /// <summary>
    /// 构造条件分支
    /// </summary>
    /// <returns></returns>
    public static List<KeyValuePair<Expression, Expression>> CreateFlagToEnumConditions(Expression sourceUnder, Expression dest, List<FlagEnumField> sourceFields, IEnumBundle destBundle)
    {
        var conditions = new List<KeyValuePair<Expression, Expression>>(sourceFields.Count);
        foreach (var sourceField in sourceFields)
        {
            var under = sourceField.Under;
            if (System.Convert.ToUInt64(under.Value) == 0UL)
                continue;
            var destField = Map(sourceField, destBundle);
            if (destField == null)
                continue;
            conditions.Add(new KeyValuePair<Expression, Expression>(
                CreateFlagCondition(sourceUnder, under),
                Expression.Assign(dest, destField.Expression)
            ));
        }
        return conditions;
    }
    #endregion
    /// <summary>
    /// 构造位域判断条件
    /// </summary>
    /// <param name="source"></param>
    /// <param name="under"></param>
    /// <returns></returns>
    public static Expression CreateFlagCondition(Expression source, Expression under)
        => Expression.Equal(under, Expression.And(source, under));
    ///// <summary>
    ///// 枚举转化为位域
    ///// </summary>
    ///// <param name="source"></param>
    ///// <param name="destFlagBundle"></param>
    ///// <returns></returns>
    //public Expression EnumToFlag(Expression source, FlagEnumBundle destFlagBundle)
    //{
    //    return Convert(source);
    //}
    /// <summary>
    /// 位域转化为位域
    /// </summary>
    /// <param name="source"></param>
    /// <param name="sourceFields"></param>
    /// <param name="destFlagBundle"></param>
    /// <returns></returns>
    public Expression FlagToFlag(Expression source, List<FlagEnumField> sourceFields, FlagEnumBundle destFlagBundle)
    {
        var sourceUnderType = _sourceBundle.UnderType;
        var destUnderType = _destBundle.UnderType;
        var sourceUnder = Expression.Parameter(sourceUnderType, "sourceUnder");
        var destUnder = Expression.Parameter(destUnderType, "destUnder");
        var conditions = new List<Expression>(sourceFields.Count + 3)
        {
            Expression.Assign(sourceUnder, _sourceToUnderConverter.FromFlag(source)),
            Expression.Assign(destUnder, Expression.Default(destUnderType))
        };
        CheckFlagToFlagConditions(conditions, sourceUnder, destUnder, sourceFields, destFlagBundle);
        conditions.Add(_destFromUnderConverter.ToFlag(destUnder));

        return Expression.Block([sourceUnder, destUnder],
            conditions
        );
    }
    /// <summary>
    /// 构造条件分支
    /// </summary>
    /// <returns></returns>
    public static void CheckFlagToFlagConditions(ICollection<Expression> conditions, Expression sourceUnder, Expression destUnder, List<FlagEnumField> sourceFields, FlagEnumBundle destBundle)
    {
        foreach (var sourceField in sourceFields)
        {
            var sourceFieldUnder = sourceField.Under;
            if (System.Convert.ToUInt64(sourceFieldUnder.Value) == 0UL)
                continue;
            var destFields = MapFlag(sourceField, destBundle);
            if (destFields.Length == 0)
                continue;
            var flag = destFields.Aggregate(0UL, (a, b) => a | b.Flag);
            if(flag == 0UL)
                continue;
            var underType = destBundle.UnderType;
            var destFieldUnder = Expression.Constant(System.Convert.ChangeType(flag, underType));
            conditions.Add(Expression.IfThen(
                CreateFlagCondition(sourceUnder, sourceFieldUnder),
                Expression.OrAssign(destUnder, destFieldUnder)
            ));
        }
    }
    /// <summary>
    /// 按源字段映射到目标字段(优先Member)
    /// </summary>
    /// <param name="sourceField"></param>
    /// <param name="destBundle"></param>
    /// <returns></returns>
    private static IEnumField Map(IEnumField sourceField, IEnumBundle destBundle)
        => destBundle.GetFieldByName(sourceField.Member) ?? destBundle.GetFieldByName(sourceField.Name);
    /// <summary>
    /// 按源字段映射到位域字段
    /// </summary>
    /// <param name="sourceField"></param>
    /// <param name="destBundle"></param>
    /// <returns></returns>
    private static FlagEnumField[] MapFlag(IEnumField sourceField, FlagEnumBundle destBundle)
        => [.. destBundle.GetFieldsByName(sourceField.Name, sourceField.Member)];
}
