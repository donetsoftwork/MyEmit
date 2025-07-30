using PocoEmit;
using PocoEmitUnitTests.Supports;

namespace PocoEmitUnitTests.New;

public class PocoGetConvertFuncTests : PocoConvertTestBase
{
    [Fact]
    public void GetConvertFunc_int2long()
    {
        // Act
        var convertFun = _poco.GetConvertFunc<int, long>();
        // Assert
        Assert.NotNull(convertFun);
        Assert.Equal(123L, convertFun(123));
    }
  
    [Fact]
    public void GetConvertFunc_intNullable()
    {
        // Act
        var convertFun = _poco.GetConvertFunc<int?, int>();
        // Assert
        Assert.NotNull(convertFun);
        int? source = 123;
        Assert.Equal(123, convertFun(source));
    }
    [Fact]
    public void GetConvertFunc_stringNullable()
    {
        // Act
        var convertFun = _poco.GetConvertFunc<string, string?>();
        // Assert
        Assert.NotNull(convertFun);
        string source = "123";
        string? expected = "123";
        Assert.Equal(expected, convertFun(source));
    }
    [Fact]
    public void GetConvertFunc_intSelf()
    {
        // Act
        var convertFun = _poco.GetConvertFunc<int, int>();
        // Assert
        Assert.NotNull(convertFun);
        int source = 123;
        Assert.Equal(123, convertFun(source));
    }
    [Fact]
    public void GetConvertFunc_Id()
    {
        // Act
        var convertFun = _poco.GetConvertFunc<int, PocoId>();
        // Assert
        Assert.NotNull(convertFun);
        int source = 11;
        var result = convertFun(source);
        Assert.Equal(source, result.Id);
    }
    #region 多态
    [Fact]
    public void GetConvertFunc_string2DateTime()
    {
        // Act
        var converter = _poco.GetConvertFunc<string, DateTime>();
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
        var converter = _poco.GetConvertFunc<DateTime, string>();
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
}
