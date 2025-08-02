using PocoEmit.Configuration;
using PocoEmit.Converters;
using PocoEmit.Members;
using System;

namespace PocoEmit.Builders;

/// <summary>
/// 复杂类型转化构建器
/// </summary>
/// <param name="options"></param>
public class ComplexConvertBuilder(IMapperOptions options)
    : DefaultConvertBuilder
{
    #region 配置
    private readonly IMapperOptions _options = options;
    /// <summary>
    /// Emit配置
    /// </summary>
    public IMapperOptions Options
        => _options;
    #endregion
    /// <inheritdoc />
    public override IEmitConverter Build(Type sourceType, Type destType)
    {
        if (_options.CheckPrimitive(sourceType))
            return base.Build(sourceType, destType);
        var converter = TryBuildByMember(sourceType, destType);
        if(converter is not null)
            return converter;
        if (_options.CheckPrimitive(destType))
            return null;
        var key = new MapTypeKey(sourceType, destType);
        var activator = _options.GetEmitActivatorr(destType);
        if (activator is null)
            return null;
        var copier = _options.GetEmitCopier(key);
        return new ComplexTypeConverter(activator, copier);
    }
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
}
