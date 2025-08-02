using PocoEmit.MapperUnitTests.Supports;

namespace PocoEmit.MapperUnitTests.New;

public class MapHelperSourceTests : MapHelperBaseTests
{
    [Fact]
    public void Ignore()
    {
        Mapper mapper = new();
        mapper.Configure<User, UserDTO>()
            .Source
            .Ignore(nameof(User.Name));
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
            .Source
            .ForMember(nameof(User.Name)).Ignore();
        var source = new User { Id = 111, Name = "Jxj" };
        var converter = mapper.GetConverter<User, UserDTO>();
        var result = converter.Convert(source);
        Assert.NotNull(result);
        Assert.Equal(source.Id, result.Id);
        Assert.NotEqual(source.Name, result.Name);
    }
    [Fact]
    public void MapTo()
    {
        Mapper mapper = new();
        mapper.Configure<User, UserCustomDTO>()
            .Source
            .MapTo(nameof(User.Id), nameof(UserCustomDTO.UserId))
            .MapTo(nameof(User.Name), nameof(UserCustomDTO.UserName));
        var source = new User { Id = 222, Name = "Jxj2" };
        var converter = mapper.GetConverter<User, UserCustomDTO>();
        var result = converter.Convert(source);
        Assert.NotNull(result);
        Assert.Equal(source.Id, result.UserId);
        Assert.Equal(source.Name, result.UserName);
    }
    [Fact]
    public void MapTo2()
    {
        Mapper mapper = new();
        mapper.Configure<User, UserCustomDTO>()
            .Source
            .ForMember(nameof(User.Id)).MapTo(nameof(UserCustomDTO.UserId))
            .ForMember(nameof(User.Name)).MapTo(nameof(UserCustomDTO.UserName));
        var source = new User { Id = 222, Name = "Jxj2" };
        var converter = mapper.GetConverter<User, UserCustomDTO>();
        var result = converter.Convert(source);
        Assert.NotNull(result);
        Assert.Equal(source.Id, result.UserId);
        Assert.Equal(source.Name, result.UserName);
    }
}
