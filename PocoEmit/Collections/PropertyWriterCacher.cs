using PocoEmit.Members;
using System.Reflection;

namespace PocoEmit.Collections;

/// <summary>
/// 属性写入器缓存
/// </summary>
/// <param name="cacher"></param>
public class PropertyWriterCacher(ISettings<PropertyInfo, PropertyWriter> cacher)
    : CacheBase<PropertyInfo, PropertyWriter>(cacher)
{
    #region CacheBase<PropertyInfo, PropertyWriter>
    /// <inheritdoc />
    protected override PropertyWriter CreateNew(PropertyInfo key)
        => new(key);
    #endregion
}
