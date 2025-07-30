using PocoEmit.Collections;
using PocoEmit.Configuration;
using PocoEmit.Converters;
using System;
using System.Linq;

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
        var primitives = _options.Primitives;
        if (primitives.Get(sourceType))
            return base.Build(sourceType, destType);
        var converter = TryBuildByMember(sourceType, destType);
        if(converter is not null)
            return converter;
        if (primitives.Get(destType))
            return null;
        var key = new MapTypeKey(sourceType, destType);
        var copier = _options.CopierFactory.Get(key);
        if (copier is null)
            return null;
        var activator = _options.ActivatorFactory.Get(destType);
        if (activator is null)
            return null;
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
        foreach (var member in bundle.ReadMembers.Values)
        {
            var memberReader = MemberContainer.Instance.MemberReaderCacher.Get(member);
            if (memberReader is null)
                continue;
            if (memberReader.ValueType == destType)
                return new MemberReadConverter(memberReader);
        }
        return null;
    }
}
