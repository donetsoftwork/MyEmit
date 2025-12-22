using Hand.Cache;
using Hand.Creational;
using PocoEmit.Configuration;
using PocoEmit.Members;
using System.Linq.Expressions;

namespace PocoEmit.Cachers;

/// <summary>
/// 属性默认值
/// </summary>
/// <param name="provider"></param>
public class MemberDefaultValueCacher(DefaultValueProvider provider)
     : CacheFactoryBase<IEmitMemberWriter, ICreator<Expression>>()
{
    #region 配置
    private readonly DefaultValueProvider _provider = provider;
    /// <summary>
    /// 默认值提供器
    /// </summary>
    public DefaultValueProvider Provider
        => _provider;
    #endregion
    /// <inheritdoc />
    #region CacheFactoryBase<IEmitMemberWriter, ICreator<Expression>>
    protected override ICreator<Expression> CreateNew(in IEmitMemberWriter key)
        => _provider.BuildCore(key);
    #endregion
}