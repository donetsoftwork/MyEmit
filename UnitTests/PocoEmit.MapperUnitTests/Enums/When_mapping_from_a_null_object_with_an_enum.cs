using PocoEmit;
using PocoEmit.MapperUnitTests.Supports;

namespace PocoEmit.MapperUnitTests.Enums;

public class When_mapping_from_a_null_object_with_an_enum : MapperConvertTestBase
{
    public enum EnumValues
    {
        One, Two, Three
    }

    public class DestinationClass
    {
        public EnumValues Values { get; set; }
    }

    public class SourceClass
    {
        public EnumValues Values { get; set; }
    }

    [Fact]
    public void Should_set_the_target_enum_to_the_default_value()
    {
        SourceClass sourceClass = new();
        var dest = _mapper.Convert<SourceClass, DestinationClass>(sourceClass);
        Assert.Equal(default, dest.Values);
    }
}
