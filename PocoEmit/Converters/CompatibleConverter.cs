using PocoEmit.Configuration;
using System;
using System.Linq.Expressions;

namespace PocoEmit.Converters;

/// <summary>
/// 兼容类型转化
/// </summary>
/// <param name="original"></param>
/// <param name="originalSourceType"></param>
/// <param name="destType"></param>
public sealed class CompatibleConverter(IEmitConverter original, Type originalSourceType, Type destType)
    : IEmitConverter
{
    #region 配置
    private readonly IEmitConverter _original = original;
    private readonly Type _originalSourceType = originalSourceType;
    private readonly Type _destType = destType;
    /// <summary>
    /// 源转换器
    /// </summary>
    public IEmitConverter Original
        => _original;
    /// <summary>
    /// 源转换器类型
    /// </summary>
    public Type OriginalSourceType
        => _originalSourceType;
    /// <summary>
    /// 映射目标类型
    /// </summary>
    public Type DestType
        => _destType;
    /// <inheritdoc />
    bool ICompileInfo.Compiled
        => false;
    #endregion
    /// <inheritdoc />
    public Expression Convert(Expression value)
    {
        if (_originalSourceType != value.Type)
            value = Expression.Convert(value, _originalSourceType);
        var convert = _original.Convert(value);
        return _destType == convert.Type ? convert : Expression.Convert(convert, _destType);
    }
}
