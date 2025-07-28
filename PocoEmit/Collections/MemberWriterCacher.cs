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
    //#region 配置
    //private readonly MemberContainer _container = container;
    ///// <summary>
    ///// 成员容器
    ///// </summary>
    //public MemberContainer Container
    //    => _container;
    //#endregion
    #region CacheBase<MemberInfo, IMemberWriter>
    /// <inheritdoc />
    protected override IEmitMemberWriter CreateNew(MemberInfo key)
    {
        if (key is FieldInfo field)
            return new FieldWriter(field);
        else if (key is PropertyInfo property && property.CanWrite)
            return new PropertyWriter(property);
        //if (key is FieldInfo field)
        //    return _container.FieldWriterCacher.Get(field);
        //else if (key is PropertyInfo property)
        //    return _container.PropertyWriterCacher.Get(property);
        return null;
    }
    #endregion
}
