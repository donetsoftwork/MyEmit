using PocoEmit.MapperUnitTests.Supports;

namespace PocoEmit.MapperUnitTests.Enums;

public class DefaultEnumValueToString : MapperConvertTestBase
{
    class Source
    {
        public ConsoleColor Color { get; set; }
    }

    class Destination
    {
        public string Color { get; set; }
    }
    [Fact]
    public void Should_map_ok()
    {
        var destination = _mapper.Convert<Source, Destination>(new Source());
        Assert.Equal("Black", destination.Color);
    }
}
