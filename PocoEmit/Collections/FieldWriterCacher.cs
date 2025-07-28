using PocoEmit.Members;
using System.Reflection;

namespace PocoEmit.Collections;

/// <summary>
/// 字段写入器缓存
/// </summary>
/// <param name="cacher"></param>
public class FieldWriterCacher(ISettings<FieldInfo, FieldWriter> cacher)
    : CacheBase<FieldInfo, FieldWriter>(cacher)
{
    #region CacheBase<FieldInfo, FieldWriter>
    /// <inheritdoc />
    protected override FieldWriter CreateNew(FieldInfo key)
        => new(key);
    #endregion
}
