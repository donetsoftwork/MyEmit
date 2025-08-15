using PocoEmit.Configuration;
using PocoEmit.Copies;

namespace PocoEmit.Builders;

/// <summary>
/// 集合复制
/// </summary>
public class CollectionCopierBuilder(IMapperOptions options)
    : CopierBuilder(options)
{
    #region 配置
    private readonly CopyToCollection _collectionCopier = new(options);
    private readonly CopyToDictionary _dictionaryCopier = new(options);
    /// <summary>
    /// 集合复制器
    /// </summary>
    public CopyToCollection CollectionCopier 
        => _collectionCopier;
    /// <summary>
    /// 字典复制器
    /// </summary>
    public CopyToDictionary DictionaryCopier
        => _dictionaryCopier;
    #endregion
    /// <inheritdoc />
    public override IEmitCopier ToCollection(PairTypeKey key)
        => _collectionCopier.ToCollection(key);
    /// <inheritdoc />
    public override IEmitCopier ToDictionary(PairTypeKey key)
        => _dictionaryCopier.ToDictionary(key);
}
