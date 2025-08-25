using PocoEmit;
using PocoEmitUnitTests.Supports;

namespace PocoEmitUnitTests.Enums;

public class EnumToNullableEnum : PocoConvertTestBase
{
    public enum SomeEnum { Foo, Bar }

    [Fact]
    public void Should_map_enum_to_nullable_enum()
    {
        var destination = _poco.Convert<SomeEnum, SomeEnum?>(SomeEnum.Bar);
        Assert.Equal(SomeEnum.Bar, destination);
    }
}
