using System;
#if NET7_0_OR_GREATER || NETSTANDARD2_0_OR_GREATER
using System.Runtime.CompilerServices;
#endif

namespace PocoEmit.Configuration;

/// <summary>
/// 转化缓存键
/// </summary>
/// <param name="source"></param>
/// <param name="destType"></param>
public readonly struct ConvertCacheKey(object source, Type destType)
     : IEquatable<ConvertCacheKey>
{
    #region 配置
    private readonly object _source = source;
    private readonly Type _destType = destType;
    /// <summary>
    /// 映射源对象
    /// </summary>
    public object Source
        => _source;
    /// <summary>
    /// 映射类型
    /// </summary>
    public Type DestType
        => _destType;
    #endregion
    /// <inheritdoc />
    public override int GetHashCode()
#if (NETSTANDARD1_1 || NETSTANDARD1_3 || NETSTANDARD1_6 || NET45)
        => _destType.GetHashCode() ^ _source.GetHashCode();
#else
        => HashCode.Combine(_destType, RuntimeHelpers.GetHashCode(_source));
#endif
#region IEquatable
    /// <inheritdoc />
    public bool Equals(ConvertCacheKey other)
        => _destType.Equals(other._destType) && _source.Equals(other._source);
    /// <inheritdoc />
    public override bool Equals(object other)
        => other is ConvertCacheKey key && Equals(key);
    #endregion
}