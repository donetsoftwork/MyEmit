using PocoEmit.MapperUnitTests.Supports;

namespace PocoEmit.MapperUnitTests.New;

public class MapHelperDestTests : MapHelperBaseTests
{
    [Fact]
    public void Ignore()
    {
        Mapper mapper = new();
        mapper.Configure<User, UserDTO>()
            .Dest
            .Ignore(nameof(UserDTO.Name));
        var source = new User { Id = 111, Name = "Jxj" };
        var converter = mapper.GetConverter<User, UserDTO>();
        var result = converter.Convert(source);
        Assert.Equal(source.Id, result.Id);
        Assert.NotEqual(source.Name, result.Name);
    }
    [Fact]
    public void Ignore2()
    {
        Mapper mapper = new();
        mapper.Configure<User, UserDTO>()
            .Dest
            .ForMember(nameof(UserDTO.Name)).Ignore();
        var source = new User { Id = 111, Name = "Jxj" };
        var converter = mapper.GetConverter<User, UserDTO>();
        var result = converter.Convert(source);
        Assert.NotNull(result);
        Assert.Equal(source.Id, result.Id);
        Assert.NotEqual(source.Name, result.Name);
    }
    [Fact]
    public void MapFrom()
    {
        Mapper mapper = new();
        mapper.Configure<UserCustomDTO, User>()
            .Dest
            .MapFrom(nameof(User.Id), nameof(UserCustomDTO.UserId))
            .MapFrom(nameof(User.Name), nameof(UserCustomDTO.UserName));
        var source = new UserCustomDTO("Jxj2") { UserId = 222 };
        var converter = mapper.GetConverter<UserCustomDTO, User>();
        var result = converter.Convert(source);
        Assert.NotNull(result);
        Assert.Equal(source.UserId, result.Id);
        Assert.Equal(source.UserName, result.Name);
    }
    [Fact]
    public void MapFrom2()
    {
        Mapper mapper = new();
        mapper.Configure<UserCustomDTO, User>()
            .Dest
            .ForMember(nameof(User.Id)).MapFrom(nameof(UserCustomDTO.UserId))
            .ForMember(nameof(User.Name)).MapFrom(nameof(UserCustomDTO.UserName));
        var source = new UserCustomDTO("Jxj2") { UserId = 222 };
        var converter = mapper.GetConverter<UserCustomDTO, User>();
        var result = converter.Convert(source);
        Assert.NotNull(result);
        Assert.Equal(source.UserId, result.Id);
        Assert.Equal(source.UserName, result.Name);
    }
}