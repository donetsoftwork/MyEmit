using System;
#if (NETSTANDARD1_1 || NETSTANDARD1_3 || NETSTANDARD1_6)
using System.Reflection;
#endif

namespace PocoEmit.Configuration;
#if NET7_0_OR_GREATER
/// <summary>
/// 类型关联键
/// </summary>
/// <param name="LeftType"></param>
/// <param name="RightType"></param>
public record PairTypeKey(Type LeftType, Type RightType)
{
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
#endif
    #region CheckNullable
    /// <summary>
    /// 判断可空类型
    /// </summary>
    /// <param name="leftType"></param>
    /// <param name="rightType"></param>
    /// <returns></returns>
    public static bool CheckNullable(ref Type leftType, ref Type rightType)
    {
        bool isNullable = false;
        if (ReflectionHelper.IsNullable(leftType))
        {
            leftType = Nullable.GetUnderlyingType(leftType);
            isNullable = true;
        }
        if (ReflectionHelper.IsNullable(rightType))
        {
            rightType = Nullable.GetUnderlyingType(rightType);
            return true;
        }
        return isNullable;
    }
    #endregion
    #region CheckNullCondition
    /// <summary>
    /// 判断是否需要检查null
    /// </summary>
    /// <param name="declareType"></param>
    /// <returns></returns>
    public static bool CheckNullCondition(Type declareType)
    {
#if (NETSTANDARD1_1 || NETSTANDARD1_3 || NETSTANDARD1_6)
        var sourceTypeInfo = declareType.GetTypeInfo();
        return sourceTypeInfo.IsGenericType || !sourceTypeInfo.IsValueType;
#else
        return declareType.IsGenericType || !declareType.IsValueType;
#endif
    }
    /// <summary>
    /// 判断是否需要检查null(值类型转非值类型需要检查null)
    /// </summary>
    /// <param name="sourceType"></param>
    /// <param name="destType"></param>
    /// <returns></returns>
    public static bool CheckNullCondition(Type sourceType, Type destType)
    {
#if (NETSTANDARD1_1 || NETSTANDARD1_3 || NETSTANDARD1_6)
        var destTypeInfo = destType.GetTypeInfo();
        if(destTypeInfo.IsGenericType)
            return false;
        return destTypeInfo.IsValueType && CheckNullCondition(sourceType);
#else
        if (destType.IsGenericType)
            return false;
        return destType.IsValueType && CheckNullCondition(sourceType);
#endif
    }
    #endregion
    #region CheckType
    /// <summary>
    /// 判断是否兼容值类型
    /// </summary>
    /// <param name="leftType"></param>
    /// <param name="rightType"></param>
    /// <returns></returns>
    public static bool CheckValueType(Type leftType, Type rightType)
#if (NETSTANDARD1_1 || NETSTANDARD1_3 || NETSTANDARD1_6)
        => CheckValueType(leftType.GetTypeInfo(), rightType.GetTypeInfo());
    /// <summary>
    /// 判断是否兼容值类型
    /// </summary>
    /// <param name="leftType"></param>
    /// <param name="rightType"></param>
    /// <returns></returns>
    public static bool CheckValueType(TypeInfo leftType, TypeInfo rightType)
#endif
    {
        if (leftType == rightType)
            return true;
        if (rightType.IsValueType)
        {
            if (leftType.IsValueType && !rightType.IsGenericType)
                return rightType.IsAssignableFrom(leftType);
            return false;
        }
        if (leftType.IsValueType)
            return false;
        return rightType.IsAssignableFrom(leftType);
    }
    #endregion
}