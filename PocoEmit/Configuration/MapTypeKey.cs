using System;

namespace PocoEmit.Configuration;
#if NET7_0_OR_GREATER
/// <summary>
/// 类型关联键
/// </summary>
/// <param name="LeftType"></param>
/// <param name="RightType"></param>
public record PairTypeKey(Type LeftType, Type RightType);
#else
/// <summary>
/// 类型关联键
/// </summary>
/// <param name="leftType"></param>
/// <param name="rightType"></param>
public class PairTypeKey(Type leftType,Type rightType)
     : IEquatable<PairTypeKey>
{
    #region 配置
    private readonly Type _leftType = leftType;
    private readonly Type _rightType = rightType;
    /// <summary>
    /// 映射源类型
    /// </summary>
    public Type LeftType 
        => _leftType;
    /// <summary>
    /// 映射目标类型
    /// </summary>
    public Type RightType
        => _rightType;
    #endregion
    /// <summary>
    /// HashCode
    /// </summary>
    /// <returns></returns>
    public override int GetHashCode()
#if (NETSTANDARD1_1 || NETSTANDARD1_3 || NETSTANDARD1_6 || NET45)
        => _leftType.GetHashCode() ^ _rightType.GetHashCode();
#else
        => HashCode.Combine(_leftType, _rightType);
#endif
#region IEquatable
    /// <summary>
    /// 判同
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool Equals(PairTypeKey other)
        => _leftType.Equals(other._leftType) && _rightType.Equals(other._rightType);
    /// <summary>
    /// 判同
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public override bool Equals(object other)
        => other is PairTypeKey key && Equals(key);
    #endregion
}
#endif