using PocoEmit.Configuration;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace PocoEmit.Copies;

/// <summary>
/// 兼容类型转化
/// </summary>
/// <param name="inner"></param>
/// <param name="innerSourceType"></param>
/// <param name="destType"></param>
public sealed class CompatibleCopier(IEmitCopier inner, Type innerSourceType, Type destType)
    : IEmitCopier
{
    #region 配置
    private readonly IEmitCopier _inner = inner;
    private readonly Type _innerSourceType = innerSourceType;
    private readonly Type _destType = destType;
    /// <summary>
    /// 内部转换器
    /// </summary>
    public IEmitCopier Inner
        => _inner;
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
    public IEnumerable<Expression> Copy(Expression source, Expression dest)
    {
        if (_innerSourceType != source.Type)
            source = Expression.Convert(source, _innerSourceType);
        if (_destType != dest.Type)
            dest = Expression.Convert(dest, _destType);
        return _inner.Copy(source, dest);
    }
}
