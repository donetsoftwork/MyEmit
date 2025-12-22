using PocoEmit;
using PocoEmit.Builders;
using System.Linq.Expressions;

namespace PocoEmitBuilderTests;

public class EmitBuilderTests
{
    [Fact]  
    public void Declare()
    {
        var builder = new EmitBuilder();
        var x = builder.Declare<int>("x");
        var x2 = Expression.Add(x, x);
        builder.Add(x2);
        var block = builder.Create(x);
        var lambda = Expression.Lambda<Func<int, int>>(block, x);
        var func = lambda.Compile();
        var result = func(5);
        Assert.Equal(10, result);
    }
    [Fact]
    public void Assign()
    {
        var builder = new EmitBuilder();
        var x = builder.Declare<int>("x");
        var y = builder.Declare<int>("y");
        var sum = builder.Declare<int>("sum");
        builder.Assign(sum, Expression.Add(x, y));
        builder.Add(sum);
        var block = builder.Create(x, y);
        var lambda = Expression.Lambda<Func<int, int, int>>(block, x, y);
        var func = lambda.Compile();
        var result = func(1, 2);
        Assert.Equal(3, result);
    }
    [Fact]
    public void Add()
    {
        var builder = new EmitBuilder();
        var result = builder.Declare<int>("result");
        builder.Assign(result, Expression.Constant(0));
        var x = builder.Declare<int>("x");
        var y = builder.Declare<int>("y");
        var x2 = BuildSquare(builder, x);
        builder.Add(Expression.AddAssign(result, x2));
        var y2 = BuildSquare(builder, y);
        builder.Add(Expression.AddAssign(result, y2));
        var xy = BuildMultiply(builder, x, y);
        var xy2 = BuildAdd(builder, xy, xy);
        builder.Add(Expression.AddAssign(result, xy2));
        builder.Add(result);
        var block = builder.Create(x, y);
        var lambda = Expression.Lambda<Func<int, int, int>>(block, x, y);
        var code = FastExpressionCompiler.ToCSharpPrinter.ToCSharpString(lambda);
        Assert.NotNull(code);
        var func = lambda.Compile();
        var resultValue = func(1, 2);
        Assert.Equal(9, resultValue);
    }
    private static ParameterExpression BuildAdd(EmitBuilder builder, ParameterExpression x, ParameterExpression y)
    {
        var result = builder.Declare<int>("result");
        builder.Assign(result, Expression.Add(x, y));
        return result;
    }
    private static ParameterExpression BuildSquare(EmitBuilder builder, ParameterExpression x)
        => BuildMultiply(builder, x, x);
    private static ParameterExpression BuildMultiply(EmitBuilder builder, ParameterExpression x, ParameterExpression y)
    {
        var result = builder.Declare<int>("result");
        builder.Assign(result, Expression.Multiply(x, y));
        return result;
    }
}
