using Hand.Reflection;
using PocoEmit.Collections.Bundles;
using PocoEmit.Collections.Converters;
using PocoEmit.Complexes;
using PocoEmit.Configuration;
using PocoEmit.Converters;
using System;
#if (NETSTANDARD1_1 || NETSTANDARD1_3 || NETSTANDARD1_6)
using System.Reflection;
#endif

namespace PocoEmit.Builders;

/// <summary>
/// 集合类型转化构建器
/// </summary>
public class CollectionConvertBuilder(IMapperOptions options)
    : ComplexConvertBuilder(options)
{
    #region 配置
    private readonly ConvertToArray _arrayConverter = new(options);
    private readonly ConvertToCollection _collectionConverter = new(options);
    private readonly ConvertToDictionary _dictionaryConverter = new(options);
    /// <summary>
    /// 数组转化器
    /// </summary>
    public ConvertToArray ArrayConverter 
        => _arrayConverter;
    /// <summary>
    /// 集合转化器
    /// </summary>
    public ConvertToCollection CollectionConverter 
        => _collectionConverter;
    /// <summary>
    /// 字典转化器
    /// </summary>
    public ConvertToDictionary DictionaryConverter 
        => _dictionaryConverter;
    #endregion
    /// <inheritdoc />
    protected override IEmitConverter BuildOther(Type sourceType, Type destType)
    {
        var sourceIsPrimitive = _options.CheckPrimitive(sourceType);
        IEmitConverter converter = null;
        // 基础类型没有成员
        if (!sourceIsPrimitive && TryBuildByMember(sourceType, destType, ref converter))
            return converter;
        var destIsPrimitive = _options.CheckPrimitive(destType);
        // 系统类型转换
        if (sourceIsPrimitive && destIsPrimitive)
            return BuildByConvert(sourceType, destType);
        if (destType.IsArray)
            return ToArray(sourceType, sourceIsPrimitive, destType);
#if (NETSTANDARD1_1 || NETSTANDARD1_3 || NETSTANDARD1_6)
        var isInterface = destType.GetTypeInfo().IsInterface;
#else
        var isInterface = destType.IsInterface;
#endif
        var container = CollectionContainer.Instance;
        if (container.DictionaryCacher.Validate(destType, out var destDictionaryBundle))
        {
            if(sourceIsPrimitive)
                return null;
            return ToDictionary(sourceType, isInterface, destType, destDictionaryBundle);
        }
        if (isInterface)
            return ToInterface(container, sourceType, sourceIsPrimitive, destType);
        else if (container.CollectionCacher.Validate(destType, out var destCollectionBundle))
            return ToCollection(sourceType, sourceIsPrimitive, destType, destCollectionBundle);
        if (!destIsPrimitive && TryBuildByConstructor(sourceType, destType, ref converter))
            return converter;
        if (destIsPrimitive || isInterface)
            return null;
        var key = new PairTypeKey(sourceType, destType);

        var activator = _options.GetEmitActivator(key) ?? CreateDefaultActivator(sourceType, destType);
        if (activator is null)
            return null;
        return new ComplexTypeConverter(_options, key, activator, _options.GetEmitCopier(key));
    }
    /// <summary>
    /// 转化为集合接口
    /// </summary>
    /// <param name="container"></param>
    /// <param name="sourceType"></param>
    /// <param name="sourceIsPrimitive"></param>
    /// <param name="destInterface"></param>
    /// <returns></returns>
    private WrapConverter ToInterface(CollectionContainer container, Type sourceType, bool sourceIsPrimitive, Type destInterface)
    {
        var genericImplType = ConvertToCollection.CheckGenericImplType(destInterface);
        if (genericImplType is not null)
        {
            var destEnumerableBundle = container.EnumerableCacher.Get(destInterface);
            if (destEnumerableBundle is null)
                return null;
            var destElementType = destEnumerableBundle.ElementType;
            var collectionType = genericImplType.MakeGenericType(destElementType);
            var converter = ToCollection(sourceType, sourceIsPrimitive, collectionType, container.CollectionCacher.Get(collectionType));
            return new WrapConverter(_options, sourceType, destInterface, converter);
        }
        return null;
    }
    /// <summary>
    /// 转化为数组
    /// </summary>
    /// <param name="sourceType"></param>
    /// <param name="isPrimitive"></param>
    /// <param name="destType"></param>
    /// <returns></returns>
    private IEmitComplexConverter ToArray(Type sourceType, bool isPrimitive, Type destType)
        => _arrayConverter.ToArray(sourceType, isPrimitive, destType);
    /// <summary>
    /// 转化为集合
    /// </summary>
    /// <param name="sourceType"></param>
    /// <param name="sourceIsPrimitive"></param>
    /// <param name="destType"></param>
    /// <param name="destBundle"></param>
    /// <returns></returns>
    private IEmitComplexConverter ToCollection(Type sourceType, bool sourceIsPrimitive, Type destType, CollectionBundle destBundle)
    {
        if(sourceIsPrimitive)
            return _collectionConverter.ElementToCollection(sourceType, destType, destBundle);
        return _collectionConverter.ToCollection(sourceType, destType, destBundle);
    }
    /// <summary>
    /// 转化为字典
    /// </summary>
    /// <param name="sourceType"></param>
    /// <param name="isInterface"></param>
    /// <param name="destType"></param>
    /// <param name="destBundle"></param>
    /// <returns></returns>
    private IEmitComplexConverter ToDictionary(Type sourceType, bool isInterface, Type destType, DictionaryBundle destBundle)
    {
        return _dictionaryConverter.ToDictionary(sourceType, isInterface, destType, destBundle);
    }
}
