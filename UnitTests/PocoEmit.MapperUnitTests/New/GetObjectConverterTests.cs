using PocoEmit.Converters;
using PocoEmit.MapperUnitTests.Supports;

namespace PocoEmit.MapperUnitTests.New;

public class GetObjectConverterTests : MapperConvertTestBase
{
    [Fact]
    public void GetObjectConverter_string2int()
    {
        // Act
        var converter = _mapper.GetObjectConverter(typeof(string), typeof(int)) as IObjectConverter;
        // Assert
        Assert.NotNull(converter);
        Assert.Equal(123, converter.ConvertObject("123"));
    }
    [Fact]
    public void GetObjectConverter_int2long()
    {
        // Act
        var converter = _mapper.GetObjectConverter(typeof(int), typeof(long)) as IObjectConverter;
        // Assert
        Assert.NotNull(converter);
        Assert.Equal(123L, converter.ConvertObject(123));
    }

    [Fact]
    public void GetObjectConverter_intNullable()
    {
        // Act
        var converter = _mapper.GetObjectConverter(typeof(int?), typeof(int)) as IObjectConverter;
        // Assert
        Assert.NotNull(converter);
        int? source = 123;
        Assert.Equal(123, converter.ConvertObject(source));
    }
    [Fact]
    public void GetObjectConverter_intSelf()
    {
        // Act
        var converter = _mapper.GetObjectConverter(typeof(int), typeof(int)) as IObjectConverter;
        // Assert
        Assert.NotNull(converter);
        int source = 123;
        Assert.Equal(123, converter.ConvertObject(source));
    }
    [Fact]
    public void GetObjectConverter_User2DTO()
    {
        var converter = _mapper.GetObjectConverter(typeof(User), typeof(UserDTO)) as IObjectConverter;
        Assert.NotNull(converter);
        var source = new User { Id = 1, Name = "Jxj" };
        var result = converter.ConvertObject(source) as UserDTO;
        Assert.NotNull(result);
        Assert.Equal(source.Id, result.Id);
        Assert.Equal(source.Name, result.Name);
    }
    [Fact]
    public void GetObjectConverter_DTO2User()
    {
        var converter = _mapper.GetObjectConverter(typeof(UserDTO), typeof(User)) as IObjectConverter;
        Assert.NotNull(converter);
        var source = new UserDTO { Id = 3, Name = "张三" };
        var result = converter.ConvertObject(source) as User;
        Assert.NotNull(result);
        Assert.Equal(source.Id, result.Id);
        Assert.Equal(source.Name, result.Name);
    }
}
