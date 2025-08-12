using PocoEmit.Collections.Converters;
using PocoEmit.Configuration;
using System;
using System.Collections.Generic;

namespace PocoEmit.Converters;

/// <summary>
/// 转化为数组
/// </summary>
/// <param name="options"></param>
public class ConvertToArray(IMapperOptions options)
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
    /// 转化为数组
    /// </summary>
    /// <param name="sourceType"></param>
    /// <param name="destType"></param>
    /// <returns></returns>
    public IEmitConverter ToArray(Type sourceType, Type destType)
    {
        var destElementType = ReflectionHelper.GetElementType(destType);
        var sourceElementType = ReflectionHelper.GetElementType(sourceType);
        if (sourceElementType == null)
            return null;
        var elementConverter = _options.GetEmitConverter(sourceElementType, destElementType);
        if (elementConverter is null)
            return null;
        if (sourceType.IsArray)
            return ArrayToArray(destType, destElementType, elementConverter);
        if (ReflectionHelper.HasGenericType(sourceType, typeof(IList<>)))
            return ListToArray(sourceType, destType, destElementType, elementConverter);
        if (ReflectionHelper.HasGenericType(sourceType, typeof(IDictionary<,>)))
            return DictionaryToArray(sourceType, destType, destElementType, elementConverter);
        if (ReflectionHelper.HasGenericType(sourceType, typeof(IEnumerable<>)))
            return EnumerableToArray(sourceType, destType, destElementType, elementConverter);
        return null;
    }
    /// <summary>
    /// 数组转数组
    /// </summary>
    /// <param name="destType"></param>
    /// <param name="destElementType"></param>
    /// <param name="elementConverter"></param>
    /// <returns></returns>
    public static ArrayConverter ArrayToArray(Type destType, Type destElementType, IEmitConverter elementConverter)
        => new(destType, destElementType, elementConverter);
    /// <summary>
    /// 列表转数组
    /// </summary>
    /// <param name="sourceType"></param>
    /// <param name="destType"></param>
    /// <param name="destElementType"></param>
    /// <param name="elementConverter"></param>
    /// <returns></returns>
    public static IndexArrayConverter ListToArray(Type sourceType, Type destType, Type destElementType, IEmitConverter elementConverter)
    {
        var container = CollectionContainer.Instance;
        var length = container.GetCounter(sourceType);
        var indexReader = container.GetIndexReader(sourceType);
        return new(destType, destElementType, length, indexReader, elementConverter);
    }
    /// <summary>
    /// 迭代转数组
    /// </summary>
    /// <param name="sourceType"></param>
    /// <param name="destType"></param>
    /// <param name="destElementType"></param>
    /// <param name="elementConverter"></param>
    /// <returns></returns>
    public static CollectionArrayConverter EnumerableToArray(Type sourceType, Type destType, Type destElementType, IEmitConverter elementConverter)
    {
        var container = CollectionContainer.Instance;
        var length = container.GetCounter(sourceType);
        if (length is null)
            return null;
        var visitor = container.GetVisitor(sourceType);
        if (visitor is null)
            return null;
        return new CollectionArrayConverter(destType, destElementType, length, visitor, elementConverter);
    }
    /// <summary>
    /// 字典转数组
    /// </summary>
    /// <returns></returns>
    public static IEmitConverter DictionaryToArray(Type sourceType, Type destType, Type destElementType, IEmitConverter elementConverter)
        => EnumerableToArray(sourceType, destType, destElementType, elementConverter);
}
