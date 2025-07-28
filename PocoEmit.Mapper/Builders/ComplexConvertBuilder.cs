using PocoEmit.Configuration;
using PocoEmit.Converters;
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
        if (_options.Primitives.Get(destType))
            return base.Build(sourceType, destType);
        if (_options.Primitives.Get(destType))
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
}
