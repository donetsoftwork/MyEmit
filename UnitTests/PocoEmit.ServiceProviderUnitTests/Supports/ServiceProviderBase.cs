using Microsoft.Extensions.DependencyInjection;

namespace PocoEmit.ServiceProviderUnitTests.Supports;

public class ServiceProviderBase
{
    protected readonly ServiceCollection _services = new();
    public IServiceProvider BuildProvider(Action<ServiceCollection> configure)
    {
        configure(_services);
        return _services.BuildServiceProvider();
    }
}
