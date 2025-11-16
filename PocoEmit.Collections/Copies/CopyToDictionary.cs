using Hand.Reflection;
using PocoEmit.Collections.Bundles;
using PocoEmit.Configuration;
using PocoEmit.Dictionaries;
using System;
using System.Reflection;

namespace PocoEmit.Copies;

/// <summary>
/// 复制数据到字典
/// </summary>
/// <param name="options"></param>
public class CopyToDictionary(IMapperOptions options)
{
    #region 配置
    private readonly IMapperOptions _options = options;
    /// <summary>
    /// 配置
    /// </summary>
    public IMapperOptions Options
        => _options;
    #endregion
    /// <summary>
    /// 复制数据到字典
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public IEmitCopier ToDictionary(in PairTypeKey key)
    {
        var destType = key.RightType;
        var destBundle = CollectionContainer.Instance.DictionaryCacher.Get(destType);
        if(destBundle is null)
            return null;
        return Create(key.LeftType, destType, destBundle);
    }
    /// <summary>
    /// 构造复制器
    /// </summary>
    /// <param name="sourceType"></param>
    /// <param name="destType"></param>
    /// <param name="destBundle"></param>
    /// <returns></returns>
    public DictionaryCopier Create(Type sourceType, Type destType, DictionaryBundle destBundle)
    {
        var keyType = destBundle.KeyType;
        var destElementType = destBundle.ValueType;
        if (sourceType.IsArray)
            return ArrayToDictionary(sourceType, destType, keyType, destElementType, destBundle.Items);
        var container = CollectionContainer.Instance;
        if (container.DictionaryCacher.Validate(sourceType))
            return DictionaryToDictionary(sourceType, container.DictionaryCacher.Get(sourceType), destType, keyType, destElementType, destBundle.Items);
        if (container.ListCacher.Validate(sourceType))
            return ListToDictionary(sourceType, destType, keyType, destElementType, destBundle.Items);
        return null;
    }

    /// <summary>
    /// 字典到字典
    /// </summary>
    /// <param name="sourceType"></param>
    /// <param name="bundle"></param>
    /// <param name="destType"></param>
    /// <param name="keyType"></param>
    /// <param name="elementType"></param>
    /// <param name="itemProperty"></param>
    /// <returns></returns>
    public DictionaryCopier DictionaryToDictionary(Type sourceType, DictionaryBundle bundle, Type destType, Type keyType, Type elementType, PropertyInfo itemProperty)
    {
        if(bundle is null)
            return null;
        var keyConverter = _options.GetEmitConverter(bundle.KeyType, keyType);
        if (keyConverter is null)
            return null;
        var elementConverter = _options.GetEmitConverter(bundle.ValueType, elementType);
        if (elementConverter is null)
            return null;
        var sourceVisitor = CollectionContainer.Instance.IndexVisitorCacher.Get(sourceType);
        if (sourceVisitor is null)
            return null;
        return new(destType, keyType, elementType, itemProperty, sourceVisitor, keyConverter, elementConverter, false);
    }
    /// <summary>
    /// 数组到字典
    /// </summary>
    /// <param name="sourceType"></param>
    /// <param name="destType"></param>
    /// <param name="keyType"></param>
    /// <param name="elementType"></param>
    /// <param name="itemProperty"></param>
    /// <returns></returns>
    public DictionaryCopier ArrayToDictionary(Type sourceType, Type destType, Type keyType, Type elementType, PropertyInfo itemProperty)
    {
        var sourceVisitor = CollectionContainer.Instance.IndexVisitorCacher.Get(sourceType);
        if (sourceVisitor is null)
            return null;
        var sourceKeyType = typeof(int);
        var keyConverter = _options.GetEmitConverter(sourceKeyType, keyType);
        if (keyConverter is null)
            return null;
        var sourceElementType = sourceType.GetElementType();
        var elementConverter = _options.GetEmitConverter(sourceElementType, elementType);
        if (elementConverter is null)
            return null;
        return new(destType, keyType, elementType, itemProperty, sourceVisitor, keyConverter, elementConverter, false);
    }
    /// <summary>
    /// 迭代到字典
    /// </summary>
    /// <param name="sourceType"></param>
    /// <param name="destType"></param>
    /// <param name="keyType"></param>
    /// <param name="elementType"></param>
    /// <param name="itemProperty"></param>
    /// <returns></returns>
    public DictionaryCopier ListToDictionary(Type sourceType, Type destType, Type keyType, Type elementType, PropertyInfo itemProperty)
    {
        var sourceKeyType = typeof(int);
        var keyConverter = _options.GetEmitConverter(sourceKeyType, keyType);
        if (keyConverter is null)
            return null;
        var sourceElementType = ReflectionType.GetElementType(sourceType);
        var elementConverter = _options.GetEmitConverter(sourceElementType, elementType);
        if (elementConverter is null)
            return null;
        var sourceVisitor = CollectionContainer.Instance.IndexVisitorCacher.Get(sourceType);
        if (sourceVisitor is null)
            return null;
        return new(destType, keyType, elementType, itemProperty, sourceVisitor, keyConverter, elementConverter, false);
    }
}
