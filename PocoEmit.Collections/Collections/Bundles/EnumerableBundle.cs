using System;
using System.Reflection;

namespace PocoEmit.Collections.Bundles;

#if NET7_0_OR_GREATER
/// <summary>
/// 迭代器成员
/// </summary>
/// <param name="EnumeratorType">迭代器类型</param>
/// <param name="ElementType">子元素类型</param>
/// <param name="GetEnumeratorMethod">GetEnumerator方法</param>
/// <param name="MoveNextMethod">MoveNext方法</param>
/// <param name="CurrentProperty">Current属性</param>
/// <param name="RequireIEnumerator">依赖IEnumerator接口</param>
public record EnumerableBundle(Type EnumeratorType, Type ElementType, MethodInfo GetEnumeratorMethod, MethodInfo MoveNextMethod, PropertyInfo CurrentProperty, bool RequireIEnumerator);
#else
/// <summary>
/// 迭代器成员
/// </summary>
/// <param name="enumeratorType">迭代器类型</param>
/// <param name="elementType">子元素类型</param>
/// <param name="getEnumeratorMethod">GetEnumerator方法</param>
/// <param name="moveNextMethod">MoveNext方法</param>
/// <param name="currentProperty">Current属性</param>
/// <param name="requireIEnumerator">依赖IEnumerator接口</param>
public class EnumerableBundle(Type enumeratorType, Type elementType, MethodInfo getEnumeratorMethod, MethodInfo moveNextMethod, PropertyInfo currentProperty, bool requireIEnumerator)
{
    #region 配置
    /// <summary>
    /// GetEnumerator方法
    /// </summary>
    public MethodInfo GetEnumeratorMethod { get; } = getEnumeratorMethod;
    /// <summary>
    /// 迭代器类型
    /// </summary>
    public Type EnumeratorType { get; } = enumeratorType;
    /// <summary>
    /// 子元素类型
    /// </summary>
    public Type ElementType { get; } = elementType;
    /// <summary>
    /// MoveNext方法
    /// </summary>
    public MethodInfo MoveNextMethod { get; } = moveNextMethod;
    /// <summary>
    /// Current属性
    /// </summary>
    public PropertyInfo CurrentProperty { get; } = currentProperty;
    /// <summary>
    /// 依赖IEnumerator接口
    /// </summary>
    public bool RequireIEnumerator { get; } = requireIEnumerator;
    #endregion
}
#endif