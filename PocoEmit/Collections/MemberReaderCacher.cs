using Hand.Cache;
using PocoEmit.Members;
using System.Reflection;

namespace PocoEmit.Collections;

/// <summary>
/// 成员读取缓存
/// </summary>
/// <param name="container"></param>
public class MemberReaderCacher(MemberContainer container)
    : CacheFactoryBase<MemberInfo, IEmitMemberReader>(container)
{
    private readonly MemberContainer _container = container;
    #region CacheBase<MemberInfo, IMemberReader>
    /// <inheritdoc />
    protected override IEmitMemberReader CreateNew(in MemberInfo key)
    {
        if (key is FieldInfo field)
            return _container.Fields.Get(field);
        else if (key is PropertyInfo property && property.CanRead)
            return _container.Propertes.Get(property);
        return null;
    }
    #endregion
}
