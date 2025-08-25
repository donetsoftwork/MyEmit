using PocoEmit;
using PocoEmitUnitTests.Supports;

namespace PocoEmitUnitTests.Enums;

public class NullableEnumToString : PocoConvertTestBase
{
    class Source
    {
        public ConsoleColor? Color { get; set; }
    }
    class Destination
    {
        public string Color { get; set; }
    }

    [Fact]
    public void Should_map_with_underlying_type()
    {
        var expression = _poco.BuildConverter<ConsoleColor?, string>();
        var code = FastExpressionCompiler.ToCSharpPrinter.ToCSharpString(expression);
        Assert.NotNull(code);
        ConsoleColor? color = ConsoleColor.Red;
        var destination = _poco.Convert<ConsoleColor?, string>(color);
        Assert.Equal(nameof(ConsoleColor.Red), destination);
    }

    [Fact]
    public void Should_Null()
    {
        ConsoleColor? color = null;
        var destination = _poco.Convert<ConsoleColor?, string>(color);
        Assert.Null(destination);
    }
}
