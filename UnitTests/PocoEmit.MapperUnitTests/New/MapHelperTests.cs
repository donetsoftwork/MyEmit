using PocoEmit.MapperUnitTests.Supports;
using System.Net.Http.Headers;

namespace PocoEmit.MapperUnitTests.New;

public class MapHelperTests : MapHelperBaseTests
{
    [Fact]
    public void ConfigureMap()
    {
        IMapper mapper = Mapper.Create();
        mapper.ConfigureMap<AutoUserDTO, User>();
        var source = new AutoUserDTO{ UserId = "222", UserName = "Jxj"  };
        var converter = mapper.GetConverter<AutoUserDTO, User>();
        var result = converter.Convert(source);
        Assert.NotNull(result);
        // ConfigureMap默认把类名作为前缀
        Assert.Equal(source.UserId, result.Id.ToString());
        Assert.Equal(source.UserName, result.Name);
    }
    [Fact]
    public void UseConvertFunc()
    {
        IMapper mapper = Mapper.Create();
        mapper.ConfigureMap<UserDTO, User>()
            .UseConvertFunc(source => new User { Id= source.Id, Name = source.Name });

        var source = new UserDTO { Id = 111, Name = "Jxj" };
        var converter = mapper.GetConverter<UserDTO, User>();
        var result = converter.Convert(source);
        Assert.NotNull(result);
        Assert.Equal(source.Id, result.Id);
        Assert.Equal(source.Name, result.Name);
    }
}
