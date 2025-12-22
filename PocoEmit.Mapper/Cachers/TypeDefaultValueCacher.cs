using Hand.Cache;
using Hand.Creational;
using PocoEmit.Configuration;
using System;
using System.Linq.Expressions;

namespace PocoEmit.Cachers;

/// <summary>
/// 类型默认值缓存
/// </summary>
/// <param name="provider"></param>
public class TypeDefaultValueCacher(DefaultValueProvider provider)
     : CacheFactoryBase<Type, ICreator<Expression>>()
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
    #region CacheFactoryBase<Type, ICreator<Expression>>
    protected override ICreator<Expression> CreateNew(in Type key)
        => _provider.BuildCore(key);
    #endregion
}