using PocoEmit.Builders;
using PocoEmitUnitTests.Supports;
using System.Linq.Expressions;
using System.Reflection;

namespace PocoEmitUnitTests;

public class EmitHelperTests
{
    public static int Sqrt(int x)
        => x * x;

    [Fact]
    public void TestSqrtMethod()
    {
        int x = 3;
        var sqrtMethod0 = typeof(EmitHelperTests).GetMethod("Sqrt", BindingFlags.Static | BindingFlags.Public, [typeof(int)]);
        Assert.NotNull(sqrtMethod0);
        var expected = sqrtMethod0.Invoke(null, [x]);

        var sqrtMethod = Method<int, int>(x => Sqrt(x));
        Assert.NotNull(sqrtMethod);
        var result = sqrtMethod.Invoke(null, [x]);
        Assert.Equal(expected, result);

        var sqrtMethod2 = EmitHelper.GetActionMethodInfo<int>(x => Sqrt(x));
        Assert.NotNull(sqrtMethod2);
    }
    [Fact]
    public void GetMethodInfo()
    {
        var toStringMethod = EmitHelper.GetMethodInfo<object, string>(obj => obj.ToString());
        Assert.NotNull(toStringMethod);
        var obj = 123;
        var result = toStringMethod.Invoke(obj, null);
        Assert.Equal(obj.ToString(), result);
    }

    [Fact]
    public void GetActionMethodInfo()
    {
        var add = EmitHelper.GetActionMethodInfo((ICollection<int> collection, int item) => collection.Add(item));
        var list = new List<int>();
        add.Invoke(list, [123]);
        Assert.True(list.Count > 0);
    }

    [Fact]
    public void GetPropertyTest()
    { 
        var idProperty = GetProperty<User, int>(u => u.Id);
        Assert.NotNull(idProperty);
        User user = new(1, "Jxj");
        var id = idProperty.GetValue(user, null);
        Assert.NotNull(id);
        Assert.Equal(user.Id, id);
    }
    /// <summary>
    /// 从表达式提取方法
    /// </summary>
    /// <typeparam name="TInstance"></typeparam>
    /// <typeparam name="TMember"></typeparam>
    /// <param name="expression"></param>
    /// <returns></returns>
    public static PropertyInfo? GetProperty<TInstance, TMember>(Expression<Func<TInstance, TMember>> expression)
        => GetBodyMember<PropertyInfo>(expression);
    /// <summary>
    /// 从表达式提取方法
    /// </summary>
    /// <param name="expression"></param>
    /// <returns></returns>
    private static TMember? GetBodyMember<TMember>(LambdaExpression expression)
        where TMember : MemberInfo
    {
        if(expression.Body is MemberExpression memberExpression)
            return memberExpression.Member as TMember;
        return null;
    }

    public static MethodInfo Method<T>(Expression<Func<T>> expression) => GetExpressionBodyMethod(expression);

    public static MethodInfo Method<TType, TResult>(Expression<Func<TType, TResult>> expression) => GetExpressionBodyMethod(expression);

    private static MethodInfo GetExpressionBodyMethod(LambdaExpression expression) => ((MethodCallExpression)expression.Body).Method;
}
