using PocoEmit.Members;
using System.Reflection;

namespace PocoEmit.Collections;

/// <summary>
/// 成员写入器缓存
/// </summary>
/// <param name="cacher"></param>
public class MemberWriterCacher(ICacher<MemberInfo, IEmitMemberWriter> cacher)
    : CacheBase<MemberInfo, IEmitMemberWriter>(cacher)
{
    #region CacheBase<MemberInfo, IMemberWriter>
    /// <inheritdoc />
    protected override IEmitMemberWriter CreateNew(MemberInfo key)
    {
        if (key is FieldInfo field)
            return new FieldWriter(field);
        else if (key is PropertyInfo property && property.CanWrite)
            return new PropertyWriter(property);
        return null;
    }
    #endregion
}
