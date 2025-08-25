using PocoEmit;
using PocoEmitUnitTests.Supports;

namespace PocoEmitUnitTests.Enums;

public class When_mapping_from_a_null_object_with_a_nullable_enum : PocoConvertTestBase
{
    public enum EnumValues
    {
        One, Two, Three
    }

    [Fact]
    public void Should_set_the_target_enum_to_the_default_value()
    {
        EnumValues? enumValues = null;
        var dest = _poco.Convert<EnumValues?, EnumValues>(enumValues);
        Assert.Equal(default, dest);
    }
}
