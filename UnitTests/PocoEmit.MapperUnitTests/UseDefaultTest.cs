using PocoEmit.MapperUnitTests.Supports;

namespace PocoEmit.MapperUnitTests;

public class UseDefaultTest
{
    [Fact]
    public void UseDefault()
    {
        var repository = new UserRepository();
        var mapper = Mapper.Create()
            .UseDefault(repository);
        var dto = new UserDTO { Id = 1, Name = "Jxj" };
        UserDomain user = mapper.Convert<UserDTO, UserDomain>(dto);
        Assert.NotNull(user);
        Assert.NotNull(user.Repository);
        Assert.Equal(repository, user.Repository);
        UserDomain user2 = mapper.Convert<UserDTO, UserDomain>(dto);
        Assert.NotNull(user2);
        Assert.Equal(repository, user2.Repository);
    }
    [Fact]
    public void GetCheckAction()
    {
        var mapper = Mapper.Create()
            .UseDefault(new UserRepository());
        var action = mapper.GetCheckAction<UserDomain2>();
        var user = new UserDomain2();
        Assert.Null(user.Repository);
        action(user);
        Assert.NotNull(user.Repository);
    }
    [Fact]
    public void GetCreateFunc()
    {
        var repository = new UserRepository();
        var mapper = Mapper.Create()
            .UseDefault(repository);
        var func = mapper.GetCreateFunc<UserDomain2>();
        var user = func();
        Assert.NotNull(user);
        Assert.NotNull(user.Repository);
        Assert.Equal(repository, user.Repository);
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
    class UserDomain2
    {
        public UserRepository Repository { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        // ...
    }
    class UserRepository
    {
        void Add(UserDomain user) { }
        void Update(UserDomain entity) { }
        void Remove(UserDomain entity) { }
    }
}
