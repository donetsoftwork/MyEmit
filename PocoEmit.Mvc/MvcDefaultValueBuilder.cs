using PocoEmit.Configuration;
using PocoEmit.ServiceProvider;
using PocoEmit.ServiceProvider.Builders;
using PocoEmit.ServiceProvider.Cachers;
using System;

namespace PocoEmit.Mvc;

/// <summary>
/// Web默认值构造器
/// </summary>
/// <param name="options"></param>
/// <param name="providerBuilder"></param>
public class MvcDefaultValueBuilder(IMapperOptions options, IServiceProviderBuilder providerBuilder)
    : ServiceDefaultValueBuilder(options, providerBuilder)
{
    /// <summary>
    /// 参数默认值构造器
    /// </summary>
    /// <returns></returns>
    protected override ParameterExpressionCacher CreateParameterBuilder()
        => new(this);
    /// <summary>
    /// 属性默认值缓存
    /// </summary>
    /// <returns></returns>
    protected override MemberExpressionCacher CreateMemberBuilder()
        => new(this);
    #region 基础功能
    /// <summary>
    /// 按特性构造
    /// </summary>
    /// <param name="serviceType"></param>
    /// <returns></returns>
    internal ServiceBuilder BuildFromServices(Type serviceType)
        => new(_providerBuilder, serviceType);
    #endregion
}
