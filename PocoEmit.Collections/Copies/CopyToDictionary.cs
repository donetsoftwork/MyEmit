using PocoEmit.Configuration;
using PocoEmit.Dictionaries;
using System;
using System.Collections.Generic;

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
    public IEmitCopier ToDictionary(PairTypeKey key)
        => Create(key.LeftType, key.RightType, true);
    /// <summary>
    /// 构造复制器
    /// </summary>
    /// <param name="sourceType"></param>
    /// <param name="destType"></param>
    /// <param name="clear"></param>
    /// <returns></returns>
    public DictionaryCopier Create(Type sourceType, Type destType, bool clear = true)
    {
        var destArguments = ReflectionHelper.GetGenericArguments(destType);
        if (destArguments.Length != 2)
            return null;
        var keyType = destArguments[0];
        var elementType = destArguments[1];
        if (sourceType.IsArray)
            return ArrayToDictionary(sourceType, destType, keyType, elementType, clear);
        if (ReflectionHelper.HasGenericType(sourceType, typeof(IDictionary<,>)))
            return DictionaryToDictionary(sourceType, destType, keyType, elementType, clear);
        if (ReflectionHelper.HasGenericType(sourceType, typeof(IEnumerable<>)))
            return ListToDictionary(sourceType, destType, keyType, elementType, clear);
        return null;
    }
    /// <summary>
    /// 字典到字典
    /// </summary>
    /// <param name="sourceType"></param>
    /// <param name="destType"></param>
    /// <param name="keyType"></param>
    /// <param name="elementType"></param>
    /// <param name="clear"></param>
    /// <returns></returns>
    public DictionaryCopier DictionaryToDictionary(Type sourceType, Type destType, Type keyType, Type elementType, bool clear = true)
    {
        var sourceArguments = ReflectionHelper.GetGenericArguments(sourceType);
        if (sourceArguments.Length != 2)
            return null;
        var keyConverter = _options.GetEmitConverter(sourceArguments[0], keyType);
        if (keyConverter is null)
            return null;
        var elementConverter = _options.GetEmitConverter(sourceArguments[1], elementType);
        if (elementConverter is null)
            return null;
        var sourceVisitor = CollectionContainer.Instance.GetIndexVisitor(sourceType);
        if (sourceVisitor is null)
            return null;
        return new(destType, keyType, elementType, sourceVisitor, keyConverter, elementConverter, clear);
    }
    /// <summary>
    /// 数组到字典
    /// </summary>
    /// <param name="sourceType"></param>
    /// <param name="destType"></param>
    /// <param name="keyType"></param>
    /// <param name="elementType"></param>
    /// <param name="clear"></param>
    /// <returns></returns>
    public DictionaryCopier ArrayToDictionary(Type sourceType, Type destType, Type keyType, Type elementType, bool clear = true)
    {
        var sourceVisitor = CollectionContainer.Instance.GetIndexVisitor(sourceType);
        if (sourceVisitor is null)
            return null;
        var sourceKeyType = typeof(int);
        var keyConverter = _options.GetEmitConverter(sourceKeyType, keyType);
        if (keyConverter is null)
            return null;
        var sourcetElementType = ReflectionHelper.GetElementType(sourceType);
        var elementConverter = _options.GetEmitConverter(sourcetElementType, elementType);
        if (elementConverter is null)
            return null;
        return new(destType, keyType, elementType, sourceVisitor, keyConverter, elementConverter, clear);
    }
    /// <summary>
    /// 列表到字典
    /// </summary>
    /// <param name="sourceType"></param>
    /// <param name="destType"></param>
    /// <param name="keyType"></param>
    /// <param name="elementType"></param>
    /// <param name="clear"></param>
    /// <returns></returns>
    public DictionaryCopier ListToDictionary(Type sourceType, Type destType, Type keyType, Type elementType, bool clear = true)
    {
        var sourceKeyType = typeof(int);
        var keyConverter = _options.GetEmitConverter(sourceKeyType, keyType);
        if (keyConverter is null)
            return null;
        var sourcetElementType = ReflectionHelper.GetElementType(sourceType);
        var elementConverter = _options.GetEmitConverter(sourcetElementType, elementType);
        if (elementConverter is null)
            return null;
        var sourceVisitor = CollectionContainer.Instance.GetIndexVisitor(sourceType);
        if (sourceVisitor is null)
            return null;
        return new(destType, keyType, elementType, sourceVisitor, keyConverter, elementConverter, clear);
    }
}
