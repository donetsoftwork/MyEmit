using PocoEmit.MapperUnitTests.Supports;

namespace PocoEmit.MapperUnitTests.New;

public class MapperConvertTests : MapperConvertTestBase
{
    [Fact]
    public void Convert_int2long()
    {
        Assert.Equal(123L, _mapper.Convert<int, long>(123));
    }

    [Fact]
    public void Convert_intNullable()
    {
        int? source = 123;
        Assert.Equal(123, _mapper.Convert<int?, int>(source));
    }
    [Fact]
    public void Convert_stringNullable()
    {
        string source = "123";
        string? expected = "123";
        Assert.Equal(expected, _mapper.Convert<string, string?>(source));
    }
    [Fact]
    public void Convert_intSelf()
    {
        int source = 123;
        Assert.Equal(123, _mapper.Convert<int, int>(source));
    }
    #region 多态
    [Fact]
    public void Convert_string2DateTime()
    {
        var source = "2025/07/21";
        var expected = _timeConverter.Convert(source);
        Assert.Equal(expected, _mapper.Convert<string, DateTime>(source));
    }
    [Fact]
    public void Convert_DateTime2string()
    {
        var source = new DateTime(2025, 7, 21);
        var expected = _timeConverter.Convert(source);
        var result = _mapper.Convert<DateTime, string>(source);
        Assert.Equal(expected, result);
        // 不是默认的ToString()格式,已经覆盖实现多态
        Assert.NotEqual(source.ToString(), result);
    }
    #endregion
    [Fact]
    public void Convert_User2DTO()
    {
        var source = new User { Id = 1, Name = "Jxj" };
        var result = _mapper.Convert<User, UserDTO>(source);
        Assert.NotNull(result);
        Assert.Equal(source.Id, result.Id);
        Assert.Equal(source.Name, result.Name);
    }
    [Fact]
    public void Convert_DTO2User()
    {
        var source = new UserDTO { Id = 3, Name = "张三" };
        var result = _mapper.Convert<UserDTO, User>(source);
        Assert.NotNull(result);
        Assert.Equal(source.Id, result.Id);
        Assert.Equal(source.Name, result.Name);
    }

    [Fact]
    public void Convert_Struct()
    {
        var source = new StructSource { Id = 22, Name = "BBB" };
        var dest = _mapper.Convert<StructSource, StructDest>(source);
        Assert.Equal(source.Id, dest.Id);
        Assert.Equal(source.Name, dest.Name);
    }
}
