using PocoEmit;
using PocoEmitUnitTests.Supports;

namespace PocoEmitUnitTests.New;

public class AddStaticConverterTests
{
    private Poco _options = new();
    public AddStaticConverterTests()
    {
        // 添加静态转换器
        _options.AddStaticConverter<UserConverter>();
    }
    [Fact]
    public void GetConvertFunc_User2DTO()
    {
        // Act
        var converter = _options.GetConvertFunc<User, UserDTO>();
        // Assert
        Assert.NotNull(converter);
        var source = new User(1, "Jxj");
        var expected = UserConverter.ToDTO(source);
        Assert.Equal(expected, converter(source));
    }
    [Fact]
    public void GetConvertFunc_DTO2User()
    {
        // Act
        var converter = _options.GetConvertFunc<UserDTO, User>();
        // Assert
        Assert.NotNull(converter);
        var source = new UserDTO(1, "Jxj");
        var expected = UserConverter.ToUser(source);
        Assert.Equal(expected, converter(source));
    }
}
