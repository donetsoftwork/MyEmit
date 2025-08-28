using PocoEmit.MapperUnitTests.Supports;

namespace PocoEmit.MapperUnitTests.New;

public class UseDefaultTest2
{
    [Fact]
    public void UseDefault()
    {
        IMapper mapper = Mapper.Create()
            .UseDefault(Repository);
        var dto = new UserDTO { Id = 1, Name = "Jxj" };
        UserDomain user = mapper.Convert<UserDTO, UserDomain>(dto);
        Assert.NotNull(user);
        Assert.NotNull(user.Repository);
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
    static readonly UserRepository Repository = new();
    class UserRepository
    {
        void Add(UserDomain user) { }
        void Update(UserDomain entity) { }
        void Remove(UserDomain entity) { }
    }
}
