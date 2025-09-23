using PocoEmit.Collections.Bundles;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace PocoEmit.Collections.Cachers;

/// <summary>
/// 字典成员缓存
/// </summary>
/// <param name="container"></param>
internal class DictionaryCacher(CollectionContainer container)
    : CacheBase<Type, DictionaryBundle>(container)
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
    protected override DictionaryBundle CreateNew(in Type key)
    {
        if (!ReflectionHelper.HasGenericType(key, typeof(IDictionary<,>)))
            return null;
        return CreateByType(_container, key);
    }
    /// <summary>
    /// 验证列表类型是否合法
    /// </summary>
    /// <param name="dictionaryType"></param>
    /// <returns></returns>
    public bool Validate(Type dictionaryType)
        => Validate(dictionaryType, out var _);
    /// <summary>
    /// 验证列表类型是否合法
    /// </summary>
    /// <param name="dictionaryType"></param>
    /// <param name="bundle"></param>
    /// <returns></returns>
    public bool Validate(Type dictionaryType, out DictionaryBundle bundle)
    {
        if (ReflectionHelper.HasGenericType(dictionaryType, typeof(IDictionary<,>)))
            return (bundle = Get(dictionaryType)) is not null;
        return TryGetValue(dictionaryType, out bundle) && bundle is not null;
    }
    /// <summary>
    /// 获取字典成员
    /// </summary>
    /// <param name="container"></param>
    /// <param name="dictionaryType"></param>
    /// <returns></returns>
    public static DictionaryBundle CreateByType(CollectionContainer container, Type dictionaryType)
    {
        var keys = GetKeysProperty(dictionaryType);
        if (keys is null)
            return null;
        var values = GetValuesProperty(dictionaryType);
        if (values is null)
            return null;
        var arguments = ReflectionHelper.GetGenericArguments(dictionaryType);
        Type keyType;
        Type valueType;
        if (arguments.Length == 2)
        {
            keyType = arguments[0];
            valueType = arguments[1];
        }
        else
        {
            var keysBundle = container.EnumerableCacher.Get(keys.PropertyType);
            if (keysBundle is null)
                return null;
            keyType = keysBundle.CurrentProperty.PropertyType;
            var valuesBundle = container.EnumerableCacher.Get(values.PropertyType);
            if (valuesBundle is null)
                return null;
            valueType = valuesBundle.CurrentProperty.PropertyType;
        }
        var items = CollectionContainer.GetItemProperty(dictionaryType, keyType);
        if (items is null)
            return null;
        var count = CollectionContainer.GetCountProperty(dictionaryType);
        if (count is null)
        {
            var collection = container.CollectionCacher.Get(typeof(ICollection<>).MakeGenericType(MakePairType(keyType, valueType)));
            count = collection.Count;
        }
        return new(keyType, valueType, keys, values, items, count);
    }
    #region MethodInfo
    /// <summary>
    /// 获取Keys属性
    /// </summary>
    /// <param name="dictionaryType">字典类型</param>
    /// <param name="propertyName">属性名</param>
    /// <returns></returns>
    public static PropertyInfo GetKeysProperty(Type dictionaryType, string propertyName = "Keys")
        => ReflectionHelper.GetPropery(dictionaryType, property => property.Name == propertyName);
    /// <summary>
    /// 获取Values属性
    /// </summary>
    /// <param name="dictionaryType">字典类型</param>
    /// <returns></returns>
    public static PropertyInfo GetValuesProperty(Type dictionaryType)
        => ReflectionHelper.GetPropery(dictionaryType, property => property.Name == "Values");
    #endregion
    /// <summary>
    /// 键值对类型
    /// </summary>
    /// <param name="keyType"></param>
    /// <param name="elementType"></param>
    /// <returns></returns>
    public static Type MakePairType(Type keyType, Type elementType)
        => typeof(KeyValuePair<,>).MakeGenericType(keyType, elementType);
}
