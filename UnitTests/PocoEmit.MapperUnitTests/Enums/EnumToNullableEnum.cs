using PocoEmit;
using PocoEmit.MapperUnitTests.Supports;

namespace PocoEmit.MapperUnitTests.Enums;

public class EnumToNullableEnum : MapperConvertTestBase
{
    public enum SomeEnum { Foo, Bar }

    public class Source
    {
        public SomeEnum EnumValue { get; set; }
    }

    public class Destination
    {
        public SomeEnum? EnumValue { get; set; }
    }


    [Fact]
    public void Should_map_enum_to_nullable_enum()
    {
        var source = new Source { EnumValue = SomeEnum.Bar };
        var destination = _mapper.Convert<Source, Destination>(source);
        Assert.Equal(SomeEnum.Bar, destination.EnumValue);
    }
}
