using PocoEmit;
using PocoEmit.Builders;
using PocoEmitBuilderTests.Supports;
using System.Linq.Expressions;

namespace PocoEmitBuilderTests;

public class VariableBuilderTests
{
    [Fact]
    public void New()
    {
        var builder = VariableBuilder.New(0, "sum");
        var x = builder.Declare<int>("x");
        builder.Assign(Expression.Add(x, x));
        var block = builder.Create(x);
        var lambda = Expression.Lambda<Func<int, int>>(block, x);
        var func = lambda.Compile();
        var result = func(5);
        Assert.Equal(10, result);
    }
    [Fact]
    public void Declare()
    {
        var builder = VariableBuilder.New(0, "sum");
        var x = builder.Declare<int>("x");
        var y = builder.Declare<int>("y");
        builder.Assign(Expression.Add(x, y));
        var block = builder.Create(x, y);
        var lambda = Expression.Lambda<Func<int, int, int>>(block, x, y);
        var func = lambda.Compile();
        var result = func(1, 2);
        Assert.Equal(3, result);
    }
    [Fact]
    public void Property()
    {
        var u = Expression.Parameter(typeof(User), "u");
        var builder = new VariableBuilder(u, []);
        var idProperty = builder.Property("Id");
        builder.Add(idProperty);
        var block = builder.Create(u);
        var lambda = Expression.Lambda<Func<User, int>>(block, u);
        var code = FastExpressionCompiler.ToCSharpPrinter.ToCSharpString(lambda);
        Assert.NotNull(code);
        var func = lambda.Compile();
        var user = new User(1, "Jxj");
        var id = func(user);
        Assert.Equal(user.Id, id);
    }
    [Fact]
    public void Add()
    {
        var builder = VariableBuilder.New(0, "result");
        var result = builder.Current;
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
