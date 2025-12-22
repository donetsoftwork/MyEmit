using Hand.Reflection;
using PocoEmit.Activators;
using PocoEmit.Configuration;
using PocoEmit.Converters;
using PocoEmit.Members;
using System;
using System.Collections.Generic;
#if (NETSTANDARD1_1 || NETSTANDARD1_3 || NETSTANDARD1_6)
using System.Reflection;
#endif

namespace PocoEmit.Builders;

/// <summary>
/// 复杂类型转化构建器
/// </summary>
/// <param name="options"></param>
public class ComplexConvertBuilder(IMapperOptions options)
    : ConvertBuilder(options)
{
    #region 配置
    /// <summary>
    /// Emit配置
    /// </summary>
    protected readonly IMapperOptions _options = options;
    /// <summary>
    /// Emit配置
    /// </summary>
    public new IMapperOptions Options
        => _options;
    #endregion
    /// <inheritdoc />
    public override IEmitConverter Build(Type sourceType, Type destType)
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
        return BuildOther(sourceType, destType);
    }
    /// <inheritdoc />
    protected override IEmitConverter BuildOther(Type sourceType, Type destType)
    {
        var sourceIsPrimitive = _options.CheckPrimitive(sourceType);
        IEmitConverter converter = null;
        // 基础类型没有成员
        if (!sourceIsPrimitive && TryBuildByMember(sourceType, destType, ref converter))
            return converter;
        var destIsPrimitive = _options.CheckPrimitive(destType);
        // 系统类型转换
        if (sourceIsPrimitive && destIsPrimitive)
            return BuildByConvert(sourceType, destType);
        if (destType.IsArray)
            return null;
        //if (ReflectionType.HasGenericType(destType, typeof(IDictionary<,>)))
        //    return null;
        if (ReflectionType.HasGenericType(destType, typeof(IEnumerable<>)))
            return null;
        if (!destIsPrimitive && TryBuildByConstructor(sourceType, destType, ref converter))
            return converter;
#if (NETSTANDARD1_1 || NETSTANDARD1_3 || NETSTANDARD1_6)
        var isInterface = destType.GetTypeInfo().IsInterface;
#else
        var isInterface = destType.IsInterface;
#endif
        if (destIsPrimitive || isInterface)
            return null;
        var key = new PairTypeKey(sourceType, destType);
        var activator = _options.GetEmitActivator(key) ?? CreateDefaultActivator(sourceType, destType);
        if (activator is null)
            return null;
        return new ComplexTypeConverter(_options, key, activator, _options.GetEmitCopier(key));
    }
    /// <inheritdoc />
    public override IEmitConverter BuildForNullable(IEmitConverter original, Type originalSourceType, Type destType)
        => new CompatibleConverter(_options.CheckPrimitive(originalSourceType), original, originalSourceType, destType);
    /// <summary>
    /// 尝试构造函数转化器
    /// </summary>
    public static bool TryBuildByConstructor(Type sourceType, Type destType, ref IEmitConverter converter)
    {
        // 按构造函数
        var constructor = ReflectionMember.GetConstructorByParameterType(destType, sourceType);
        if (constructor is null)
        {
#if (NETSTANDARD1_1 || NETSTANDARD1_3 || NETSTANDARD1_6)
        var isValueType = sourceType.GetTypeInfo().IsValueType;
#else
            var isValueType = sourceType.IsValueType;
#endif
            if (isValueType)
            {
                var compatibleSourceType = typeof(Nullable<>).MakeGenericType(sourceType);
                constructor = ReflectionMember.GetConstructorByParameterType(destType, compatibleSourceType);
                if (constructor is null)
                    return false;
                converter = new ConstructorCompatibleConverter(new(sourceType, destType), constructor, compatibleSourceType);
                return true;
            }
        }
        else
        {
            converter = new ConstructorConverter(new(sourceType, destType), constructor);
            return true;
        }
        return false;
    }
    /// <summary>
    /// 系统转化
    /// </summary>
    /// <param name="sourceType"></param>
    /// <param name="destType"></param>
    /// <returns></returns>
    protected IEmitConverter BuildByConvert(Type sourceType, Type destType)
    {
        IEmitConverter converter = null;
        TryBuildByConvert(sourceType, destType, ref converter);
        return converter;
    }
    /// <summary>
    /// 尝试按成员读取来转化
    /// </summary>
    /// <param name="sourceType"></param>
    /// <param name="destType"></param>
    /// <param name="converter"></param>
    /// <returns></returns>
    protected bool TryBuildByMember(Type sourceType, Type destType, ref IEmitConverter converter)
    {
        var bundle = _options.MemberCacher.Get(sourceType);
        if (bundle is null)
            return false;
        foreach (var memberReader in bundle.EmitReaders.Values)
        {
            var reader = memberReader;
            if (CheckReader(_options, ref reader, destType) && reader is not null)
            {
                converter = new MemberReadConverter(new(sourceType, destType), reader);
                return true;
            }
        }
        return false;
    }
    /// <summary>
    /// 检查成员是否匹配
    /// </summary>
    /// <param name="options"></param>
    /// <param name="reader"></param>
    /// <param name="destType"></param>
    /// <returns></returns>
    protected static bool CheckReader(IMapperOptions options, ref IEmitMemberReader reader, Type destType)
    {
        var valueType = reader.ValueType;
        if (PairTypeKey.CheckValueType(valueType, destType))
            return true;
        bool isNullable = false;
        if (ReflectionType.IsNullable(valueType))
        {
            valueType = Nullable.GetUnderlyingType(valueType);
            isNullable = true;
        }
        if (ReflectionType.IsNullable(destType))
        {
            isNullable = true;
            destType = Nullable.GetUnderlyingType(destType);
        }
        if(isNullable && PairTypeKey.CheckValueType(valueType, destType))
        {
            options.CheckValueType(ref reader, destType);
            return true;
        }
        return false;
    }
    /// <summary>
    /// 构造默认激活器
    /// </summary>
    /// <param name="sourceType"></param>
    /// <param name="destType"></param>
    /// <returns></returns>
    public IEmitActivator CreateDefaultActivator(Type sourceType, Type destType)
    {
        var constructor = _options.GetConstructor(destType);
        if (constructor is not null)
        {
            var parameters = constructor.GetParameters();
            if (parameters.Length == 0)
                return new ConstructorActivator(destType, constructor);
            return ParameterConstructorActivator.Create(_options, sourceType, destType, constructor, parameters);
        }
#if (NETSTANDARD1_1 || NETSTANDARD1_3 || NETSTANDARD1_6)
        var isValueType = destType.GetTypeInfo().IsValueType;
#else
        var isValueType = destType.IsValueType;
#endif
        if (isValueType)
            return new TypeActivator(destType);
        return null;
    }
}
