using PocoEmit;
using PocoEmit.MapperUnitTests.Supports;
using System.Runtime.Serialization;

namespace PocoEmit.MapperUnitTests.Enums;

public class When_the_target_has_an_enummemberattribute_value : MapperConvertTestBase
{
    public enum EnumWithEnumMemberAttribute
    {
        Null,
        [EnumMember(Value = "Eins")]
        One
    }
    [Fact]
    public void Should_return_the_enum_from_defined_enummemberattribute_value()
    {
        var dest = _mapper.Convert<string, EnumWithEnumMemberAttribute>("Eins");
        Assert.Equal(EnumWithEnumMemberAttribute.One, dest);
    }

    [Fact]
    public void Should_return_the_enum_from_undefined_enummemberattribute_value()
    {
        var dest = _mapper.Convert<string, EnumWithEnumMemberAttribute>("Null");
        Assert.Equal(EnumWithEnumMemberAttribute.Null, dest);
    }

    [Fact]
    public void Should_return_the_nullable_enum_from_defined_enummemberattribute_value()
    {
        var dest = _mapper.Convert<string, EnumWithEnumMemberAttribute?>("Eins");
        Assert.Equal(EnumWithEnumMemberAttribute.One, dest);
    }

    [Fact]
    public void Should_return_the_enum_from_undefined_enummemberattribute_value_mixedcase()
    {
        var dest = _mapper.Convert<string, EnumWithEnumMemberAttribute>("NuLl");
        Assert.Equal(EnumWithEnumMemberAttribute.Null, dest);
    }

    [Fact]
    public void Should_return_the_enum_from_defined_enummemberattribute_value_mixedcase()
    {
        var expression = _mapper.BuildConverter<string, EnumWithEnumMemberAttribute?>();
        var code = FastExpressionCompiler.ToCSharpPrinter.ToCSharpString(expression);
        Assert.NotNull(code);
        var dest = _mapper.Convert<string, EnumWithEnumMemberAttribute?>("eInS");
        Assert.Equal(EnumWithEnumMemberAttribute.One, dest);
    }

    [Fact]
    public void Should_return_the_nullable_enum_from_null_value()
    {
        var dest = _mapper.Convert<string, EnumWithEnumMemberAttribute?>(null);
        Assert.Null(dest);
    }

    [Fact]
    public void Should_return_the_nullable_enum_from_undefined_enummemberattribute_value()
    {
        var dest = _mapper.Convert<string, EnumWithEnumMemberAttribute?>("Null");
        Assert.Equal(EnumWithEnumMemberAttribute.Null, dest);
    }
}
