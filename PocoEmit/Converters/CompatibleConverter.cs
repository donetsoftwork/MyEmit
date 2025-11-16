using Hand.Structural;
using PocoEmit.Builders;
using PocoEmit.Configuration;
using System;
using System.Linq.Expressions;

namespace PocoEmit.Converters;

/// <summary>
/// 兼容类型转化
/// </summary>
/// <param name="isPrimitiveSource"></param>
/// <param name="original"></param>
/// <param name="originalSourceType"></param>
/// <param name="destType"></param>
public sealed class CompatibleConverter(bool isPrimitiveSource, IEmitConverter original, Type originalSourceType, Type destType)
    : EmitConverter(isPrimitiveSource, new(originalSourceType, destType))
    , IEmitConverter
    , IWrapper<IEmitConverter>
{
    #region 配置
    private readonly IEmitConverter _original = original;
    private readonly Type _originalSourceType = originalSourceType;
    /// <inheritdoc />
    public IEmitConverter Original
        => _original;
    /// <summary>
    /// 源转换器类型
    /// </summary>
    public Type OriginalSourceType
        => _originalSourceType;
    /// <inheritdoc />
    bool ICompileInfo.Compiled
        => false;
    #endregion
    /// <inheritdoc />
    protected override Expression ConvertCore(Expression value, Type destType)
        => ConvertDestCore(_original.Convert(ConvertOriginalSourceCore(value, _originalSourceType)), _destType);
    //=> ConvertDest(ConvertOriginal(value));
    #region ConvertOriginal
    ///// <summary>
    ///// 源转化
    ///// </summary>
    ///// <param name="value"></param>
    ///// <returns></returns>
    //private Expression ConvertOriginalSource(Expression value)
    //    => Convert(value, _isPrimitiveSource, _originalSourceType, ConvertOriginalSourceCore);
    /// <summary>
    /// 源核心转化
    /// </summary>
    /// <param name="value"></param>
    /// <param name="originalSourceType"></param>
    /// <returns></returns>
    private Expression ConvertOriginalSourceCore(Expression value, Type originalSourceType)
        => originalSourceType == value.Type ? value : Expression.Convert(value, originalSourceType);
    #endregion
    #region ConvertDest
    ///// <summary>
    ///// 最终转化
    ///// </summary>
    ///// <param name="value"></param>
    ///// <returns></returns>
    //private Expression ConvertDest(Expression value)
    //    => Convert(value, _isPrimitiveSource, _destType, ConvertDestCore);
    /// <summary>
    /// 最终核心转化
    /// </summary>
    /// <param name="value"></param>
    /// <param name="destType"></param>
    /// <returns></returns>
    private Expression ConvertDestCore(Expression value, Type destType)
        => destType == value.Type ? value : Expression.Convert(value, destType);
    #endregion
}
