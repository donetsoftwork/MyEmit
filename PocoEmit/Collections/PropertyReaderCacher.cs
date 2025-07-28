using PocoEmit.Members;
using System.Reflection;

namespace PocoEmit.Collections;

/// <summary>
/// 属性读取缓存
/// </summary>
/// <param name="cacher"></param>
public class PropertyReaderCacher(ISettings<PropertyInfo, PropertyReader> cacher)
    : CacheBase<PropertyInfo, PropertyReader>(cacher)
{
    #region CacheBase<FieldInfo, PropertyReader>
    /// <inheritdoc />
    protected override PropertyReader CreateNew(PropertyInfo key)
        => new(key);
    #endregion
}
