using PocoEmit;
using PocoEmitUnitTests.Supports;

namespace PocoEmitUnitTests.New;

public class PocoGetConverterTests : PocoConvertTestBase
{
    [Fact]
    public void GetConverter_int2long()
    {
        // Act
        var converter = _poco.GetConverter<int, long>();
        // Assert
        Assert.NotNull(converter);
        Assert.Equal(123L, converter.Convert(123));
    }

    [Fact]
    public void GetConverter_intNullable()
    {
        // Act
        var converter = _poco.GetConverter<int?, int>();
        // Assert
        Assert.NotNull(converter);
        int? source = 123;
        Assert.Equal(123, converter.Convert(source));
    }
    [Fact]
    public void GetConverter_stringNullable()
    {
        // Act
        var converter = _poco.GetConverter<string, string?>();
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
        var converter = _poco.GetConverter<int, int>();
        // Assert
        Assert.NotNull(converter);
        int source = 123;
        Assert.Equal(123, converter.Convert(source));
    }
    [Fact]
    public void GetConverter_Id()
    {
        // Act
        var converter = _poco.GetConverter<int, PocoId>();
        // Assert
        Assert.NotNull(converter);
        int source = 11;
        var result = converter.Convert(source);
        Assert.Equal(source, result.Id);
    }
    #region 多态
    [Fact]
    public void GetConverter_string2DateTime()
    {
        // Act
        var converter = _poco.GetConverter<string, DateTime>();
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
        var converter = _poco.GetConverter<DateTime, string>();
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
}
