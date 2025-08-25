using PocoEmit;
using PocoEmitUnitTests.Supports;

namespace PocoEmitUnitTests.Enums;

public class EnumCaseSensitivity : PocoConvertTestBase
{
    public enum FirstEnum
    {
        Dog,
        Cat
    }

    public enum SecondEnum
    {
        cat,
        dog
    }

    [Fact]
    public void First()
    {
        var expression = _poco.BuildConverter<FirstEnum, SecondEnum>();
        var code = FastExpressionCompiler.ToCSharpPrinter.ToCSharpString(expression);
        Assert.NotNull(code);
        var destination = _poco.Convert<FirstEnum, SecondEnum>(FirstEnum.Cat);
        Assert.Equal(SecondEnum.cat, destination);
    }
    [Fact]
    public void Second()
    {
        var destination = _poco.Convert<SecondEnum, FirstEnum>(SecondEnum.dog);
        Assert.Equal(FirstEnum.Dog, destination);
    }
}