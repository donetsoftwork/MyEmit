using PocoEmit;
using PocoEmitUnitTests.Supports;

namespace PocoEmitUnitTests.New;

public class PocoConvertTests : PocoConvertTestBase
{
    [Fact]
    public void Convert_int2long()
    {
        Assert.Equal(123L, _poco.Convert<int, long>(123));
    }

    [Fact]
    public void Convert_intNullable()
    {
        int? source = 123;
        Assert.Equal(123, _poco.Convert<int?, int>(source));
    }
    [Fact]
    public void GetConvert_stringNullable()
    {
        string source = "123";
        string? expected = "123";
        Assert.Equal(expected, _poco.Convert<string, string?>(source));
    }
    [Fact]
    public void Convert_intSelf()
    {
        int source = 123;
        Assert.Equal(123, _poco.Convert<int, int>(source));
    }
    //[Fact]
    //public void Convert_Id()
    //{
    //    int source = 11;
    //    var result = _poco.Convert<int, PocoId>(source);
    //    Assert.Equal(source, result.Id);
    //}
    #region 多态
    [Fact]
    public void GetConvert_string2DateTime()
    {
        var source = "2025/07/21";
        var expected = _timeConverter.Convert(source);
        Assert.Equal(expected, _poco.Convert<string, DateTime>(source));
    }
    [Fact]
    public void GetConvert_DateTime2string()
    {
        var source = new DateTime(2025, 7, 21);
        var expected = _timeConverter.Convert(source);
        var result = _poco.Convert<DateTime, string>(source);
        Assert.Equal(expected, result);
        // 不是默认的ToString()格式,已经覆盖实现多态
        Assert.NotEqual(source.ToString(), result);
    }
    #endregion
}
