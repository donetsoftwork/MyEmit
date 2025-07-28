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
        var converter = _options.GetConvertFunc<int, long>();
        // Assert
        Assert.NotNull(converter);
        Assert.Equal(123L, converter(123));
    }
    [Fact]
    public void GetConvertFunc_int2string()
    {
        // Act
        var converter = _options.GetConvertFunc<int, string>();
        // Assert
        Assert.NotNull(converter);
        Assert.Equal("123", converter(123));
    }
    [Fact]
    public void GetConvertFunc_int2true()
    {
        // Act
        var converter = _options.GetConvertFunc<int, bool>();
        // Assert
        Assert.NotNull(converter);
        int source = 123;
        var result = converter(source);
        Assert.True(result);
        bool expected = Convert.ToBoolean(source);
        Assert.Equal(expected, result);
    }
    [Fact]
    public void GetConvertFunc_int2true2()
    {
        // Act
        var converter = _options.GetConvertFunc<int, bool>();
        // Assert
        Assert.NotNull(converter);
        int source = 0;
        var result = converter(source);
        Assert.False(result);
        bool expected = Convert.ToBoolean(source);
        Assert.Equal(expected, result);
    }
    [Fact]
    public void GetConvertFunc_int2false()
    {
        // Act
        var converter = _options.GetConvertFunc<int, bool>();
        // Assert
        Assert.NotNull(converter);
        int source = -123;
        var result = converter(source);
        Assert.True(result);
        bool expected = Convert.ToBoolean(source);
        Assert.Equal(expected, result);
    }
    [Fact]
    public void GetConvertFunc_intNullable()
    {
        // Act
        var converter = _options.GetConvertFunc<int?, int>();
        // Assert
        Assert.NotNull(converter);
        int? source = 123;
        Assert.Equal(123, converter(source));
    }
    [Fact]
    public void GetConvertFunc_stringNullable()
    {
        // Act
        var converter = _options.GetConvertFunc<string, string?>();
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
        var converter = _options.GetConvertFunc<int, int>();
        // Assert
        Assert.NotNull(converter);
        int source = 123;
        Assert.Equal(123, converter(source));
    }
    [Fact]
    public void GetConvertFunc_string2int()
    {
        // Act
        var converter = _options.GetConvertFunc<string, int>();
        // Assert
        Assert.NotNull(converter);
        Assert.Equal(123, converter("123"));
    }
    [Fact]
    public void GetConvertFunc_string2DateTime()
    {
        // Act
        var converter = _options.GetConvertFunc<string, DateTime>();
        // Assert
        Assert.NotNull(converter);
        var source = "2025-07-21 00:00:00";
        var expected = Convert.ToDateTime(source);
        Assert.Equal(expected, converter(source));
    }
    [Fact]
    public void GetConvertFunc_DateTime2string()
    {
        // Act
        var converter = _options.GetConvertFunc<DateTime, string>();
        // Assert
        Assert.NotNull(converter);
        var source = DateTime.Now;
        var expected = source.ToString();
        Assert.Equal(expected, converter(source));
    }
}
