using PocoEmit;
using PocoEmit.Builders;
using PocoEmit.Enums;
using PocoEmitUnitTests.Supports;
using System.Linq.Expressions;

namespace PocoEmitUnitTests.Enums;

public class StringToNullableEnum : PocoConvertTestBase
{
    [Fact]
    public void Should_map_with_underlying_type()
    {
        var expression = _poco.BuildConverter<string, ConsoleColor?>();
        var code = FastExpressionCompiler.ToCSharpPrinter.ToCSharpString(expression);
        Assert.NotNull(code);
        var destination = _poco.Convert<string, ConsoleColor?>(nameof(ConsoleColor.Red));
        Assert.Equal(ConsoleColor.Red, destination);
    }

    [Fact]
    public void Builder()
    {
        var source = Expression.Parameter(typeof(string), "source");
        var switchExpr = new SwitchBuilder(Expression.Constant(true), typeof(ConsoleColor))
            .Case(Expression.Constant(ConsoleColor.Black), EnumFromStringConverter.CreateCondition(source, "Black"))
            .Case(Expression.Constant(ConsoleColor.Red), EnumFromStringConverter.CreateCondition(source, "Red"))
            .Case(Expression.Constant(ConsoleColor.Green), EnumFromStringConverter.CreateCondition(source, "Green"))
            .Build(Expression.Default(typeof(ConsoleColor)));
        var lambda = Expression.Lambda<Func<string, ConsoleColor>>(switchExpr, source);
        var code = FastExpressionCompiler.ToCSharpPrinter.ToCSharpString(lambda);
        Assert.NotNull(code);
        var func = lambda.Compile();
        Assert.NotNull(func);
        var destination = func("green");
        Assert.Equal(ConsoleColor.Green, destination);
    }
    [Fact]
    public void Switch()
    {
        //EnumFromStringConverter
        var source = Expression.Parameter(typeof(string), "source");
        SwitchCase[] cases = [
            Expression.SwitchCase(Expression.Constant(ConsoleColor.Black), EnumFromStringConverter.CreateCondition(source, "Black")),
            Expression.SwitchCase(Expression.Constant(ConsoleColor.Red), EnumFromStringConverter.CreateCondition(source, "Red")),
            Expression.SwitchCase(Expression.Constant(ConsoleColor.Green), EnumFromStringConverter.CreateCondition(source, "Green"))
            ];
         var switchExpr = Expression.Switch(Expression.Constant(true), Expression.Default(typeof(ConsoleColor)), cases);
        var lambda = Expression.Lambda<Func<string, ConsoleColor>>(switchExpr, source);
        var code = FastExpressionCompiler.ToCSharpPrinter.ToCSharpString(lambda);
        Assert.NotNull(code);
        var func = lambda.Compile();
        Assert.NotNull(func);
        var destination = func("green");
        Assert.Equal(ConsoleColor.Green, destination);
    }
}
