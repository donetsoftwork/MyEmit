using PocoEmit.MapperUnitTests.Supports;

namespace PocoEmit.MapperUnitTests.New;

public class MapHelperSourceTests : MapHelperBaseTests
{
    [Fact]
    public void Ignore()
    {
        IMapper mapper = Mapper.Create();
        mapper.ConfigureMap<User, UserDTO>()
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
        IMapper mapper = Mapper.Create();
        mapper.ConfigureMap<User, UserDTO>()
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
        IMapper mapper = Mapper.Create();
        mapper.ConfigureMap<User, UserCustomDTO>()
            .Source
            .MapTo(nameof(User.Id), nameof(UserCustomDTO.UId))
            .MapTo(nameof(User.Name), nameof(UserCustomDTO.UName));
        var source = new User { Id = 222, Name = "Jxj2" };
        var converter = mapper.GetConverter<User, UserCustomDTO>();
        var result = converter.Convert(source);
        Assert.NotNull(result);
        Assert.Equal(source.Id, result.UId);
        Assert.Equal(source.Name, result.UName);
    }
    [Fact]
    public void MapTo2()
    {
        IMapper mapper = Mapper.Create();
        mapper.ConfigureMap<User, UserCustomDTO>()
            .Source
            .ForMember(nameof(User.Id)).MapTo(nameof(UserCustomDTO.UId))
            .ForMember(nameof(User.Name)).MapTo(nameof(UserCustomDTO.UName));
        var source = new User { Id = 222, Name = "Jxj2" };
        var converter = mapper.GetConverter<User, UserCustomDTO>();
        var result = converter.Convert(source);
        Assert.NotNull(result);
        Assert.Equal(source.Id, result.UId);
        Assert.Equal(source.Name, result.UName);
    }
    [Fact]
    public void Prefix()
    {
        IMapper mapper = Mapper.Create();
        mapper.ConfigureMap<UserCustomDTO, User>()
            .Source
            .AddPrefix("U");
        var source = new UserCustomDTO("Jxj2") { UId = 222 };
        var converter = mapper.GetConverter<UserCustomDTO, User>();
        var result = converter.Convert(source);
        Assert.NotNull(result);
        Assert.Equal(source.UId, result.Id);
        Assert.Equal(source.UName, result.Name);
    }
}
