using PocoEmit;
using PocoEmitUnitTests.Supports;

namespace PocoEmitUnitTests.Enums;

public class DefaultEnumValueToString : PocoConvertTestBase
{
    [Fact]
    public void Should_map_ok()
    {
        var destination = _poco.Convert<ConsoleColor, string>(default);
        Assert.Equal(nameof(ConsoleColor.Black), destination);
    }
}
