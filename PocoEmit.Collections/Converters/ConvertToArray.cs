using PocoEmit.Collections.Bundles;
using PocoEmit.Collections.Converters;
using PocoEmit.Complexes;
using PocoEmit.Configuration;
using System;

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
    /// <param name="isPrimitive"></param>
    /// <param name="destType"></param>
    /// <returns></returns>
    public IComplexIncludeConverter ToArray(Type sourceType, bool isPrimitive, Type destType)
    {
        //不支持多维数组
        if (destType.GetArrayRank() > 1)
            return null;
        var destElementType = ReflectionHelper.GetElementType(destType);
        if(isPrimitive || PairTypeKey.CheckValueType(sourceType, destElementType))
            return ElementToArray(sourceType, destType, destElementType);
        if (sourceType.IsArray)
        {
            //不支持多维数组
            if (sourceType.GetArrayRank() > 1)
                return null;
            return ArrayToArray(sourceType, destType, destElementType);
        }
        var container = CollectionContainer.Instance;
        if (container.DictionaryCacher.Validate(sourceType))
            return DictionaryToArray(sourceType, container.DictionaryCacher.Get(sourceType), destType, destElementType);
        else if (container.ListCacher.Validate(sourceType))
            return ListToArray(sourceType, container.ListCacher.Get(sourceType), destType, destElementType);
        else if (container.EnumerableCacher.Validate(sourceType))
            return EnumerableToArray(sourceType, container.EnumerableCacher.Get(sourceType), destType, destElementType);
        return null;
    }
    ///// <summary>
    ///// 其他情况
    ///// </summary>
    ///// <param name="sourceType"></param>
    ///// <param name="arrayType"></param>
    ///// <param name="elementType"></param>
    ///// <param name="isObject"></param>
    ///// <returns></returns>
    //public IEmitConverter OthersToArray(Type sourceType, Type arrayType, Type elementType, bool isObject)
    //{
    //    var bundle = _options.MemberCacher.Get(sourceType).EmitReaders;
    //    if (bundle.Count == 0)
    //    {
    //        if (isObject)
    //            return ElementToArray(sourceType, arrayType, elementType);
    //        return null;
    //    }
    //    if (isObject)
    //        return new MemberArrayConverter(_options, arrayType, elementType, bundle, bundle.Keys);
    //    var members = bundle.Values
    //        .Where(m => PairTypeKey.CheckValueType(m.ValueType, elementType))
    //        .Select(m => m.Name)
    //        .ToList();
    //    if (members.Count == 0)
    //        return ElementToArray(sourceType, arrayType, elementType);
    //    return new MemberArrayConverter(_options, arrayType, elementType, bundle, members);
    //}
    /// <summary>
    /// 子元素转数组
    /// </summary>
    /// <param name="sourceType"></param>
    /// <param name="arrayType"></param>
    /// <param name="elementType"></param>
    /// <returns></returns>
    public ArrayInitConverter ElementToArray(Type sourceType, Type arrayType, Type elementType)
    {
        var elementConverter = _options.GetEmitConverter(sourceType, elementType);
        if (elementConverter is null)
            return null;
        return new ArrayInitConverter(arrayType, elementType, elementConverter);
    }

    /// <summary>
    /// 数组转数组
    /// </summary>
    /// <param name="sourceType"></param>
    /// <param name="destType"></param>
    /// <param name="destElementType"></param>
    /// <returns></returns>
    public ArrayConverter ArrayToArray(Type sourceType, Type destType, Type destElementType)
    {
        var sourceElementType = ReflectionHelper.GetElementType(sourceType);
        var elementConverter = _options.GetEmitConverter(sourceElementType, destElementType);
        if (elementConverter is null)
            return null;
        return new(sourceType, sourceElementType, destType, destElementType, elementConverter);
    }
    /// <summary>
    /// 列表转数组
    /// </summary>
    /// <param name="sourceType"></param>
    /// <param name="bundle"></param>
    /// <param name="destType"></param>
    /// <param name="destElementType"></param>
    /// <returns></returns>
    public IndexArrayConverter ListToArray(Type sourceType, ListBundle bundle, Type destType, Type destElementType)
    {
        if (bundle is null)
            return null;
        var sourceElementType = bundle.ElementType;
        var elementConverter = _options.GetEmitConverter(sourceElementType, destElementType);
        if (elementConverter is null)
            return null;
        var container = CollectionContainer.Instance;
        var length = container.CountCacher.Get(sourceType, sourceElementType);
        if (length is null)
            return null;
        var indexReader = container.ReadIndexCacher.Get(sourceType);
        if (indexReader is null)
            return null;
        return new(sourceType, sourceElementType, destType, destElementType, length, indexReader, elementConverter);
    }
    /// <summary>
    /// 迭代转数组
    /// </summary>
    /// <param name="sourceType"></param>
    /// <param name="bundle"></param>
    /// <param name="destType"></param>
    /// <param name="destElementType"></param>
    /// <returns></returns>
    public CollectionArrayConverter EnumerableToArray(Type sourceType, EnumerableBundle bundle, Type destType, Type destElementType)
    {
        if (bundle is null)
            return null;
        var sourceElementType = bundle.ElementType;
        var elementConverter = _options.GetEmitConverter(sourceElementType, destElementType);
        if (elementConverter is null)
            return null;
        var container = CollectionContainer.Instance;
        var length = container.CountCacher.GetByEnumerable(sourceType, sourceElementType);
        if (length is null)
            return null;
        var visitor = container.VisitorCacher.GetByEnumerable(sourceType, bundle);
        if (visitor is null)
            return null;
        return new(sourceType, sourceElementType, destType, destElementType, length, visitor, elementConverter);
    }
    /// <summary>
    /// 字典转数组
    /// </summary>
    /// <returns></returns>
    public CollectionArrayConverter DictionaryToArray(Type sourceType, DictionaryBundle bundle, Type destType, Type destElementType)
    {
        if (bundle is null)
            return null;
        var sourceElementType = bundle.ValueType;
        var elementConverter = _options.GetEmitConverter(sourceElementType, destElementType);
        if (elementConverter is null)
            return null;
        var container = CollectionContainer.Instance;
        var length = container.CountCacher.GetByDictionary(sourceType, bundle);
        if (length is null)
            return null;
        var visitor = container.VisitorCacher.GetByDictionary(sourceType, bundle);
        if (visitor is null)
            return null;
        return new(sourceType, sourceElementType, destType, destElementType, length, visitor, elementConverter);
    }
}
