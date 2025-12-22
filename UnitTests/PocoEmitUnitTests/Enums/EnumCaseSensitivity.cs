using PocoEmit;
using PocoEmit.Builders;
using PocoEmitUnitTests.Supports;
using System.Linq.Expressions;

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
    [Fact]
    public void Switch()
    {
        var source = Expression.Parameter(typeof(FirstEnum), "source");
        var switchExpr =
            Expression.Switch(
                typeof(SecondEnum),                
                source,
                Expression.Default(typeof(SecondEnum)),
                null,
                [
                    Expression.SwitchCase(
                        Expression.Constant(SecondEnum.dog),
                        Expression.Constant(FirstEnum.Dog)                        
                    ),
                    Expression.SwitchCase(
                        Expression.Constant(SecondEnum.cat),
                        Expression.Constant(FirstEnum.Cat)                       
                    )
                ]
            );
        var lambda = Expression.Lambda<Func<FirstEnum, SecondEnum>>(switchExpr, source);
        var code = FastExpressionCompiler.ToCSharpPrinter.ToCSharpString(lambda);
        Assert.NotNull(code);
        var func = lambda.Compile();
        Assert.NotNull(func);
        var destination = func(FirstEnum.Cat);
        Assert.Equal(SecondEnum.cat, destination);
    }
    [Fact]
    public void Builder()
    {
        var source = Expression.Parameter(typeof(FirstEnum), "source");
        var switchExpr = new SwitchBuilder(source, typeof(SecondEnum))
            .Case(Expression.Constant(SecondEnum.dog), Expression.Constant(FirstEnum.Dog))
            .Case(Expression.Constant(SecondEnum.cat), Expression.Constant(FirstEnum.Cat))
            .Build(Expression.Default(typeof(SecondEnum)));
        var lambda = Expression.Lambda<Func<FirstEnum, SecondEnum>>(switchExpr, source);
        var code = FastExpressionCompiler.ToCSharpPrinter.ToCSharpString(lambda);
        Assert.NotNull(code);
        var func = lambda.Compile();
        Assert.NotNull(func);
        var destination = func(FirstEnum.Cat);
        Assert.Equal(SecondEnum.cat, destination);
    }
}