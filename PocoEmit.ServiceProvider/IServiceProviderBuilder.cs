using PocoEmit.ServiceProvider.Builders;

namespace PocoEmit.ServiceProvider;

/// <summary>
/// 服务定位构造器
/// </summary>
public interface IServiceProviderBuilder
{
    /// <summary>
    /// IServiceProvider
    /// </summary>
    ServiceProviderBuilder CreateProvider();
    /// <summary>
    /// IKeyedServiceProvider
    /// </summary>
    ServiceProviderBuilder CreateKeyed();
}
