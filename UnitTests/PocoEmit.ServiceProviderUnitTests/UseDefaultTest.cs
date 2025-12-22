using Microsoft.Extensions.DependencyInjection;
using PocoEmit.ServiceProviderUnitTests.Supports;

namespace PocoEmit.ServiceProviderUnitTests;

public class UseDefaultTest : ServiceProviderBase
{
    [Fact]
    public void UseSingleton()
    {
        var provider = BuildProvider(services => services.AddSingleton<UserRepository>());
        var mapper = Mapper.Create();
        mapper.UseSingleton(provider);
        var dto = new UserDTO { Id = 1, Name = "Jxj" };
        UserDomain user = mapper.Convert<UserDTO, UserDomain>(dto);
        Assert.NotNull(user);
        Assert.NotNull(user.Repository);
        UserDomain user2 = mapper.Convert<UserDTO, UserDomain>(dto);
        Assert.NotNull(user2);
        Assert.Equal(user.Repository, user2.Repository);
    }
    [Fact]
    public void GetService()
    {
        var services = new ServiceCollection();
        var provider = services.BuildServiceProvider();
        var scope = provider.GetService<IServiceProvider>();
        Assert.NotNull(scope);
        Assert.NotEqual(provider, scope);
        var user = scope.GetService<UserWithServiceProvider>();
        Assert.Null(user);
        var user2 = scope.GetService<UserWithServiceProvider2>();
        Assert.Null(user2);
        // 值类型未注册时，抛出异常
        Assert.Throws<NullReferenceException>(() => scope.GetService<int>());
    }
    [Fact]
    public void GetService2()
    {
        var services = new ServiceCollection()
            .AddSingleton<UserRepository>()
            .AddTransient<UserWithServiceProvider>()
            .AddTransient<UserWithServiceProvider2>();
        var provider = services.BuildServiceProvider();
        var scope = provider.GetService<IServiceProvider>();
        Assert.NotNull(scope);
        Assert.NotEqual(provider, scope);
        var user = scope.GetService<UserWithServiceProvider>();
        Assert.NotNull(user);
        var user2 = scope.GetService<UserWithServiceProvider2>();
        Assert.NotNull(user2);
        // 值类型未注册时，抛出异常
        Assert.Throws<NullReferenceException>(() => scope.GetService<int>());
    }
    [Fact]
    public void Circular()
    {
        var services = new ServiceCollection()
            .AddTransient<CircularService>();
        var provider = services.BuildServiceProvider();
        // 循环引用抛出异常
        Assert.Throws<InvalidOperationException>(() => provider.GetService<CircularService>());
    }
    [Fact]
    public void GetCheckAction()
    {
        var services = new ServiceCollection();
        var provider = services.BuildServiceProvider();
        var mapper = Mapper.Create();
        mapper.UseSingleton(provider);
        var action = mapper.GetCheckAction<UserWithServiceProvider>();
        var user = new UserWithServiceProvider() { Id = 1 };
        Assert.Null(user.ServiceProvider);
        action(user);
        // ServiceProvider支持参数注入
        Assert.NotNull(user.ServiceProvider);
        _ = mapper.GetCheckAction<UserWithServiceProvider>();
    }
    [Fact]
    public void GetCreateFunc()
    {
        var services = new ServiceCollection()
            .AddTransient<UserWithServiceProvider>();
        var provider = services.BuildServiceProvider();
        var mapper = Mapper.Create();
        mapper.UseSingleton(provider);
        var func = mapper.GetCreateFunc<UserWithServiceProvider>();
        var user = func();
        Assert.NotNull(user);
        // ServiceProvider不支持属性注入
        Assert.Null(user.ServiceProvider);
    }
    [Fact]
    public void ServiceProviderUseSingleton()
    {
        var services = new ServiceCollection();
        var serviceProvider = services.BuildServiceProvider();
        var mapper = Mapper.Create();
        mapper.UseSingleton(serviceProvider);
        var dto = new UserDTO { Id = 1, Name = "Jxj" };
        UserWithServiceProvider user = mapper.Convert<UserDTO, UserWithServiceProvider>(dto);
        Assert.NotNull(user);
        Assert.NotNull(user.ServiceProvider);
    }
    [Fact]
    public void UseScope()
    {
        var services = new ServiceCollection()
            .AddScoped<UserRepository>();
        var serviceProvider = services.BuildServiceProvider();
        var mapper = Mapper.Create();
        mapper.UseScope(serviceProvider);
        var dto = new UserDTO { Id = 1, Name = "Jxj" };
        UserDomain user = mapper.Convert<UserDTO, UserDomain>(dto);
        Assert.NotNull(user);
        Assert.NotNull(user.Repository);
        UserDomain user2 = mapper.Convert<UserDTO, UserDomain>(dto);
        Assert.NotNull(user2);
        Assert.Equal(user.Repository, user2.Repository);
    }
    class CircularService(CircularService parent)
    {
        private readonly CircularService _parent = parent;
        private CircularService Parent 
            => _parent;
    }
    class UserWithServiceProvider
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IServiceProvider ServiceProvider { get; set; }
    }
    class UserWithServiceProvider2(IServiceProvider serviceProvider)
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IServiceProvider ServiceProvider { get; set; } = serviceProvider;
    }
    class UserDomain(UserRepository repository, int id, string name)
    {
        private readonly UserRepository _repository = repository;
        public UserRepository Repository
            => _repository;
        public int Id { get; } = id;
        public string Name { get; } = name;
        // ...
    }
    class UserRepository
    {
        void Add(UserDomain user) { }
        void Update(UserDomain entity) { }
        void Remove(UserDomain entity) { }
    }
}
