using PocoEmit;
using PocoEmitUnitTests.Supports;
using System.Runtime.Serialization;

namespace PocoEmitUnitTests.Enums;

public class When_the_source_has_an_enummemberattribute_value : PocoConvertTestBase
{
    public enum EnumWithEnumMemberAttribute
    {
        Null,
        [EnumMember(Value = "Eins")]
        One
    }

    [Fact]
    public void Should_return_the_defined_enummemberattribute_value()
    {
        var dest = _poco.Convert<EnumWithEnumMemberAttribute, string>(EnumWithEnumMemberAttribute.One);
        Assert.Equal("Eins", dest);
    }

    [Fact]
    public void Should_return_the_enum_value()
    {
        var dest = _poco.Convert<EnumWithEnumMemberAttribute, string>(EnumWithEnumMemberAttribute.Null);
        Assert.Equal("Null", dest);
    }

    [Fact]
    public void Should_return_the_defined_enummemberattribute_value_nullable()
    {
        var dest = _poco.Convert<EnumWithEnumMemberAttribute?, string>(EnumWithEnumMemberAttribute.One);
        Assert.Equal("Eins", dest);
    }

    [Fact]
    public void Should_return_the_enum_value_nullable()
    {
        var dest = _poco.Convert<EnumWithEnumMemberAttribute?, string>(EnumWithEnumMemberAttribute.Null);
        Assert.Equal("Null", dest);
    }

    [Fact]
    public void Should_return_null()
    {
        var dest = _poco.Convert<EnumWithEnumMemberAttribute?, string>(null);
        Assert.Null(dest);
    }
}
