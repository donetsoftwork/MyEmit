using Hand.Reflection;
using PocoEmit.Configuration;
using PocoEmit.Converters;
using PocoEmit.Enums;
using System;
using System.Reflection;

namespace PocoEmit.Builders;

/// <summary>
/// 默认转换构建器
/// </summary>
public class ConvertBuilder(IPocoOptions options)
{
    #region 配置
    /// <summary>
    /// Emit配置
    /// </summary>
    private readonly IPocoOptions _options = options;
    /// <summary>
    /// Emit配置
    /// </summary>
    public IPocoOptions Options
        => _options;
    private readonly EnumBuilder _enumBuilder = new(options);
    #endregion
    /// <summary>
    /// 构建转换器
    /// </summary>
    /// <param name="sourceType">源类型</param>
    /// <param name="destType">目标类型</param>
    /// <returns>转换器</returns>
    public virtual IEmitConverter Build(Type sourceType, Type destType)
    {
        // 同类型
        if (sourceType == destType)
            return BuildForSelf(destType);
        IEmitConverter converter = null;
        // object类型
        if (TryBuildObject(sourceType, destType, ref converter))
            return converter;
        // 兼容类型
        if (PairTypeKey.CheckValueType(sourceType, destType))
            return BuildForSelf(destType);
        // 可空类型
        if (TryBuildNullable(sourceType, destType, ref converter))
            return converter;
        // 枚举类型
        if (TryBuildEnumConverter(sourceType, destType, ref converter))
            return converter;
        // 字符串
        if (destType == typeof(string))
            return BuildForString(sourceType);
        // 系统类型转换
        if (TryBuildByConvert(sourceType, destType, ref converter))
            return converter;
        return BuildOther(sourceType, destType) ?? BuildByEmit(sourceType, destType);
    }
    /// <summary>
    /// 通过Emit构建转换器
    /// </summary>
    /// <param name="sourceType"></param>
    /// <param name="destType"></param>
    /// <returns></returns>
    protected static EmitConverter BuildByEmit(Type sourceType, Type destType)
        => new(false, new(sourceType, destType));
    /// <summary>
    /// 其他类型
    /// </summary>
    /// <param name="sourceType"></param>
    /// <param name="destType"></param>
    /// <returns></returns>
    protected virtual IEmitConverter BuildOther(Type sourceType, Type destType)
        => null;
    /// <summary>
    /// 尝试object类型转化
    /// </summary>
    /// <param name="sourceType"></param>
    /// <param name="destType"></param>
    /// <param name="converter"></param>
    /// <returns></returns>
    protected static bool TryBuildObject(Type sourceType, Type destType, ref IEmitConverter converter)
    {
        if (sourceType == typeof(object) || destType == typeof(object))
        {
            converter = BuildByEmit(sourceType, destType);
            return true;
        }
        return false;
    }
    /// <summary>
    /// 尝试构建可空类型转换器
    /// </summary>
    /// <param name="sourceType"></param>
    /// <param name="destType"></param>
    /// <param name="converter"></param>
    /// <returns></returns>
    protected bool TryBuildNullable(Type sourceType, Type destType, ref IEmitConverter converter)
    {
        var destType0 = destType;
        // 可空类型
        if (PairTypeKey.CheckNullable(ref sourceType, ref destType))
        {
            IEmitConverter original = _options.GetEmitConverter(sourceType, destType);
            if (original is null)
                return false;
            // 可空类型
            converter = BuildForNullable(original, sourceType, destType0);
            return true;
        }
        return false;
    }
    /// <summary>
    /// 尝试构建枚举转换器
    /// </summary>
    /// <param name="sourceType"></param>
    /// <param name="destType"></param>
    /// <param name="converter"></param>
    /// <returns></returns>
    protected bool TryBuildEnumConverter(Type sourceType, Type destType, ref IEmitConverter converter)
    {
#if (NETSTANDARD1_1 || NETSTANDARD1_3 || NETSTANDARD1_6)
        var sourceTypeInfo = sourceType.GetTypeInfo();
        var destTypeInfo = destType.GetTypeInfo();
        var sourceIsEnum = sourceTypeInfo.IsEnum;
#else
        var sourceIsEnum = sourceType.IsEnum;
#endif
        if (sourceIsEnum)
        {
            converter = _enumBuilder.FromEnum(sourceType, destType);
            return true;
        }
            
#if (NETSTANDARD1_1 || NETSTANDARD1_3 || NETSTANDARD1_6)
        var destIsEnum = destTypeInfo.IsEnum;
#else
        var destIsEnum = destType.IsEnum;
#endif
        if (destIsEnum)
        {
            converter = _enumBuilder.ToEnum(sourceType, destType);
            return true;
        }
        return false;
    }
    /// <summary>
    /// 构建同类型转换器
    /// </summary>
    /// <param name="instanceType">类型</param>
    /// <returns>转换器</returns>
    public virtual IEmitConverter BuildForSelf(Type instanceType)
        => new PassConverter(instanceType);
    /// <summary>
    /// 构建字符串转换器
    /// </summary>
    /// <param name="sourceType"></param>
    /// <returns></returns>
    public virtual IEmitConverter BuildForString(Type sourceType)
        => SelfMethodConverter.ConvertToString(sourceType);
    /// <summary>
    /// 构建Nullable转换器
    /// </summary>
    /// <param name="original"></param>
    /// <param name="originalSourceType"></param>
    /// <param name="destType"></param>
    /// <returns></returns>
    public virtual IEmitConverter BuildForNullable(IEmitConverter original, Type originalSourceType, Type destType)
        => new CompatibleConverter(false, original, originalSourceType, destType);
    /// <summary>
    /// 尝试系统类型转化
    /// </summary>
    /// <param name="sourceType"></param>
    /// <param name="destType"></param>
    /// <param name="converter"></param>
    /// <returns></returns>
    protected virtual bool TryBuildByConvert(Type sourceType, Type destType, ref IEmitConverter converter)
    {
        var method = ReflectionMember.GetMethod(typeof(Convert), "To" + destType.Name, [sourceType]);
        if (method is null)
            return false;
        converter = new MethodConverter(null, method);
        return true;
    }
    /// <summary>
    /// 判断是否为类型转化静态方法
    /// </summary>
    /// <param name="method"></param>
    /// <param name="sourceType"></param>
    /// <param name="destType"></param>
    /// <returns></returns>
    public static bool CheckStaticConvertMethod(MethodInfo method, Type sourceType, Type destType)
    {
        if (method.IsStatic && method.ReturnType == destType)
        {
            var parameters = method.GetParameters();
            return parameters.Length == 1 && parameters[0].ParameterType == sourceType;
        }
        return false;
    }
}
