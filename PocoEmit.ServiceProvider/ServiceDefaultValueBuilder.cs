using Microsoft.Extensions.DependencyInjection;
using PocoEmit.Builders;
using PocoEmit.Collections;
using PocoEmit.Configuration;
using PocoEmit.Members;
using PocoEmit.ServiceProvider.Builders;
using PocoEmit.ServiceProvider.Cachers;
using System;
using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Reflection;

namespace PocoEmit.ServiceProvider;

/// <summary>
/// 服务默认值构造器
/// </summary>
public class ServiceDefaultValueBuilder
    : DefaultValueBuilder
    , ICacher<ConstructorParameterMember, IBuilder<Expression>>
    , ICacher<IEmitMemberWriter, IBuilder<Expression>>
{
    #region 配置
    /// <summary>
    /// 服务定位构造器
    /// </summary>
    protected readonly IServiceProviderBuilder _providerBuilder;
    /// <summary>
    /// 服务定位构造器
    /// </summary>
    public IServiceProviderBuilder ProviderBuilder
        => _providerBuilder;
    /// <summary>
    /// 参数默认值构造器
    /// </summary>
    private readonly CacheBase<ConstructorParameterMember, IBuilder<Expression>> _parameterBuilder;
    /// <summary>
    /// 属性默认值构造器
    /// </summary>
    private readonly CacheBase<IEmitMemberWriter, IBuilder<Expression>> _memberBuilder;
    /// <summary>
    /// 属性默认值缓存
    /// </summary>
    private readonly ConcurrentDictionary<IEmitMemberWriter, IBuilder<Expression>> _memberCacher = [];
    /// <summary>
    /// 参数默认值缓存
    /// </summary>
    private readonly ConcurrentDictionary<ConstructorParameterMember, IBuilder<Expression>> _parameterCacher = [];
    #endregion
    /// <summary>
    /// 服务默认值构造器
    /// </summary>
    /// <param name="options"></param>
    /// <param name="providerBuilder"></param>
    public ServiceDefaultValueBuilder(IMapperOptions options, IServiceProviderBuilder providerBuilder)
        : base(options)
    {
        _providerBuilder = providerBuilder;
        _parameterBuilder = CreateParameterBuilder();
        _memberBuilder = CreateMemberBuilder();
    }
    /// <summary>
    /// 参数默认值构造器
    /// </summary>
    /// <returns></returns>
    protected virtual ParameterExpressionCacher CreateParameterBuilder()
        => new(this);
    /// <summary>
    /// 属性默认值缓存
    /// </summary>
    /// <returns></returns>
    protected virtual MemberExpressionCacher CreateMemberBuilder()
        => new(this);
    #region ICacher<ConstructorParameterMember, IBuilder<Expression>>
    bool ICacher<ConstructorParameterMember, IBuilder<Expression>>.ContainsKey(in ConstructorParameterMember key)
        => _parameterCacher.ContainsKey(key);
    bool ICacher<ConstructorParameterMember, IBuilder<Expression>>.TryGetCache(in ConstructorParameterMember key, out IBuilder<Expression> cached)
        => _parameterCacher.TryGetValue(key, out cached);
    void IStore<ConstructorParameterMember, IBuilder<Expression>>.Set(in ConstructorParameterMember key, IBuilder<Expression> value)
        => _parameterCacher[key] = value;
    #endregion
    #region ICacher<IEmitMemberWriter, IBuilder<Expression>>
    bool ICacher<IEmitMemberWriter, IBuilder<Expression>>.ContainsKey(in IEmitMemberWriter key)
        => _memberCacher.ContainsKey(key);
    bool ICacher<IEmitMemberWriter, IBuilder<Expression>>.TryGetCache(in IEmitMemberWriter key, out IBuilder<Expression> cached)
        => _memberCacher.TryGetValue(key, out cached);
    void IStore<IEmitMemberWriter, IBuilder<Expression>>.Set(in IEmitMemberWriter key, IBuilder<Expression> value)
        => _memberCacher[key] = value;
    #endregion
    #region DefaultValueBuilder
    /// <inheritdoc />
    public override IBuilder<Expression> Build(ConstructorParameterMember parameter)
        => _parameterBuilder.Get(parameter);
    /// <inheritdoc />
    public override IBuilder<Expression> Build(IEmitMemberWriter member)
         => _memberBuilder.Get(member);
    #endregion
    #region 基础功能
    /// <summary>
    /// 按特性构造
    /// </summary>
    /// <param name="serviceType"></param>
    /// <param name="keyed"></param>
    /// <returns></returns>
    internal KeyedServiceBuilder BuildFromKeyed(Type serviceType, FromKeyedServicesAttribute keyed)
        => new(_providerBuilder, serviceType, keyed.Key);
    /// <summary>
    /// 按类型获取配置
    /// </summary>
    /// <param name="serviceType"></param>
    /// <param name="serviceBuilder"></param>
    /// <returns></returns>
    public bool TryGetConfig(Type serviceType, out IBuilder<Expression> serviceBuilder)
        => _options.TryGetConfig(serviceType, out serviceBuilder);
    /// <summary>
    /// 按属性获取配置
    /// </summary>
    /// <param name="member"></param>
    /// <param name="serviceBuilder"></param>
    /// <returns></returns>
    public bool TryGetConfig(MemberInfo member, out IBuilder<Expression> serviceBuilder)
        => _options.TryGetConfig(member, out serviceBuilder);
    #endregion
}
