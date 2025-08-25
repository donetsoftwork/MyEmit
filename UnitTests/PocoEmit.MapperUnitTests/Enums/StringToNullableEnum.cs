using PocoEmit;
using PocoEmit.MapperUnitTests.Supports;

namespace PocoEmit.MapperUnitTests.Enums;

public class StringToNullableEnum : MapperConvertTestBase
{
    class Source
    {
        public string Color { get; set; }
    }

    class Destination
    {
        public ConsoleColor? Color { get; set; }
    }
    [Fact]
    public void Should_map_with_underlying_type()
    {
        var destination = _mapper.Convert<Source, Destination>(new Source());
        Assert.Null(destination.Color);
    }
}
