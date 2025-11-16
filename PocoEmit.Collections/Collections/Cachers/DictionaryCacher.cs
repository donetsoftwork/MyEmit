using Hand.Cache;
using Hand.Reflection;
using PocoEmit.Collections.Bundles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace PocoEmit.Collections.Cachers;

/// <summary>
/// 字典成员缓存
/// </summary>
/// <param name="container"></param>
internal class DictionaryCacher(CollectionContainer container)
    : CacheFactoryBase<Type, DictionaryBundle>(container)
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
        if (ReflectionType.HasGenericType(key, typeof(IDictionary<,>)))
            return CreateByType(_container, key);
        return null;
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
        if (ReflectionType.HasGenericType(dictionaryType, typeof(IDictionary<,>)))
            return (bundle = Get(dictionaryType)) is not null;
        return TryGetCache(dictionaryType, out bundle) && bundle is not null;
    }
    /// <summary>
    /// 获取字典成员
    /// </summary>
    /// <param name="container"></param>
    /// <param name="dictionaryType"></param>
    /// <returns></returns>
    public static DictionaryBundle CreateByType(CollectionContainer container, Type dictionaryType)
    {
        var dictionaryInterface = ReflectionType.GetGenericCloseInterfaces(dictionaryType, typeof(IDictionary<,>))
            .FirstOrDefault();
        if (dictionaryInterface is null)
            return null;
        var keys = GetKeysProperty(dictionaryType)
            ?? GetKeysProperty(dictionaryInterface);
        var values = GetValuesProperty(dictionaryType)
            ?? GetValuesProperty(dictionaryInterface);
        var arguments = ReflectionType.GetGenericArguments(dictionaryInterface);
        Type keyType = arguments[0];
        Type valueType = arguments[1];
        var items = CollectionContainer.GetItemProperty(dictionaryType, keyType)
            ?? CollectionContainer.GetItemProperty(dictionaryInterface, keyType);
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
        => ReflectionMember.GetPropery(dictionaryType, property => property.Name == propertyName);
    /// <summary>
    /// 获取Values属性
    /// </summary>
    /// <param name="dictionaryType">字典类型</param>
    /// <returns></returns>
    public static PropertyInfo GetValuesProperty(Type dictionaryType)
        => ReflectionMember.GetPropery(dictionaryType, property => property.Name == "Values");
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
