using PocoEmit.MapperUnitTests.Supports;

namespace PocoEmit.MapperUnitTests.New;

public class MapperGetConvertFuncTests : MapperConvertTestBase
{
    [Fact]
    public void GetConvertFunc_int2long()
    {
        // Act
        var convertFunc = _mapper.GetConvertFunc<int, long>();
        // Assert
        Assert.NotNull(convertFunc);
        Assert.Equal(123L, convertFunc(123));
    }
  
    [Fact]
    public void GetConvertFunc_intNullable()
    {
        // Act
        var convertFunc = _mapper.GetConvertFunc<int?, int>();
        // Assert
        Assert.NotNull(convertFunc);
        int? source = 123;
        Assert.Equal(123, convertFunc(source));
    }
    [Fact]
    public void GetConvertFunc_stringNullable()
    {
        // Act
        var convertFunc = _mapper.GetConvertFunc<string, string?>();
        // Assert
        Assert.NotNull(convertFunc);
        string source = "123";
        string? expected = "123";
        Assert.Equal(expected, convertFunc(source));
    }
    [Fact]
    public void GetConvertFunc_intSelf()
    {
        // Act
        var convertFunc = _mapper.GetConvertFunc<int, int>();
        // Assert
        Assert.NotNull(convertFunc);
        int source = 123;
        Assert.Equal(123, convertFunc(source));
    }
    #region 多态
    [Fact]
    public void GetConvertFunc_string2DateTime()
    {
        // Act
        var convertFunc = _mapper.GetConvertFunc<string, DateTime>();
        // Assert
        Assert.NotNull(convertFunc);
        var source = "2025/07/21";
        var expected = _timeConverter.Convert(source);
        Assert.Equal(expected, convertFunc(source));
    }
    [Fact]
    public void GetConvertFunc_DateTime2string()
    {
        // Act
        var convertFunc = _mapper.GetConvertFunc<DateTime, string>();
        // Assert
        Assert.NotNull(convertFunc);
        var source = new DateTime(2025, 7, 21);
        var expected = _timeConverter.Convert(source);
        var result = convertFunc(source);
        Assert.Equal(expected, result);
        // 不是默认的ToString()格式,已经覆盖实现多态
        Assert.NotEqual(source.ToString(), result); 
    }
    #endregion
    [Fact]
    public void GetConvertFunc_Id()
    {
        // Act
        var convertFunc = _mapper.GetConvertFunc<int, MyMapperId>();
        Assert.NotNull(convertFunc);
        int source = 11;
        var result = convertFunc(source);
        Assert.Equal(source, result.Id);
    }
    [Fact]
    public void GetConvertFunc_MapperId()
    {
        // Act
        var convertFunc = _mapper.GetConvertFunc<MyMapperId, int>();
        Assert.NotNull(convertFunc);
        var source = new MyMapperId(22);
        var result = convertFunc(source);
        Assert.Equal(source.Id, result);
    }
    [Fact]
    public void GetConvertFunc_User2DTO()
    {
        // Act
        var convertFunc = _mapper.GetConvertFunc<User, UserDTO>();
        // Assert
        Assert.NotNull(convertFunc);
        var source = new User { Id = 1, Name = "Jxj" };
        var result = convertFunc(source);
        Assert.NotNull(result);
        Assert.Equal(source.Id, result.Id);
        Assert.Equal(source.Name, result.Name);
    }
    [Fact]
    public void GetConvertFunc_DTO2User()
    {
        // Act
        var convertFunc = _mapper.GetConvertFunc<UserDTO, User>();
        // Assert
        Assert.NotNull(convertFunc);
        var source = new UserDTO { Id = 3, Name = "张三" };
        var result = convertFunc(source);
        Assert.NotNull(result);
        Assert.Equal(source.Id, result.Id);
        Assert.Equal(source.Name, result.Name);
    }
}
