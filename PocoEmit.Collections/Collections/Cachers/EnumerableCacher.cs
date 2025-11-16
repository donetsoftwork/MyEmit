using Hand.Cache;
using Hand.Reflection;
using PocoEmit.Collections.Bundles;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace PocoEmit.Collections.Cachers;

/// <summary>
/// 迭代器缓存
/// </summary>
/// <param name="container"></param>
internal class EnumerableCacher(CollectionContainer container)
    : CacheFactoryBase<Type, EnumerableBundle>(container)
{
    #region 配置
    private readonly CollectionContainer _container = container;
    /// <summary>
    /// 集合容器
    /// </summary>
    public CollectionContainer Container
        => _container;
    #endregion
    /// <inheritdoc />
    protected override EnumerableBundle CreateNew(in Type key)
    {
        if (!ReflectionType.HasGenericType(key, typeof(IEnumerable<>)))
            return null;
        return CreateByType(key);
    }
    /// <summary>
    /// 验证集合类型是否合法
    /// </summary>
    /// <param name="enumerableType"></param>
    /// <returns></returns>
    public bool Validate(Type enumerableType)
        => Validate(enumerableType, out var _);
    /// <summary>
    /// 验证集合类型是否合法
    /// </summary>
    /// <param name="enumerableType"></param>
    /// <param name="bundle"></param>
    /// <returns></returns>
    public bool Validate(Type enumerableType, out EnumerableBundle bundle)
    {
        if (ReflectionType.HasGenericType(enumerableType, typeof(IEnumerable<>)))
            return (bundle = Get(enumerableType)) is not null;
        return TryGetCache(enumerableType, out bundle) && bundle is not null;
    }
    /// <summary>
    /// 获取迭代器成员
    /// </summary>
    /// <param name="enumerableType"></param>
    /// <returns></returns>
    public static EnumerableBundle CreateByType(Type enumerableType)
    {
        var enumerableInterface = ReflectionType.GetGenericCloseInterfaces(enumerableType, typeof(IEnumerable<>))
            .FirstOrDefault();
        if (enumerableInterface is null)
            return null;
        var arguments = ReflectionType.GetGenericArguments(enumerableInterface);
        var elementType = arguments[0];
        var getEnumeratorMethod = GetGetEnumerator(enumerableType)
            ?? GetGetEnumerator(enumerableInterface);
        var enumeratorType = getEnumeratorMethod.ReturnType;
        var moveNextMethod = GetMoveNext(enumeratorType) ?? _moveNextCore;
        var currentProperty = GetCurrent(enumeratorType) ?? _currentCore;
        var requireIEnumerator = false;
        if(currentProperty is not null && elementType is null)
            elementType = currentProperty.PropertyType;
        //if (currentProperty == null || moveNextMethod is null)
        //{
        //    moveNextMethod = _moveNextCore;
        //    currentProperty = _currentCore;
        //    requireIEnumerator = true;
        //}
        return new(enumeratorType, elementType, getEnumeratorMethod, moveNextMethod, currentProperty, requireIEnumerator);
    }
    #region MethodInfo
    /// <summary>
    /// 获取GetEnumerator方法
    /// </summary>
    /// <param name="enumerableType"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public static MethodInfo GetGetEnumerator(Type enumerableType)
        => ReflectionMember.GetMethod(enumerableType, "GetEnumerator");
    /// <summary>
    /// 获取MoveNext方法
    /// </summary>
    /// <param name="enumeratorType"></param>
    /// <returns></returns>
    private static MethodInfo GetMoveNext(Type enumeratorType)
        => ReflectionMember.GetMethod(enumeratorType, "MoveNext");

    /// <summary>
    /// 打底的MoveNext方法
    /// </summary>
    private static readonly MethodInfo _moveNextCore = GetMoveNext(typeof(IEnumerator));
    #endregion
    #region PropertyInfo
    /// <summary>
    /// 获取Current属性
    /// </summary>
    /// <param name="enumeratorType"></param>
    /// <returns></returns>
    public static PropertyInfo GetCurrent(Type enumeratorType)
        => ReflectionMember.GetPropery(enumeratorType, property => property.Name == "Current");
    /// <summary>
    /// 打底的Current属性
    /// </summary>
    private static readonly PropertyInfo _currentCore = GetCurrent(typeof(IEnumerator));
    #endregion
}
