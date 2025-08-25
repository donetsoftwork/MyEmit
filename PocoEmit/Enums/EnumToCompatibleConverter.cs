using PocoEmit.Converters;
using System.Linq.Expressions;

namespace PocoEmit.Enums;

/// <summary>
/// 枚举转化为兼容类型
/// </summary>
/// <param name="bundle"></param>
/// <param name="converter"></param>
public class EnumToCompatibleConverter(IEnumBundle bundle, IEmitConverter converter)
    : EnumToUnderConverter(bundle)
{
    #region 配置
    private readonly IEmitConverter _converter = converter;
    /// <summary>
    /// 基础类型转化为兼容类型
    /// </summary>
    public IEmitConverter Converter 
        => _converter;
    #endregion
    /// <inheritdoc />
    public override Expression Convert(Expression source)
        => _converter.Convert(base.Convert(source));
}
