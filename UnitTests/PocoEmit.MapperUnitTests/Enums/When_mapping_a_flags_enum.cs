using FastExpressionCompiler;
using PocoEmit;
using PocoEmit.MapperUnitTests.Supports;
using System.Runtime.Serialization;

namespace PocoEmit.MapperUnitTests.Enums;

public class When_mapping_a_flags_enum : MapperConvertTestBase
{
    [Flags]
    private enum SourceFlags
    {
        None = 0,        
        One = 1,
        Two = 2,
        Four = 4,
        [EnumMember(Value = "SixTeen")]
        Eight = 8
    }

    [Flags]
    private enum DestinationFlags
    {
        None = 0,
        One = 1,
        Two = 2,
        Four = 4,
        Eight = 8,
        SixTeen = 16
    }
    [Fact]
    public void Should_include_all_source_enum_values()
    {
        var expression = _mapper.BuildConverter<SourceFlags, DestinationFlags>();
        var code = ToCSharpPrinter.ToCSharpString(expression);
        Assert.NotNull(code);
        var dest = _mapper.Convert<SourceFlags, DestinationFlags>(SourceFlags.One | SourceFlags.Four);
        Assert.Equal(DestinationFlags.One | DestinationFlags.Four, dest);
    }

    [Fact]
    public void Should_include_nameandenummember()
    {
        var expression = _mapper.BuildConverter<SourceFlags, DestinationFlags>();
        var code = ToCSharpPrinter.ToCSharpString(expression);
        Assert.NotNull(code);
        var dest = _mapper.Convert<SourceFlags, DestinationFlags>(SourceFlags.Eight);
        Assert.Equal(DestinationFlags.Eight | DestinationFlags.SixTeen, dest);
    }
}
