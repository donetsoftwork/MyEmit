using PocoEmit;
using PocoEmit.MapperUnitTests.Supports;

namespace PocoEmit.MapperUnitTests.Enums;

public class EnumCaseSensitivity : MapperConvertTestBase
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
        //ReflectionHelper.GetMethod(sourceEnumType, nameof(Enum.Equals));
        var expression = _mapper.BuildConverter<FirstEnum, SecondEnum>();
        var code = FastExpressionCompiler.ToCSharpPrinter.ToCSharpString(expression);
        Assert.NotNull(code);
        var destination = _mapper.Convert<FirstEnum, SecondEnum>(FirstEnum.Cat);
        Assert.Equal(SecondEnum.cat, destination);
    }
    [Fact]
    public void Second()
    {
        var destination = _mapper.Convert<SecondEnum, FirstEnum>(SecondEnum.dog);
        Assert.Equal(FirstEnum.Dog, destination);
    }
}