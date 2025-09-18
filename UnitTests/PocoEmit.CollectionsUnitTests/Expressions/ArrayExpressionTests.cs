using System;
using System.Linq.Expressions;

namespace PocoEmit.CollectionsUnitTests.Expressions;

public class ArrayExpressionTests
{
    [Fact]
    public void BySys()
    {
        var expression = CreateCopy();
        var func = expression.Compile();
        Assert.NotNull(func);
        string[] source = ["a", "b", "c"];
        var dest = func(source);
        Assert.NotNull(dest);
    }
    [Fact]
    public void ByFast()
    {
        var expression = CreateCopy();
        var code = FastExpressionCompiler.ToCSharpPrinter.ToCSharpString(expression);
        Assert.NotNull(code);
        var func = FastExpressionCompiler.ExpressionCompiler.CompileFast<Func<string[], string[]>>(expression);
        Assert.NotNull(func);
        string[] source = ["a", "b", "c"];
        var dest = func(source);
        Assert.NotNull(dest);
    }
    /// <summary>
    /// 复制数组
    /// </summary>
    /// <param name="source"></param>
    public static Expression<Func<string[], string[]>> CreateCopy()
    {
        // string[] source;
        var source = Expression.Variable(typeof(string[]), "source");
        // string[] dest;
        var dest = Expression.Variable(typeof(string[]), "dest");
        // int length;
        var length = Expression.Variable(typeof(int), "length");
        // int i;
        var i = Expression.Variable(typeof(int), "i");
        var forLabel = Expression.Label("forLabel");

        var body = Expression.Block(
            [dest, length, i],
            // length = source.Length;
            Expression.Assign(length, Expression.ArrayLength(source)),
            Expression.Assign(i, Expression.Constant(0)),
            // dest = new string[length];
            Expression.Assign(dest, Expression.NewArrayBounds(typeof(string), length)),
            Expression.Loop(
                Expression.IfThenElse(
                    // i < length
                    Expression.LessThan(i, length),
                    // dest[i++] = source[i];
                    Expression.Assign(Expression.ArrayAccess(dest, i), Expression.ArrayAccess(source, Expression.PostIncrementAssign(i))
                    ),
                    Expression.Break(forLabel)
                ),
                forLabel),
            // return dest
            dest
        );
        return Expression.Lambda<Func<string[], string[]>>(body, source);
    }
}
