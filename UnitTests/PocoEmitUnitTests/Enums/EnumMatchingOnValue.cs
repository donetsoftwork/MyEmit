using PocoEmit;
using PocoEmitUnitTests.Supports;

namespace PocoEmitUnitTests.Enums;

public class EnumMatchingOnValue : PocoConvertTestBase
{
    public enum FirstEnum
    {
        NamedEnum = 1,
        SecondNameEnum = 2
    }

    public enum SecondEnum
    {
        DifferentNamedEnum = 1,
        SecondNameEnum = 2
    }

    [Fact]
    public void Should_notmatch_on_the_name_even_if_values_match()
    {
        var destination = _poco.Convert<FirstEnum, SecondEnum>(FirstEnum.NamedEnum);
        Assert.NotEqual(SecondEnum.DifferentNamedEnum, destination);
    }

    [Fact]
    public void Should_match_on_the_name_even_if_names_match()
    {
        var destination = _poco.Convert<FirstEnum, SecondEnum>(FirstEnum.SecondNameEnum);
        Assert.Equal(SecondEnum.SecondNameEnum, destination);
    }
}