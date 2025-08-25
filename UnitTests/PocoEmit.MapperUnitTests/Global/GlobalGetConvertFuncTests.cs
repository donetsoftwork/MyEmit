using PocoEmit.Configuration;
using System;

namespace PocoEmit.MapperUnitTests.Global;

public class GlobalGetConvertTests
{
    private IMapper _global = Mapper.Default;
    public GlobalGetConvertTests()
    {
        //_global.UseSystemConvert();
    }
    [Fact]
    public void GetConvert_int2long()
    {
        // Act
        var converter = _global.GetConvertFunc<int, long>();
        // Assert
        Assert.NotNull(converter);
        Assert.Equal(123L, converter(123));

        //Func<int> func = () => 1;
        //object obj = func;
        //Delegate func2 = obj as Delegate;
        //Assert.NotNull(func2);
    }
    [Fact]
    public void GetConvert_int2string()
    {
        // Act
        var converter = _global.GetConvertFunc<int, string>();
        // Assert
        Assert.NotNull(converter);
        Assert.Equal("123", converter(123));
    }
    [Fact]
    public void GetConvert_int2true()
    {
        // Act
        var converter = _global.GetConvertFunc<int, bool>();
        // Assert
        Assert.NotNull(converter);
        int source = 123;
        var result = converter(source);
        Assert.True(result);
        bool expected = Convert.ToBoolean(source);
        Assert.Equal(expected, result);
    }
    [Fact]
    public void GetConvert_int2true2()
    {
        // Act
        var converter = _global.GetConvertFunc<int, bool>();
        // Assert
        Assert.NotNull(converter);
        int source = 0;
        var result = converter(source);
        Assert.False(result);
        bool expected = Convert.ToBoolean(source);
        Assert.Equal(expected, result);
    }
    [Fact]
    public void GetConvert_int2false()
    {
        // Act
        var converter = _global.GetConvertFunc<int, bool>();
        // Assert
        Assert.NotNull(converter);
        int source = -123;
        var result = converter(source);
        Assert.True(result);
        bool expected = Convert.ToBoolean(source);
        Assert.Equal(expected, result);
    }
    [Fact]
    public void GetConvert_intNullable()
    {
        // Act
        var converter = _global.GetConvertFunc<int?, int>();
        // Assert
        Assert.NotNull(converter);
        int? source = 123;
        Assert.Equal(123, converter(source));
    }
    [Fact]
    public void GetConvert_stringNullable()
    {
        // Act
        var converter = _global.GetConvertFunc<string, string?>();
        // Assert
        Assert.NotNull(converter);
        string source = "123";
        string? expected = "123";
        Assert.Equal(expected, converter(source));
    }
    [Fact]
    public void GetConvert_intSelf()
    {
        // Act
        var converter = _global.GetConvertFunc<int, int>();
        // Assert
        Assert.NotNull(converter);
        int source = 123;
        Assert.Equal(123, converter(source));
    }
    [Fact]
    public void GetConvert_string2int()
    {
        // Act
        var converter = _global.GetConvertFunc<string, int>();
        // Assert
        Assert.NotNull(converter);
        Assert.Equal(123, converter("123"));
    }
    [Fact]
    public void GetConvert_string2DateTime()
    {
        // Act
        var converter = _global.GetConvertFunc<string, DateTime>();
        // Assert
        Assert.NotNull(converter);
        var source = "2025-07-21 00:00:00";
        var expected = Convert.ToDateTime(source);
        Assert.Equal(expected, converter(source));
    }
    [Fact]
    public void GetConvert_DateTime2string()
    {
        // Act
        var converter = _global.GetConvertFunc<DateTime, string>();
        // Assert
        Assert.NotNull(converter);
        var source = new DateTime(2025, 7, 21);
        var expected = source.ToString();
        var result = converter(source);
        Assert.Equal(expected, result);
    }
}
