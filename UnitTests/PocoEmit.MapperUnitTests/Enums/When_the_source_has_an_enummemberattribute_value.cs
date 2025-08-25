using PocoEmit;
using PocoEmit.MapperUnitTests.Supports;
using System.Runtime.Serialization;

namespace PocoEmit.MapperUnitTests.Enums;

public class When_the_source_has_an_enummemberattribute_value : MapperConvertTestBase
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
        var dest = _mapper.Convert<EnumWithEnumMemberAttribute, string>(EnumWithEnumMemberAttribute.One);
        Assert.Equal("Eins", dest);
    }

    [Fact]
    public void Should_return_the_enum_value()
    {
        var dest = _mapper.Convert<EnumWithEnumMemberAttribute, string>(EnumWithEnumMemberAttribute.Null);
        Assert.Equal(nameof(EnumWithEnumMemberAttribute.Null), dest);
    }

    [Fact]
    public void Should_return_the_defined_enummemberattribute_value_nullable()
    {
        var dest = _mapper.Convert<EnumWithEnumMemberAttribute?, string>(EnumWithEnumMemberAttribute.One);
        Assert.Equal("Eins", dest);
    }

    [Fact]
    public void Should_return_the_enum_value_nullable()
    {
        var dest = _mapper.Convert<EnumWithEnumMemberAttribute?, string>(EnumWithEnumMemberAttribute.Null);
        Assert.Equal(nameof(EnumWithEnumMemberAttribute.Null), dest);
    }

    [Fact]
    public void Should_return_null()
    {
        var dest = _mapper.Convert<EnumWithEnumMemberAttribute?, string>(null);
        Assert.Null(dest);
    }
}
