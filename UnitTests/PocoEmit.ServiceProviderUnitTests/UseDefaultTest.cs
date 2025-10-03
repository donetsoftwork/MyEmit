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
        mapper.UseSingleton(provider)
            .UseDefault<UserRepository>();
        var dto = new UserDTO { Id = 1, Name = "Jxj" };
        UserDomain user = mapper.Convert<UserDTO, UserDomain>(dto);
        Assert.NotNull(user);
        Assert.NotNull(user.Repository);
        UserDomain user2 = mapper.Convert<UserDTO, UserDomain>(dto);
        Assert.NotNull(user2);
        Assert.Equal(user.Repository, user2.Repository);
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
        mapper.UseScope(serviceProvider)
             .UseDefault<UserRepository>();
        var dto = new UserDTO { Id = 1, Name = "Jxj" };
        UserDomain user = mapper.Convert<UserDTO, UserDomain>(dto);
        Assert.NotNull(user);
        Assert.NotNull(user.Repository);
        UserDomain user2 = mapper.Convert<UserDTO, UserDomain>(dto);
        Assert.NotNull(user2);
        Assert.Equal(user.Repository, user2.Repository);
    }

    class UserWithServiceProvider
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IServiceProvider ServiceProvider { get; set; }
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
