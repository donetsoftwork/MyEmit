using PocoEmit.Members;
using System.Reflection;

namespace PocoEmit.Collections;

/// <summary>
/// 属性读取缓存
/// </summary>
/// <param name="cacher"></param>
public class MemberReaderCacher(ICacher<MemberInfo, IEmitMemberReader> cacher)
    : CacheBase<MemberInfo, IEmitMemberReader>(cacher)
{
    #region CacheBase<MemberInfo, IMemberReader>
    /// <inheritdoc />
    protected override IEmitMemberReader CreateNew(MemberInfo key)
    {
        if (key is FieldInfo field)
            return new FieldReader(field);
        else if (key is PropertyInfo property && property.CanRead)
            return new PropertyReader(property);
        return null;
    }
    #endregion
}
