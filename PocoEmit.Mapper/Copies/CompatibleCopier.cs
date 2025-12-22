using Hand.Structural;
using PocoEmit.Builders;
using PocoEmit.Complexes;
using PocoEmit.Configuration;
using System;
using System.Linq.Expressions;

namespace PocoEmit.Copies;

/// <summary>
/// 兼容类型转化
/// </summary>
/// <param name="original"></param>
/// <param name="innerSourceType"></param>
/// <param name="destType"></param>
public sealed class CompatibleCopier(IEmitCopier original, Type innerSourceType, Type destType)
    : IEmitCopier
    , IWrapper<IEmitCopier>
{
    #region 配置
    private readonly IEmitCopier _original = original;
    private readonly Type _innerSourceType = innerSourceType;
    private readonly Type _destType = destType;
    /// <summary>
    /// 内部转换器
    /// </summary>
    public IEmitCopier Original
        => _original;
    /// <summary>
    /// 内部转换器源类型
    /// </summary>
    public Type InnerSourceType
        => _innerSourceType;
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
    public void BuildAction(IBuildContext context, ComplexBuilder builder, Expression source, Expression dest)
    {
        if (_innerSourceType != source.Type)
            source = Expression.Convert(source, _innerSourceType);
        if (_destType != dest.Type)
            dest = Expression.Convert(dest, _destType);
        _original.BuildAction(context, builder, source, dest);
    }
    /// <inheritdoc />
    public void Preview(IComplexBundle parent)
        => _original.Preview(parent);
}
