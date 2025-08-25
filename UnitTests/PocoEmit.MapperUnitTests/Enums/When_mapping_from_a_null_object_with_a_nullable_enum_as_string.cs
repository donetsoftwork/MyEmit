using PocoEmit;
using PocoEmit.MapperUnitTests.Supports;

namespace PocoEmit.MapperUnitTests.Enums;

public class When_mapping_from_a_null_object_with_a_nullable_enum_as_string : MapperConvertTestBase
{
    public enum EnumValues
    {
        One, Two, Three
    }

    public class DestinationClass
    {
        public EnumValues Values1 { get; set; }
        public EnumValues? Values2 { get; set; }
        public EnumValues Values3 { get; set; }
    }

    public class SourceClass
    {
        public string Values1 { get; set; }
        public string Values2 { get; set; }
        public string Values3 { get; set; }
    }

    [Fact]
    public void Should_set_the_target_enum_to_the_default_value()
    {
        var sourceClass = new SourceClass();
        var dest = _mapper.Convert<SourceClass, DestinationClass>(sourceClass);
        Assert.Equal(default, dest.Values1);
    }

    [Fact]
    public void Should_set_the_target_nullable_to_null()
    {
        var sourceClass = new SourceClass();
        var dest = _mapper.Convert<SourceClass, DestinationClass>(sourceClass);
        Assert.Null(dest.Values2);
    }

    [Fact]
    public void Should_set_the_target_empty_to_null()
    {
        var sourceClass = new SourceClass
        {
            Values3 = ""
        };
        var dest = _mapper.Convert<SourceClass, DestinationClass>(sourceClass);
        Assert.Equal(default, dest.Values3);
    }
}
