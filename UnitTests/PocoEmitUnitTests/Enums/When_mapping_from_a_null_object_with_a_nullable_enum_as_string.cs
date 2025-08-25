using PocoEmit;
using PocoEmitUnitTests.Supports;

namespace PocoEmitUnitTests.Enums;

public class When_mapping_from_a_null_object_with_a_nullable_enum_as_string : PocoConvertTestBase
{
    public enum EnumValues
    {
        One, Two, Three
    }
    public class SourceClass
    {
        public string Values1 { get; set; }
        public string Values2 { get; set; }
        public string Values3 { get; set; }
    }

    public class DestinationClass
    {
        public EnumValues Values1 { get; set; }
        public EnumValues? Values2 { get; set; }
        public EnumValues Values3 { get; set; }
    }

    [Fact]
    public void Should_set_the_target_enum_to_the_default_value()
    {
        var sourceClass = new SourceClass();
        var dest = _poco.Convert<string, EnumValues>(sourceClass.Values1);
        Assert.Equal(default, dest);
    }

    [Fact]
    public void Should_set_the_target_nullable_to_null()
    {
        var sourceClass = new SourceClass();
        var dest = _poco.Convert<string, EnumValues?>(sourceClass.Values2);
        Assert.Null(dest);
    }

    [Fact]
    public void Should_set_the_target_empty_to_null()
    {
        var sourceClass = new SourceClass
        {
            Values3 = ""
        };
        var dest = _poco.Convert<string, EnumValues>(sourceClass.Values3);
        Assert.Equal(default, dest);
    }
}
