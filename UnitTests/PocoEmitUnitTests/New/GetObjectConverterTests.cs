using PocoEmit;
using PocoEmit.Converters;
using PocoEmitUnitTests.Supports;

namespace PocoEmitUnitTests.New;

public class GetObjectConverterTests : PocoConvertTestBase
{
    [Fact]
    public void GetObjectConverter_string2int()
    {
        // Act
        var converter = _poco.GetObjectConverter(typeof(string), typeof(int)) as IObjectConverter;
        // Assert
        Assert.NotNull(converter);
        Assert.Equal(123, converter.ConvertObject("123"));
    }
    [Fact]
    public void GetObjectConverter_int2long()
    {
        // Act
        var converter = _poco.GetObjectConverter(typeof(int), typeof(long)) as IObjectConverter;
        // Assert
        Assert.NotNull(converter);
        Assert.Equal(123L, converter.ConvertObject(123));
    }

    [Fact]
    public void GetObjectConverter_intNullable()
    {
        // Act
        var converter = _poco.GetObjectConverter(typeof(int?), typeof(int)) as IObjectConverter;
        // Assert
        Assert.NotNull(converter);
        int? source = 123;
        Assert.Equal(123, converter.ConvertObject(source));
    }
    [Fact]
    public void GetObjectConverter_intSelf()
    {
        // Act
        var converter = _poco.GetObjectConverter(typeof(int), typeof(int)) as IObjectConverter;
        // Assert
        Assert.NotNull(converter);
        int source = 123;
        Assert.Equal(123, converter.ConvertObject(source));
    }
}
