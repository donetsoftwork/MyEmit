using PocoEmit.Configuration;
using PocoEmit.Copies;
using PocoEmit.Dictionaries;
using System;
using System.Collections.Generic;

namespace PocoEmit.Converters;

/// <summary>
/// 转化为字典
/// </summary>
/// <param name="options"></param>
public class ConvertToDictionary(IMapperOptions options)
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
    /// 转化为字典
    /// </summary>
    /// <param name="sourceType"></param>
    /// <param name="destType"></param>
    /// <param name="isInterface"></param>
    /// <returns></returns>
    public IEmitConverter ToDictionary(Type sourceType, Type destType, bool isInterface)
    {
        IEmitCopier copier = _options.GetCollectionCopier().DictionaryCopier.Create(sourceType, destType, false);
        if (copier is null)
            return null;
        var destArguments = ReflectionHelper.GetGenericArguments(destType);
        if (destArguments.Length != 2)
            return null;
        var keyType = destArguments[0];
        var elementType = destArguments[1];
        if (isInterface)
            return new DictionaryConverter(typeof(Dictionary<,>).MakeGenericType(keyType, elementType), keyType, elementType, copier);
        return new DictionaryConverter(destType, keyType, elementType, copier);
    }
}
