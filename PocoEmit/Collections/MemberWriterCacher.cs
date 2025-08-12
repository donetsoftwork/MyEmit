using PocoEmit.Members;
using System.Reflection;

namespace PocoEmit.Collections;

/// <summary>
/// 成员写入器缓存
/// </summary>
/// <param name="container"></param>
public class MemberWriterCacher(MemberContainer container)
    : CacheBase<MemberInfo, IEmitMemberWriter>(container)
{
    private readonly MemberContainer _container = container;
    #region CacheBase<MemberInfo, IMemberWriter>
    /// <inheritdoc />
    protected override IEmitMemberWriter CreateNew(MemberInfo key)
    {
        if (key is FieldInfo field)
            return _container.Fields.Get(field);
        else if (key is PropertyInfo property && property.CanWrite)
            return _container.Propertes.Get(property);
        return null;
    }
    #endregion
}
