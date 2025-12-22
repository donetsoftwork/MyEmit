using Hand.Reflection;
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
    , IArgumentExecuter
{
    #region 配置
    private readonly PairTypeKey _key = new(sourceBundle.EnumType, destBundle.EnumType);
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
    /// <inheritdoc />
    public PairTypeKey Key
        => _key;
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
        var builder = new EmitBuilder();
        builder.Add(Execute(builder, source));
        return builder.Create();
    }
    #region IArgumentExecuter
    /// <inheritdoc />
    public Expression Execute(IEmitBuilder builder, Expression argument)
    {
        if (_sourceBundle.HasFlag && _sourceBundle is FlagEnumBundle sourceFlagBundle)
        {
            if (_destBundle.HasFlag && _destBundle is FlagEnumBundle destFlagBundle)
                return FlagToFlag(builder, argument, sourceFlagBundle.Fields, destFlagBundle);
            return FlagToEnum(builder, argument, sourceFlagBundle.Fields);
        }
        //if (_destBundle.HasFlag && _destBundle is FlagEnumBundle destFlagBundle)
        //    return EnumToFlag(source, destFlagBundle);
        return EnumToEnum(argument, [.. _sourceBundle.Fields]);
    }
    #endregion
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
            // case Enum1.First: return Enum2.First;
            cases.Add(Expression.SwitchCase(
                destField.Expression,
                sourceField.Expression
            ));
        }
        return Expression.Switch(
            _destEnumType,
            source,
            Expression.Default(_destEnumType),
            null,
            cases
        );
    }
    #region FlagToEnum
    /// <summary>
    /// 位域转化为枚举
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="source"></param>
    /// <param name="sourceFields"></param>
    /// <returns></returns>
    public Expression FlagToEnum(IEmitBuilder builder, Expression source, List<FlagEnumField> sourceFields)
    {
        // int value = (int)enum;
        var sourceUnder = builder.Temp(_sourceBundle.UnderType, builder.Execute(_sourceToUnderConverter, source));
        var switchBuilder = new SwitchBuilder(Expression.Constant(true), _destEnumType);
        foreach (var sourceField in sourceFields)
        {
            var under = sourceField.Under;
            if (System.Convert.ToUInt64(under.Value) == 0UL)
                continue;
            var destField = Map(sourceField, _destBundle);
            if (destField == null)
                continue;
            // case (value & Enum.First == Enum.First) :
            //   return 1;
            switchBuilder.Case(destField.Expression, CreateFlagCondition(sourceUnder, under));
        }
        return switchBuilder.Build(Expression.Default(_destEnumType));
    }
    #endregion
    /// <summary>
    /// 构造位域判断条件
    /// </summary>
    /// <param name="source"></param>
    /// <param name="under"></param>
    /// <returns></returns>
    private static BinaryExpression CreateFlagCondition(Expression source, Expression under)
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
    /// <param name="builder"></param>
    /// <param name="source"></param>
    /// <param name="sourceFields"></param>
    /// <param name="destFlagBundle"></param>
    /// <returns></returns>
    public Expression FlagToFlag(IEmitBuilder builder, Expression source, List<FlagEnumField> sourceFields, FlagEnumBundle destFlagBundle)
    {
        var sourceUnderType = _sourceBundle.UnderType;
        var destUnderType = _destBundle.UnderType;
        // sourceUnder和destUnder很可能类型一下,不能用Temp
        // int sourceUnder = (int)source;
        var sourceUnder = builder.Declare(sourceUnderType, "sourceUnder");
        builder.Assign(sourceUnder, _sourceToUnderConverter.Convert(source));
        // int destUnder = default;
        var destUnder = builder.Declare(destUnderType, "destUnder");
        builder.Assign(destUnder, Expression.Default(destUnderType));
        //var switchBuilder = new SwitchBuilder(Expression.Constant(true), _destEnumType);
        foreach (var sourceField in sourceFields)
        {
            var sourceFieldUnder = sourceField.Under;
            if (System.Convert.ToUInt64(sourceFieldUnder.Value) == 0UL)
                continue;
            var destFields = MapFlag(sourceField, destFlagBundle);
            if (destFields.Length == 0)
                continue;
            var flag = destFields.Aggregate(0UL, (a, b) => a | b.Flag);
            if (flag == 0UL)
                continue;
            var destFieldUnder = Expression.Constant(System.Convert.ChangeType(flag, destUnderType));
            // if (value & SourceEnum.First == SourceEnum.First) 
            //   destUnder = destUnder | DestEnum.First;
            builder.IfThen(CreateFlagCondition(sourceUnder, sourceFieldUnder), Expression.OrAssign(destUnder, destFieldUnder));
        }
        // return (DestEnum)destUnder;
        return _destFromUnderConverter.ToFlag(destUnder);
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
