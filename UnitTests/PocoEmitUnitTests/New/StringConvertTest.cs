using PocoEmit;

namespace PocoEmitUnitTests.New;

public class StringConvertTest
{
    IPoco _poco;
    public StringConvertTest()
    {
        _poco = Poco.Create();
        _poco.UseSystemConvert();
        _poco.SetStringConvert();
    }

    [Fact]
    public void GetConvertFunc_int2long()
    {
        // Act
        var convertFunc = _poco.GetConvertFunc<int, long>();
        // Assert
        Assert.NotNull(convertFunc);
        Assert.Equal(123L, convertFunc(123));
    }
    [Fact]
    public void GetConvertFunc_int2string()
    {
        // Act
        var convertFunc = _poco.GetConvertFunc<int, string>();
        // Assert
        Assert.NotNull(convertFunc);
        Assert.Equal("123", convertFunc(123));
    }
    [Fact]
    public void GetConvertFunc_intNullable()
    {
        // Act
        var convertFunc = _poco.GetConvertFunc<int?, int>();
        // Assert
        Assert.NotNull(convertFunc);
        int? source = 123;
        Assert.Equal(123, convertFunc(source));
    }
    [Fact]
    public void GetConvertFunc_stringNullable()
    {
        // Act
        var convertFunc = _poco.GetConvertFunc<string, string?>();
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
        var convertFunc = _poco.GetConvertFunc<int, int>();
        // Assert
        Assert.NotNull(convertFunc);
        int source = 123;
        Assert.Equal(123, convertFunc(source));
    }
    [Fact]
    public void GetConvertFunc_string2int()
    {
        // Act
        var convertFunc = _poco.GetConvertFunc<string, int>();
        // Assert
        Assert.NotNull(convertFunc);
        Assert.Equal(123, convertFunc("123"));
    }
    [Fact]
    public void GetConvertFunc_string2DateTime()
    {
        // Act
        var convertFunc = _poco.GetConvertFunc<string, DateTime>();
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
        var convertFunc = _poco.GetConvertFunc<DateTime, string>();
        // Assert
        Assert.NotNull(convertFunc);
        var source = DateTime.Now;
        var expected = source.ToString();
        Assert.Equal(expected, convertFunc(source));
    }
}
