using PocoEmit.Members;
using System.Reflection;

namespace PocoEmit.Collections;

/// <summary>
/// 成员容器
/// </summary>
public partial class MemberContainer
{
    /// <summary>
    /// 字段成员缓存
    /// </summary>
    /// <param name="cacher"></param>
    internal class FieldCacher(ICacher<FieldInfo, FieldAccessor> cacher)
        : CacheBase<FieldInfo, FieldAccessor>(cacher)
    {
        #region CacheBase<MemberInfo, IMemberWriter>
        /// <inheritdoc />
        protected override FieldAccessor CreateNew(FieldInfo key)
            => new(key);
        #endregion
    }

    /// <summary>
    /// 属性成员缓存
    /// </summary>
    /// <param name="cacher"></param>
    internal class PropertyCacher(ICacher<PropertyInfo, PropertyAccessor> cacher)
            : CacheBase<PropertyInfo, PropertyAccessor>(cacher)
    {
        #region CacheBase<MemberInfo, IMemberReader>
        /// <inheritdoc />
        protected override PropertyAccessor CreateNew(PropertyInfo key)
                => new(key);
        #endregion
    }
}