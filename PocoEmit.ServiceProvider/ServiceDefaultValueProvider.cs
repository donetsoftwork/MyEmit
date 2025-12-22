using Hand.Creational;
using Microsoft.Extensions.DependencyInjection;
using PocoEmit.Configuration;
using PocoEmit.Members;
using PocoEmit.ServiceProvider.Builders;
using System;
using System.Linq.Expressions;
using System.Reflection;

namespace PocoEmit.ServiceProvider;

/// <summary>
/// 服务默认值提供器
/// </summary>
/// <param name="options"></param>
/// <param name="providerBuilder"></param>
public class ServiceDefaultValueProvider(IMapperOptions options, IServiceProviderBuilder providerBuilder)
        : DefaultValueProvider(options)
{
    #region 配置
    /// <summary>
    /// 服务定位构造器
    /// </summary>
    protected readonly IServiceProviderBuilder _providerBuilder = providerBuilder;
    /// <summary>
    /// 服务定位构造器
    /// </summary>
    public IServiceProviderBuilder ProviderBuilder
        => _providerBuilder;

    #endregion
    #region DefaultValueBuilder
    /// <inheritdoc />
    internal protected override ICreator<Expression> BuildCore(ConstructorParameterMember parameter)
    {
        var info = parameter.Parameter;
        // 参数支持FromKeyedServicesAttribute
        var keyed = info.GetCustomAttribute<FromKeyedServicesAttribute>();
        if (keyed is null)
            return BuildCore(parameter.ValueType);
        return BuildFromKeyed(parameter.ValueType, keyed);
    }
    /// <inheritdoc />
    internal protected override ICreator<Expression> BuildCore(IEmitMemberWriter member)
    {
        // 按属性配置默认值
        if(_options.TryGetConfig(member.Info, out var serviceBuilder))
            return serviceBuilder;
        return BuildCore(member.ValueType);
    }
    /// <inheritdoc />
    internal protected override ICreator<Expression> BuildCore(Type entityType)
    {
        // 按类型配置默认值
        if (_options.TryGetConfig(entityType, out var config))
            return config;
        return BuildFromService(entityType);
        //var serviceBuilder = BuildFromServices(entityType);
        //if (_options.CheckPrimitive(entityType))
        //    return serviceBuilder;
        //var service = Expression.Parameter(entityType, "service");
        //var builder = new VariableBuilder(service, []);
        //builder.Assign(service, serviceBuilder.Create(builder));
        //TryCreate(builder, entityType);
        //builder.Add(service);
        //return builder;
    }
    #endregion
    ///// <summary>
    ///// 尝试创建实体
    ///// </summary>
    ///// <param name="builder"></param>
    ///// <param name="entityType"></param>
    //private void TryCreate(VariableBuilder builder, Type entityType)
    //{
    //    var builder2 = new EmitBuilder();
    //    var newExpr = Create(builder2, entityType);
    //    if (newExpr is null)
    //        return;
    //    builder2.Assign(builder.Current, newExpr);
    //    builder.IfDefault(builder2.Create());
    //}
    #region 基础功能
    /// <summary>
    /// 按特性构造
    /// </summary>
    /// <param name="serviceType"></param>
    /// <returns></returns>
    internal ServiceBuilder BuildFromService(Type serviceType)
        => new(_providerBuilder, serviceType);
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
    public bool TryGetConfig(Type serviceType, out ICreator<Expression> serviceBuilder)
        => _options.TryGetConfig(serviceType, out serviceBuilder);
    /// <summary>
    /// 按属性获取配置
    /// </summary>
    /// <param name="member"></param>
    /// <param name="serviceBuilder"></param>
    /// <returns></returns>
    public bool TryGetConfig(MemberInfo member, out ICreator<Expression> serviceBuilder)
        => _options.TryGetConfig(member, out serviceBuilder);
    #endregion
}
