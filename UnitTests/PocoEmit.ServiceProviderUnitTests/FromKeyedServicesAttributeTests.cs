using Microsoft.Extensions.DependencyInjection;
using PocoEmit.ServiceProviderUnitTests.Supports;

namespace PocoEmit.ServiceProviderUnitTests;

public class FromKeyedServicesAttributeTests : ServiceProviderBase
{
    const string table1 = "User1";
    const string table2 = "User2";
    [Fact]
    public void UseSingleton()
    {
        var provider = BuildProvider(services =>
            {
                services.AddKeyedSingleton(table1, (_, _) => new UserRepository(table1));
                services.AddKeyedSingleton(table2, (_, _) => new UserRepository(table2));
            }
        );
        var mapper = Mapper.Create();
        mapper.UseSingleton(provider);
        var dto = new UserDTO { Id = 1, Name = "Jxj" };
        UserDomain user = mapper.Convert<UserDTO, UserDomain1>(dto);
        Assert.NotNull(user);
        var repository = user.Repository;
        Assert.NotNull(repository);
        Assert.Equal(table1, repository.TableName);
        UserDomain user2 = mapper.Convert<UserDTO, UserDomain2>(dto);
        Assert.NotNull(user2);
        var repository2 = user2.Repository;
        Assert.NotEqual(repository, repository2);
        Assert.Equal(table2, repository2.TableName);
    }
    [Fact]
    public void UseScope()
    {
        const string table1 = "User1";
        const string table2 = "User2";
        var services = new ServiceCollection()
            .AddKeyedScoped(table1, (_, _) => new UserRepository(table1))
            .AddKeyedScoped(table2, (_, _) => new UserRepository(table2));
        var serviceProvider = services.BuildServiceProvider();
        var mapper = Mapper.Create();
        mapper.UseScope(serviceProvider);
        var expression = mapper.BuildConverter<UserDTO, UserDomain1>();
        var code = FastExpressionCompiler.ToCSharpPrinter.ToCSharpString(expression);
        Assert.NotNull(code);
        var dto = new UserDTO { Id = 1, Name = "Jxj" };
        UserDomain user = mapper.Convert<UserDTO, UserDomain1>(dto);
        Assert.NotNull(user);
        Assert.NotNull(user.Repository);
        var repository = user.Repository;
        Assert.NotNull(repository);
        Assert.Equal(table1, repository.TableName);
        UserDomain user2 = mapper.Convert<UserDTO, UserDomain2>(dto);
        Assert.NotNull(user2);
        Assert.NotNull(user2.Repository);
        var repository2 = user2.Repository;
        Assert.NotEqual(repository, repository2);
        Assert.Equal(table2, repository2.TableName);
    }    

    class UserDomain1([FromKeyedServices("User1")]UserRepository repository, int id, string name)
        : UserDomain(repository, id, name)
    {
    }
    class UserDomain2([FromKeyedServices("User2")] UserRepository repository, int id, string name)
        : UserDomain(repository, id, name)
    {
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
    class UserRepository(string tableName)
    {
        private readonly string _tableName = tableName;
        public string TableName 
            => _tableName;
        void Add(UserDomain user) { }
        void Update(UserDomain entity) { }
        void Remove(UserDomain entity) { }
    }
}
