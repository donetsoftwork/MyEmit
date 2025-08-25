using PocoEmit.Collections;
using PocoEmit.Configuration;
using PocoEmit.Enums;

namespace PocoEmit.Converters;

/// <summary>
/// 转化器工厂
/// </summary>
public sealed class ConverterFactory(IPocoOptions options)
    : CacheBase<PairTypeKey, IEmitConverter>(options)
{
    #region 配置
    private readonly IPocoOptions _options = options;
    /// <summary>
    /// Emit配置
    /// </summary>
    public IPocoOptions Options
        => _options;
    private readonly EnumBuilder _enumBuilder = new(options);
    #endregion
    #region CacheBase
    /// <inheritdoc />
    protected override IEmitConverter CreateNew(PairTypeKey key)
        => _options.ConvertBuilder.Build(key.LeftType, key.RightType);
    #endregion

}
