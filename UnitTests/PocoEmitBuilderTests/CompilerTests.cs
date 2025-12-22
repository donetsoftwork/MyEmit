using PocoEmit.Builders;
using PocoEmitBuilderTests.Supports;
using System.Linq.Expressions;

namespace PocoEmitBuilderTests;

public class CompilerTests
{
    [Fact]
    public void CompileDelegate()
    {
        Expression<Func<User, int>> idExpression = u => u.Id;
        Func<User, int> func = Compiler.Instance.CompileDelegate(idExpression);
        User user = new(1, "Jxj");
        int id = func(user);
        Assert.Equal(user.Id, id);
    }
}
