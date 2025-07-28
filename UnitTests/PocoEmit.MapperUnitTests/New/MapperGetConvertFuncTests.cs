using PocoEmit.MapperUnitTests.Supports;

namespace PocoEmit.MapperUnitTests.New;

public class MapperGetConvertFuncTests : MapperConvertTestBase
{
    [Fact]
    public void GetConvertFunc_int2long()
    {
        // Act
        var converter = _mapper.GetConvertFunc<int, long>();
        // Assert
        Assert.NotNull(converter);
        Assert.Equal(123L, converter(123));
    }
  
    [Fact]
    public void GetConvertFunc_intNullable()
    {
        // Act
        var converter = _mapper.GetConvertFunc<int?, int>();
        // Assert
        Assert.NotNull(converter);
        int? source = 123;
        Assert.Equal(123, converter(source));
    }
    [Fact]
    public void GetConvertFunc_stringNullable()
    {
        // Act
        var converter = _mapper.GetConvertFunc<string, string?>();
        // Assert
        Assert.NotNull(converter);
        string source = "123";
        string? expected = "123";
        Assert.Equal(expected, converter(source));
    }
    [Fact]
    public void GetConvertFunc_intSelf()
    {
        // Act
        var converter = _mapper.GetConvertFunc<int, int>();
        // Assert
        Assert.NotNull(converter);
        int source = 123;
        Assert.Equal(123, converter(source));
    }
    #region 多态
    [Fact]
    public void GetConvertFunc_string2DateTime()
    {
        // Act
        var converter = _mapper.GetConvertFunc<string, DateTime>();
        // Assert
        Assert.NotNull(converter);
        var source = "2025/07/21";
        var expected = _timeConverter.Convert(source);
        Assert.Equal(expected, converter(source));
    }
    [Fact]
    public void GetConvertFunc_DateTime2string()
    {
        // Act
        var converter = _mapper.GetConvertFunc<DateTime, string>();
        // Assert
        Assert.NotNull(converter);
        var source = new DateTime(2025, 7, 21);
        var expected = _timeConverter.Convert(source);
        var result = converter(source);
        Assert.Equal(expected, result);
        // 不是默认的ToString()格式,已经覆盖实现多态
        Assert.NotEqual(source.ToString(), result); 
    }
    #endregion
    [Fact]
    public void GetConvertFunc_User2DTO()
    {
        // Act
        var converter = _mapper.GetConvertFunc<User, UserDTO>();
        // Assert
        Assert.NotNull(converter);
        var source = new User { Id = 1, Name = "Jxj" };
        var result = converter(source);
        Assert.NotNull(result);
        Assert.Equal(source.Id, result.Id);
        Assert.Equal(source.Name, result.Name);
    }
    [Fact]
    public void GetConvertFunc_DTO2User()
    {
        // Act
        var converter = _mapper.GetConvertFunc<UserDTO, User>();
        // Assert
        Assert.NotNull(converter);
        var source = new UserDTO { Id = 3, Name = "张三" };
        var result = converter(source);
        Assert.NotNull(result);
        Assert.Equal(source.Id, result.Id);
        Assert.Equal(source.Name, result.Name);
    }
}
