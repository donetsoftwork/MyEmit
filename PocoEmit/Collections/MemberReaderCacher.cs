using PocoEmit.Members;
using System.Reflection;

namespace PocoEmit.Collections;

/// <summary>
/// 属性读取缓存
/// </summary>
/// <param name="container"></param>
public class MemberReaderCacher(MemberContainer container)
    : CacheBase<MemberInfo, IEmitMemberReader>(container)
{
    #region 配置
    private readonly MemberContainer _container = container;
    /// <summary>
    /// 成员容器
    /// </summary>
    public MemberContainer Container
        => _container;
    #endregion
    #region CacheBase<MemberInfo, IMemberReader>
    /// <inheritdoc />
    protected override IEmitMemberReader CreateNew(MemberInfo key)
    {
        if (key is FieldInfo field)
            return new FieldReader(field);
        else if (key is PropertyInfo property && property.CanRead)
            return new PropertyReader(property);
        //if (key is FieldInfo field)
        //    return _container.FieldReaderCacher.Get(field);
        //else if (key is PropertyInfo property)
        //    return _container.PropertyReaderCacher.Get(property);
        return null;
    }
    #endregion
}
