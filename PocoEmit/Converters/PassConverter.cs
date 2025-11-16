using Hand.Reflection;
using PocoEmit.Configuration;
using System;
using System.Linq.Expressions;

namespace PocoEmit.Converters;

/// <summary>
/// 相同类型直接跳过转化
/// </summary>
public sealed class PassConverter(Type sourceType)
    : IEmitConverter
{
    #region 配置
    private readonly PairTypeKey _key = new(sourceType, sourceType);
    /// <inheritdoc />
    public PairTypeKey Key
        => _key;
    bool ICompileInfo.Compiled
        => false;
    #endregion
    /// <inheritdoc />
    public Expression Convert(Expression source)
        => source;
}
