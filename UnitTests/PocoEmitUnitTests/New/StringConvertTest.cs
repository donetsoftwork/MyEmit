using PocoEmit;

namespace PocoEmitUnitTests.New;

public class StringConvertTest
{
    Poco _options;
    public StringConvertTest()
    {
        _options = new();
        _options.SetStringConvert();
    }

    [Fact]
    public void GetConvertFunc_int2long()
    {
        // Act
        var convertFunc = _options.GetConvertFunc<int, long>();
        // Assert
        Assert.NotNull(convertFunc);
        Assert.Equal(123L, convertFunc(123));
    }
    [Fact]
    public void GetConvertFunc_int2string()
    {
        // Act
        var convertFunc = _options.GetConvertFunc<int, string>();
        // Assert
        Assert.NotNull(convertFunc);
        Assert.Equal("123", convertFunc(123));
    }
    [Fact]
    public void GetConvertFunc_intNullable()
    {
        // Act
        var convertFunc = _options.GetConvertFunc<int?, int>();
        // Assert
        Assert.NotNull(convertFunc);
        int? source = 123;
        Assert.Equal(123, convertFunc(source));
    }
    [Fact]
    public void GetConvertFunc_stringNullable()
    {
        // Act
        var convertFunc = _options.GetConvertFunc<string, string?>();
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
        var convertFunc = _options.GetConvertFunc<int, int>();
        // Assert
        Assert.NotNull(convertFunc);
        int source = 123;
        Assert.Equal(123, convertFunc(source));
    }
    [Fact]
    public void GetConvertFunc_string2int()
    {
        // Act
        var convertFunc = _options.GetConvertFunc<string, int>();
        // Assert
        Assert.NotNull(convertFunc);
        Assert.Equal(123, convertFunc("123"));
    }
    [Fact]
    public void GetConvertFunc_string2DateTime()
    {
        // Act
        var convertFunc = _options.GetConvertFunc<string, DateTime>();
        // Assert
        Assert.NotNull(convertFunc);
        var source = "2025-07-21 00:00:00";
        var expected = Convert.ToDateTime(source);
        Assert.Equal(expected, convertFunc(source));
    }
    [Fact]
    public void GetConvertFunc_DateTime2string()
    {
        // Act
        var convertFunc = _options.GetConvertFunc<DateTime, string>();
        // Assert
        Assert.NotNull(convertFunc);
        var source = DateTime.Now;
        var expected = source.ToString();
        Assert.Equal(expected, convertFunc(source));
    }
}
