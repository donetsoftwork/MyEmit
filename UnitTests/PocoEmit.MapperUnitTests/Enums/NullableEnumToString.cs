using PocoEmit.MapperUnitTests.Supports;

namespace PocoEmit.MapperUnitTests.Enums;

public class NullableEnumToString : MapperConvertTestBase
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
        Source source = new() { Color = ConsoleColor.Red };
        var destination = _mapper.Convert<Source, Destination>(source);
        Assert.Equal(nameof(ConsoleColor.Red), destination.Color);
    }

    [Fact]
    public void Should_Null()
    {
        var destination = _mapper.Convert<Source, Destination>(new Source());
        Assert.Null(destination.Color);
    }
}
