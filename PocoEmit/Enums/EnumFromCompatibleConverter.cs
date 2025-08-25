using PocoEmit.Converters;
using System.Linq.Expressions;

namespace PocoEmit.Enums;

/// <summary>
/// 兼容类型转化为枚举
/// </summary>
/// <param name="bundle"></param>
/// <param name="converter"></param>
public class EnumFromCompatibleConverter(IEnumBundle bundle, IEmitConverter converter)
    : EnumFromUnderConverter(bundle)
{
    #region 配置
    private readonly IEmitConverter _converter = converter;
    /// <summary>
    /// 兼容类型转化为基础类型
    /// </summary>
    public IEmitConverter Converter
        => _converter;
    #endregion
    /// <inheritdoc />
    public override Expression Convert(Expression source)
        => base.Convert(_converter.Convert(source));
}
