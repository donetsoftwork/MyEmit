using PocoEmit.MapperUnitTests.Supports;

namespace PocoEmit.MapperUnitTests.Enums;

public class EnumMatchingOnValue : MapperConvertTestBase
{
    public class FirstClass
    {
        public FirstEnum EnumValue { get; set; }
    }

    public enum FirstEnum
    {
        NamedEnum = 1,
        SecondNameEnum = 2
    }

    public class SecondClass
    {
        public SecondEnum EnumValue { get; set; }
    }

    public enum SecondEnum
    {
        DifferentNamedEnum = 1,
        SecondNameEnum = 2
    }

    [Fact]
    public void Should_notmatch_on_the_name_even_if_values_match()
    {
        var source = new FirstClass
        {
            EnumValue = FirstEnum.NamedEnum
        };
        var destination = _mapper.Convert<FirstClass, SecondClass>(source);
        Assert.NotEqual(SecondEnum.DifferentNamedEnum, destination.EnumValue);
    }

    [Fact]
    public void Should_match_on_the_name_even_if_names_match()
    {
        var source = new FirstClass
        {
            EnumValue = FirstEnum.SecondNameEnum
        };
        var destination = _mapper.Convert<FirstClass, SecondClass>(source);
        Assert.Equal(SecondEnum.SecondNameEnum, destination.EnumValue);
    }
}