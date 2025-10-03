using PocoEmit.MapperUnitTests.Supports;

namespace PocoEmit.MapperUnitTests.New;

public class UseDefaultTest3
{
    const string tableMain = "Users";
    const string table0 = "User0";
    const string table1 = "User1";

    [Fact]
    public void ValueUseDefault()
    {
        IMapper mapper = Mapper.Create()
            .UseDefault(new UserRepository(tableMain));
        mapper.ConfigureMap<UserDTO, UserDomain>()
            .Dest
            .UseDefault("Repository0", new UserRepository(table0))
            .ForMember("Repository1")
            .UseDefault(new UserRepository(table1));
        var dto = new UserDTO { Id = 1, Name = "Jxj" };
        UserDomain user = mapper.Convert<UserDTO, UserDomain>(dto);
        Check(user);
    }
    [Fact]
    public void ExpressionUseDefault()
    {
        IMapper mapper = Mapper.Create()
            .UseDefault(() => new UserRepository(tableMain));
        mapper.ConfigureMap<UserDTO, UserDomain>()
            .Dest
            .UseDefault("Repository0", () => new UserRepository(table0))
            .ForMember("Repository1")
            .UseDefault(() => new UserRepository(table1));
        var dto = new UserDTO { Id = 1, Name = "Jxj" };
        UserDomain user = mapper.Convert<UserDTO, UserDomain>(dto);
        Check(user);
    }

    private void Check(UserDomain user)
    {
        Assert.NotNull(user);
        var main = user.Main;
        Assert.NotNull(main);
        Assert.Equal(tableMain, main.TableName);
        var repository0 = user.Repository0;
        Assert.NotNull(repository0);
        Assert.Equal(table0, repository0.TableName);
        var repository1 = user.Repository1;
        Assert.NotNull(repository1);
        Assert.Equal(table1, repository1.TableName);
    }


    class UserDomain(int id, string name)
    {
        public UserRepository Main { get; set; }
        public UserRepository Repository0 { get; set; }
        public UserRepository Repository1 { get; set; }
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
