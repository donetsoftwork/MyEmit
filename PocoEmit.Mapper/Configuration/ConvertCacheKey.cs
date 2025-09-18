using System;

namespace PocoEmit.Configuration;

#if NET7_0_OR_GREATER
/// <summary>
/// 转化缓存键
/// </summary>
/// <param name="Source"></param>
/// <param name="DestType"></param>
public record ConvertCacheKey(object Source, Type DestType)
{
#else
/// <summary>
/// 转化缓存键
/// </summary>
/// <param name="source"></param>
/// <param name="destType"></param>
public class ConvertCacheKey(object source, Type destType)
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
    /// <summary>
    /// HashCode
    /// </summary>
    /// <returns></returns>
    public override int GetHashCode()
#if (NETSTANDARD1_1 || NETSTANDARD1_3 || NETSTANDARD1_6 || NET45)
        => _destType.GetHashCode() ^ _source.GetHashCode();
#else
        => HashCode.Combine(_destType, _source);
#endif
    #region IEquatable
    /// <summary>
    /// 判同
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool Equals(ConvertCacheKey other)
        => _destType.Equals(other._destType) && _source.Equals(other._source);
    /// <summary>
    /// 判同
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public override bool Equals(object other)
        => other is ConvertCacheKey key && Equals(key);
    #endregion
#endif
}
