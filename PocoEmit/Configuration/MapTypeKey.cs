using System;

namespace PocoEmit.Configuration;
#if NET7_0_OR_GREATER
/// <summary>
/// 映射类型关联键
/// </summary>
/// <param name="SourceType"></param>
/// <param name="DestType"></param>
public record  MapTypeKey(Type SourceType, Type DestType);
#else
/// <summary>
/// 映射类型关联键
/// </summary>
/// <param name="sourceType"></param>
/// <param name="destType"></param>
public class MapTypeKey(Type sourceType,Type destType)
     : IEquatable<MapTypeKey>
{
    #region 配置
    private readonly Type _sourceType = sourceType;
    private readonly Type _destType = destType;
    /// <summary>
    /// 映射源类型
    /// </summary>
    public Type SourceType 
        => _sourceType;
    /// <summary>
    /// 映射目标类型
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
        => _sourceType.GetHashCode() ^ _destType.GetHashCode();
#else
        => HashCode.Combine(_sourceType, _destType);
#endif
#region IEquatable
    /// <summary>
    /// 判同
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool Equals(MapTypeKey other)
        => _sourceType.Equals(other._sourceType) && _destType.Equals(other._destType);
    /// <summary>
    /// 判同
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public override bool Equals(object other)
        => other is MapTypeKey key && Equals(key);
    #endregion
}
#endif