using PocoEmit.Collections;
using PocoEmit.Configuration;
using PocoEmit.Converters;
using System;
#if (NETSTANDARD1_1 || NETSTANDARD1_3 || NETSTANDARD1_6)
using System.Reflection;
#endif

namespace PocoEmit.Enums;

/// <summary>
/// 枚举类型转化
/// </summary>
/// <param name="poco"></param>
public class EnumBuilder(IPocoOptions poco)
{
    #region 配置
    private readonly IPocoOptions _poco = poco;
    /// <summary>
    /// 配置
    /// </summary>
    public IPocoOptions Poco
        => _poco;
    #endregion
    /// <summary>
    /// Enum转其他类型
    /// </summary>
    /// <param name="sourceType"></param>
    /// <param name="destType"></param>
    /// <returns></returns>
    public IEmitConverter FromEnum(Type sourceType, Type destType)
    {
        var enumBundle = MemberContainer.Instance.Enums.Get(sourceType);
        if (destType == typeof(string))
            return EnumToString(enumBundle);
        var underlyingType = Enum.GetUnderlyingType(sourceType);
        if (underlyingType == destType)
            return EnumToUnderlying(enumBundle);
#if (NETSTANDARD1_1 || NETSTANDARD1_3 || NETSTANDARD1_6)
        var destIsEnum = destType.GetTypeInfo().IsEnum;
#else
        var destIsEnum = destType.IsEnum;
#endif
        if (destIsEnum)
        {
            var destBundle = MemberContainer.Instance.Enums.Get(destType);
            if(destBundle is not null)
                return EnumToEnum(enumBundle, destBundle);
        }
        IEmitConverter original = _poco.GetEmitConverter(underlyingType, destType);
        if (original is null)
            return null;
        return new EnumToCompatibleConverter(enumBundle, original);
    }
    /// <summary>
    ///其他类型转Enum
    /// </summary>
    /// <param name="sourceType"></param>
    /// <param name="destType"></param>
    /// <returns></returns>
    public IEmitConverter ToEnum(Type sourceType, Type destType)
    {
        var enumBundle = MemberContainer.Instance.Enums.Get(destType);
        if (sourceType == typeof(string))
            return EnumFromString(enumBundle);
        var underType = enumBundle.UnderType;
        if (sourceType == underType)
            return EnumFromUnderlying(enumBundle);
        IEmitConverter original = _poco.GetEmitConverter(sourceType, underType);
        if (original is null)
            return null;
        return new EnumFromCompatibleConverter(enumBundle, original);
    }
    /// <summary>
    /// 基础类型转化为枚举
    /// </summary>
    /// <param name="enumBundle"></param>
    /// <returns></returns>
    public static EnumFromUnderConverter EnumFromUnderlying(IEnumBundle enumBundle)
        => new(enumBundle);
    /// <summary>
    /// 枚举转化为字符串
    /// </summary>
    /// <param name="enumBundle"></param>
    /// <returns></returns>
    public static EnumToStringConverter EnumToString(IEnumBundle enumBundle)
        => new(enumBundle);
    /// <summary>
    /// 枚举转化为枚举
    /// </summary>
    /// <param name="sourceBundle"></param>
    /// <param name="destBundle"></param>
    /// <returns></returns>
    public static EnumToEnumConverter EnumToEnum(IEnumBundle sourceBundle, IEnumBundle destBundle)
        => new(sourceBundle, destBundle);
    /// <summary>
    /// 字符串转化为枚举
    /// </summary>
    /// <param name="enumBundle"></param>
    /// <returns></returns>
    public static EnumFromStringConverter EnumFromString(IEnumBundle enumBundle)
        => new(enumBundle);
    /// <summary>
    /// 枚举转化为基础类型
    /// </summary>
    /// <param name="enumBundle"></param>
    /// <returns></returns>
    public static EnumToUnderConverter EnumToUnderlying(IEnumBundle enumBundle)
        => new(enumBundle);
}
