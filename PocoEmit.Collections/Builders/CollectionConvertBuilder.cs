using PocoEmit.Configuration;
using PocoEmit.Converters;
using System;

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
    protected override IEmitConverter ToArray(Type sourceType, Type destType)
        => _arrayConverter.ToArray(sourceType, destType);
    /// <inheritdoc />
    protected override IEmitConverter ToCollection(Type sourceType, Type destType, bool isInterface)
        => _collectionConverter.ToCollection(sourceType, destType, isInterface);
    /// <inheritdoc />
    protected override IEmitConverter ToDictionary(Type sourceType, Type destType, bool isInterface)
        => _dictionaryConverter.ToDictionary(sourceType, destType, isInterface);
}
