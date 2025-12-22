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
    ProviderBuilder CreateProvider();
    /// <summary>
    /// IKeyedServiceProvider
    /// </summary>
    ProviderBuilder CreateKeyed();
}
