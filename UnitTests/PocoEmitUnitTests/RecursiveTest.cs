using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PocoEmitUnitTests;

public class RecursiveTest
{
    [Fact]
    public void Test0()
    {
        CallFactorial(0, 1);
    }
    //[Fact]
    //public void Test1()
    //{
    //    CallFactorial(1, 1);
    //}
    //[Fact]
    //public void Test2()
    //{
    //    CallFactorial(2, 4);
    //}
    //[Fact]
    //public void Test3()
    //{
    //    CallFactorial(3, 9);
    //}
    private static void CallFactorial(int num, int expected)
    {
        var func = BuildFactorialExpression()
        .Compile();
        Assert.NotNull(func);
        var result = func(num);
        Assert.Equal(expected, result);
    }
    public static Expression<Func<int, int>> BuildFactorialExpression()
    {        
        // 定义参数
        ParameterExpression numberParam = Expression.Parameter(typeof(int), "number");

        // 递归调用自身的方法表达式
        MethodCallExpression recursiveCall = Expression.Call(null, typeof(RecursiveTest).GetMethod(nameof(Factorial))!, numberParam);

        // 处理基本情况：number == 0 的条件表达式
        BinaryExpression baseCase = Expression.Equal(numberParam, Expression.Constant(0));
        BinaryExpression recursiveCase = Expression.Multiply(numberParam, recursiveCall);
        ConditionalExpression conditional = Expression.Condition(baseCase, Expression.Constant(1), recursiveCase);
        // 创建Lambda表达式
        return Expression.Lambda<Func<int, int>>(conditional, numberParam);
    }

    // 辅助方法，用于在表达式中调用自身（实际上这个方法不会被表达式调用）
    public static int Factorial(int number)
    {
        Console.WriteLine("Factorial:" + number);
        return number * Factorial(number -1);
    }
}
