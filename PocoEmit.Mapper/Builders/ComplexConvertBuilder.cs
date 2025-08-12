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
    : ConvertBuilder
{
    #region 配置
    /// <summary>
    /// Emit配置
    /// </summary>
    protected readonly IMapperOptions _options = options;
    /// <summary>
    /// Emit配置
    /// </summary>
    public IMapperOptions Options
        => _options;
    #endregion
    /// <inheritdoc />
    public override IEmitConverter Build(Type sourceType, Type destType)
    {
        if (destType.IsArray)
            return ToArray(sourceType, destType);
#if (NETSTANDARD1_1 || NETSTANDARD1_3 || NETSTANDARD1_6)
        var isInterface = destType.GetTypeInfo().IsInterface;
#else
        var isInterface = destType.IsInterface;
#endif
        if (ReflectionHelper.HasGenericType(destType, typeof(IDictionary<,>)))
            return ToDictionary(sourceType, destType, isInterface);
        if (ReflectionHelper.HasGenericType(destType, typeof(IEnumerable<>)))
            return ToCollection(sourceType, destType, isInterface);
        // 接口不支持
        if (isInterface)
            return null;
        if (_options.CheckPrimitive(sourceType))
            return base.Build(sourceType, destType);
        var converter = TryBuildByMember(sourceType, destType);
        if(converter is not null)
            return converter;
        if (_options.CheckPrimitive(destType))
            return null;
        var key = new MapTypeKey(sourceType, destType);
        var activator = _options.GetEmitActivator(key) ?? CreateDefaultActivator(destType);
        if (activator is null)
            return null;
        return new ComplexTypeConverter(activator, _options.GetEmitCopier(key));
    }
    /// <summary>
    /// 数组不支持(预留扩展)
    /// </summary>
    /// <param name="sourceType"></param>
    /// <param name="destType"></param>
    /// <returns></returns>
    protected virtual IEmitConverter ToArray(Type sourceType, Type destType)
        => null;
    /// <summary>
    /// 字典不支持(预留扩展)
    /// </summary>
    /// <param name="sourceType"></param>
    /// <param name="destType"></param>
    /// <param name="isInterface"></param>
    /// <returns></returns>
    protected virtual IEmitConverter ToDictionary(Type sourceType, Type destType, bool isInterface)
        => null;
    /// <summary>
    /// 集合不支持(预留扩展)
    /// </summary>
    /// <param name="sourceType"></param>
    /// <param name="destType"></param>
    /// <param name="isInterface"></param>
    /// <returns></returns>
    protected virtual IEmitConverter ToCollection(Type sourceType, Type destType, bool isInterface)
        => null;
    /// <summary>
    /// 尝试按成员读取来转化
    /// </summary>
    /// <param name="sourceType"></param>
    /// <param name="destType"></param>
    /// <returns></returns>
    private MemberReadConverter TryBuildByMember(Type sourceType, Type destType)
    {
        var bundle = _options.MemberCacher.Get(sourceType);
        if (bundle is null)
            return null;
        foreach (var memberReader in bundle.EmitReaders.Values)
        {
            var reader = memberReader;
            if (CheckReader(_options, ref reader, destType) && reader is not null)
                return new MemberReadConverter(reader);
        }
        //var method = ReflectionHelper.GetMethod(sourceType, m => m.GetParameters().Length == 0 && m.ReturnType == destType);
        //if (method is null)
        //    return null;
        //return new SelfMethodConverter(method);
        return null;
    }
    /// <summary>
    /// 检查成员是否匹配
    /// </summary>
    /// <param name="options"></param>
    /// <param name="reader"></param>
    /// <param name="destType"></param>
    /// <returns></returns>
    private static bool CheckReader(IMapperOptions options, ref IEmitMemberReader reader, Type destType)
    {
        var valueType = reader.ValueType;
        if (ReflectionHelper.CheckValueType(valueType, destType))
            return true;
        bool isNullable = false;
        if (ReflectionHelper.IsNullable(valueType))
        {
            valueType = valueType.GenericTypeArguments[0];
            isNullable = true;
        }
        if (ReflectionHelper.IsNullable(destType))
        {
            isNullable = true;
            destType = destType.GenericTypeArguments[0];
        }
        if(isNullable && ReflectionHelper.CheckValueType(valueType, destType))
        {
            options.CheckValueType(ref reader, destType);
            return true;
        }
        return false;
    }
    /// <summary>
    /// 构造默认激活器
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public IEmitActivator CreateDefaultActivator(Type key)
    {
        var constructor = _options.GetConstructor(key);
        if (constructor is not null)
        {
            var parameters = constructor.GetParameters();
            if (parameters.Length == 0)
                return new ConstructorActivator(key, constructor);
            return new ParameterConstructorActivator(_options, key, constructor, parameters);
        }
#if (NETSTANDARD1_1 || NETSTANDARD1_3 || NETSTANDARD1_6)
        var isValueType = key.GetTypeInfo().IsValueType;
#else
        var isValueType = key.IsValueType;
#endif
        if (isValueType)
            return new TypeActivator(key);
        return null;
    }
}
