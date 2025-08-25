using FastExpressionCompiler;
using PocoEmit;
using PocoEmitUnitTests.Supports;

namespace PocoEmitUnitTests.Enums;

public class When_mapping_a_flags_enum : PocoConvertTestBase
{
    [Flags]
    private enum SourceFlags
    {
        None = 0,
        One = 1,
        Two = 2,
        Four = 4,
        Eight = 8
    }

    [Flags]
    private enum DestinationFlags
    {
        None = 0,
        One = 1,
        Two = 2,
        Four = 4,
        Eight = 8
    }
    [Fact]
    public void Should_include_all_source_enum_values()
    {
        var expression = _poco.BuildConverter<SourceFlags, DestinationFlags>();
        var code = ToCSharpPrinter.ToCSharpString(expression);
        Assert.NotNull(code);
        var dest = _poco.Convert<SourceFlags, DestinationFlags>(SourceFlags.One | SourceFlags.Four | SourceFlags.Eight);
        Assert.Equal(DestinationFlags.One | DestinationFlags.Four | DestinationFlags.Eight, dest);
    }
    [Fact]
    public void MapFromValues()
    {
        var expression = _poco.BuildConverter<int, SourceFlags>();
        var code = FastExpressionCompiler.ToCSharpPrinter.ToCSharpString(expression);
        Assert.NotNull(code);
        var source = 3;
        var dest = _poco.Convert<int, SourceFlags>(source);
        Assert.Equal(SourceFlags.One | SourceFlags.Two, dest);
        var dest2 = (SourceFlags)Enum.ToObject(typeof(SourceFlags), source);
        Assert.Equal(SourceFlags.One | SourceFlags.Two, dest2);
    }
    [Fact]
    public void MapToValues()
    {
        var source = DestinationFlags.One | DestinationFlags.Four;
        var dest = _poco.Convert<DestinationFlags, int>(source);
        Assert.Equal(5, dest);
    }
}
