using Hand.Cache;
using Hand.Creational;
using PocoEmit.Configuration;
using PocoEmit.Members;
using System.Linq.Expressions;

namespace PocoEmit.Cachers;

/// <summary>
/// 参数默认值
/// </summary>
/// <param name="provider"></param>
public class ParameterDefaultValueCacher(DefaultValueProvider provider)
     : CacheFactoryBase<ConstructorParameterMember, ICreator<Expression>>()
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
    #region CacheFactoryBase<ConstructorParameterMember, ICreator<Expression>>
    protected override ICreator<Expression> CreateNew(in ConstructorParameterMember key)
        => _provider.BuildCore(key);
    #endregion
}