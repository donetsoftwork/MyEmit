using PocoEmit.Members;
using System.Reflection;

namespace PocoEmit.Collections;

/// <summary>
/// 字段读取缓存
/// </summary>
/// <param name="cacher"></param>
public class FieldReaderCacher(ISettings<FieldInfo, FieldReader> cacher)
    : CacheBase<FieldInfo, FieldReader>(cacher)
{
    #region CacheBase<FieldInfo, FieldReader>
    /// <inheritdoc />
    protected override FieldReader CreateNew(FieldInfo key)
        => new(key);
    #endregion
}
