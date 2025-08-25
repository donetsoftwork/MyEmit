using PocoEmit;
using PocoEmitUnitTests.Supports;

namespace PocoEmitUnitTests.Enums;

public class StringToNullableEnum : PocoConvertTestBase
{
    [Fact]
    public void Should_map_with_underlying_type()
    {
        var destination = _poco.Convert<string, ConsoleColor?>(nameof(ConsoleColor.Red));
        Assert.Equal(ConsoleColor.Red, destination);
    }
}
