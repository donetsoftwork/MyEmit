using PocoEmit.MapperUnitTests.Supports;

namespace PocoEmit.MapperUnitTests.New;

public class GenericOperationTests : MapperConvertTestBase
{
    [Fact]
    public void Generic()
    {
        var source = new GenericClass<int, string> { Id = 1, Name = "Jxj" };
        var result = _mapper.Convert<GenericClass<int, string>, GenericClassDTO<int, string>>(source);
        Assert.NotNull(result);
        Assert.Equal(source.Id, result.Id);
        Assert.Equal(source.Name, result.Name);
    }

    [Fact]
    public void Operation()
    {
        User user = new() { Id = 1, Name = "Jxj" };
        var book = new Book { Title = "C# in Depth", Id = 123 };
        var source = new GenericOperation<User, Book>
        {
            Role = user,
            Name = "Buy",
            Thing = book
        };
        var result = _mapper.Convert<GenericOperation<User, Book>, GenericOperationDTO<UserDTO, BookDTO>>(source);
        Assert.NotNull(result);
        Assert.Equal(source.Name, result.Name);
        Assert.NotNull(result.Role);
        Assert.Equal(source.Role.Id, result.Role.Id);
        Assert.Equal(source.Role.Name, result.Role.Name);
        Assert.NotNull(result.Thing);
        Assert.Equal(source.Thing.Id, result.Thing.Id);
        Assert.Equal(source.Thing.Title, result.Thing.Title);
    }



    internal class GenericClass<TId, TName>
    {
        public TId Id { get; set; }
        public TName Name { get; set; }
    }

    internal class GenericClassDTO<TId, TName>
    {
        public TId Id { get; set; }
        public TName Name { get; set; }
    }
}