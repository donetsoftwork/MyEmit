using PocoEmit.MapperUnitTests.Supports;

namespace PocoEmit.MapperUnitTests.New;

public class MapperGetConverterTests : MapperConvertTestBase
{
    [Fact]
    public void GetConverter_int2long()
    {
        // Act
        var converter = _mapper.GetConverter<int, long>();
        // Assert
        Assert.NotNull(converter);
        Assert.Equal(123L, converter.Convert(123));
    }

    [Fact]
    public void GetConverter_intNullable()
    {
        // Act
        var converter = _mapper.GetConverter<int?, int>();
        // Assert
        Assert.NotNull(converter);
        int? source = 123;
        Assert.Equal(123, converter.Convert(source));
    }
    [Fact]
    public void GetConverter_stringNullable()
    {
        // Act
        var converter = _mapper.GetConverter<string, string?>();
        // Assert
        Assert.NotNull(converter);
        string source = "123";
        string? expected = "123";
        Assert.Equal(expected, converter.Convert(source));
    }
    [Fact]
    public void GetConverter_intSelf()
    {
        // Act
        var converter = _mapper.GetConverter<int, int>();
        // Assert
        Assert.NotNull(converter);
        int source = 123;
        Assert.Equal(123, converter.Convert(source));
    }
    #region 多态
    [Fact]
    public void GetConverter_string2DateTime()
    {
        // Act
        var converter = _mapper.GetConverter<string, DateTime>();
        // Assert
        Assert.NotNull(converter);
        var source = "2025/07/21";
        var expected = _timeConverter.Convert(source);
        Assert.Equal(expected, converter.Convert(source));
    }
    [Fact]
    public void GetConverter_DateTime2string()
    {
        // Act
        var converter = _mapper.GetConverter<DateTime, string>();
        // Assert
        Assert.NotNull(converter);
        var source = new DateTime(2025, 7, 21);
        var expected = _timeConverter.Convert(source);
        var result = converter.Convert(source);
        Assert.Equal(expected, result);
        // 不是默认的ToString()格式,已经覆盖实现多态
        Assert.NotEqual(source.ToString(), result);
    }
    #endregion
    [Fact]
    public void GetConverter_Id()
    {
        // Act
        var converter = _mapper.GetConverter<int, MyMapperId>();
        // Assert
        Assert.NotNull(converter);
        int source = 11;
        var result = converter.Convert(source);
        Assert.Equal(source, result.Id);
    }
    [Fact]
    public void GetConverter_MapperId()
    {
        // Act
        var converter = _mapper.GetConverter<MyMapperId, int>();
        // Assert
        Assert.NotNull(converter);
        var source = new MyMapperId(22);
        var result = converter.Convert(source);
        Assert.Equal(source.Id, result);
    }
    [Fact]
    public void GetConverter_User2DTO()
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
    public void GetConverter_DTO2User()
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
    [Fact]
    public void GetConverter_Manager2DTO()
    {
        // Act
        var converter = _mapper.GetConvertFunc<Manager, ManagerDTO>();
        // Assert
        Assert.NotNull(converter);
        var source = new Manager { Id =11, Name = "AAA", User = new User { Id = 1, Name = "Jxj" } };
        var result = converter(source);
        Assert.NotNull(result);
        Assert.Equal(source.Id, result.Id);
        Assert.Equal(source.Name, result.Name);
        Assert.Equal(source.User.Id, result.User.Id);
        Assert.Equal(source.User.Name, result.User.Name);
    }
    [Fact]
    public void GetConverter_DTO2Manager()
    {
        // Act
        var converter = _mapper.GetConvertFunc<ManagerDTO, Manager>();
        // Assert
        Assert.NotNull(converter);
        var source = new ManagerDTO { Id = 22, Name = "BBB", User = new UserDTO { Id = 3, Name = "张三" } };
        var result = converter(source);
        Assert.NotNull(result);
        Assert.Equal(source.Id, result.Id);
        Assert.Equal(source.Name, result.Name);
        Assert.Equal(source.User.Id, result.User.Id);
        Assert.Equal(source.User.Name, result.User.Name);
    }
    [Fact]
    public void GetConverter_Manager2DTOWithNull()
    {
        // Act
        var converter = _mapper.GetConvertFunc<Manager, ManagerDTO>();
        // Assert
        Assert.NotNull(converter);
        var source = new Manager { Id = 11, Name = "AAA"};
        var result = converter(source);
        Assert.NotNull(result);
        Assert.Equal(source.Id, result.Id);
        Assert.Equal(source.Name, result.Name);
    }
    [Fact]
    public void GetConverter_DTO2ManagerWithNull()
    {
        // Act
        var converter = _mapper.GetConvertFunc<ManagerDTO, Manager>();
        // Assert
        Assert.NotNull(converter);
        var source = new ManagerDTO { Id = 22, Name = "BBB" };
        var result = converter(source);
        Assert.NotNull(result);
        Assert.Equal(source.Id, result.Id);
        Assert.Equal(source.Name, result.Name);
    }
    [Fact]
    public void GetConverter_DTO2Manager2()
    {
        // Act
        var converter = _mapper.GetConvertFunc<ManagerDTO, Manager2>();
        // Assert
        Assert.NotNull(converter);
        var source = new ManagerDTO { Id = 22, Name = "BBB", User = new UserDTO { Id = 3, Name = "张三" } };
        var result = converter(source);
        Assert.NotNull(result);
        Assert.Equal(source.Id, result.Id);
        Assert.Equal(source.Name, result.Name);
        Assert.Equal(source.User.Id, result.User.Id);
        Assert.Equal(source.User.Name, result.User.Name);
    }
}
