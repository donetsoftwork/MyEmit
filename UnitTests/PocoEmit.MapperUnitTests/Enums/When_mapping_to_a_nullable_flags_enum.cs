using PocoEmit.MapperUnitTests.Supports;

namespace PocoEmit.MapperUnitTests.Enums;

public class When_mapping_to_a_nullable_flags_enum : MapperConvertTestBase
{

    [Flags]
    public enum EnumValues
    {
        One, Two = 2, Three = 4
    }

    public class SourceClass
    {
        public EnumValues Values { get; set; }
    }

    public class DestinationClass
    {
        public EnumValues? Values { get; set; }
    }

    [Fact]
    public void Should_set_the_target_enum_to_the_default_value()
    {
        var values = EnumValues.Two | EnumValues.Three;
        var dest = _mapper.Convert<SourceClass, DestinationClass>(new SourceClass { Values = values });
        Assert.Equal(values, dest.Values);
    }
}