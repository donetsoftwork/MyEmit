using PocoEmit.MapperUnitTests.Supports;

namespace PocoEmit.MapperUnitTests.New;

public class MapHelperTests : MapHelperBaseTests
{
    [Fact]
    public void ConfigureMap()
    {
        IMapper mapper = Mapper.Create();
        // Emit默认不支持字符串转int,需要扩展
        mapper.UseSystemConvert();
        mapper.ConfigureMap<AutoUserDTO, User>();
        var source = new AutoUserDTO{ UserId = "222", UserName = "Jxj"  };
        var converter = mapper.GetConverter<AutoUserDTO, User>();
        var result = converter.Convert(source);
        Assert.NotNull(result);
        // ConfigureMap默认把类名作为前缀
        Assert.Equal(source.UserId, result.Id.ToString());
        Assert.Equal(source.UserName, result.Name);
    }
}
