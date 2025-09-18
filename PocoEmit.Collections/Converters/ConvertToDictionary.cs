using PocoEmit.Collections.Bundles;
using PocoEmit.Configuration;
using PocoEmit.Copies;
using PocoEmit.Dictionaries;
using System;

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
    /// <param name="destBundle"></param>
    /// <returns></returns>
    public DictionaryConverter ToDictionary(Type sourceType, Type destType, DictionaryBundle destBundle)
    {
        IEmitCopier copier = _options.GetCollectionCopier()
            .DictionaryCopier
            .Create(sourceType, destType, destBundle);
        if (copier is null)
            return null;
        return new(sourceType, destType, destBundle.KeyType, destBundle.ValueType, copier);
    }
}
