using PocoEmit.Members;
using System;
using System.Linq.Expressions;

namespace PocoEmit.Builders;

/// <summary>
/// 编译器
/// </summary>
public class Compiler
{
    #region CompileDelegate
    /// <summary>
    /// 编译委托
    /// </summary>
    /// <param name="lambda"></param>
    /// <returns></returns>
    public virtual Delegate CompileDelegate(LambdaExpression lambda)
        => lambda.Compile();
    /// <summary>
    /// 编译委托
    /// </summary>
    /// <typeparam name="TDelegate"></typeparam>
    /// <param name="lambda"></param>
    /// <returns></returns>
    public virtual TDelegate CompileDelegate<TDelegate>(Expression<TDelegate> lambda)
        where TDelegate : Delegate
        => lambda.Compile();
    #endregion
    //#region CompileFunc
    ///// <summary>
    ///// 编译Func
    ///// </summary>
    ///// <typeparam name="TArgument"></typeparam>
    ///// <typeparam name="TResult"></typeparam>
    ///// <param name="func"></param>
    ///// <returns></returns>
    //public virtual Func<TArgument, TResult> CompileFunc<TArgument, TResult>(Expression<Func<TArgument, TResult>> func)
    //    => func.Compile();
    //#endregion
    //#region CompileAction
    ///// <summary>
    ///// 编译Action
    ///// </summary>
    ///// <typeparam name="T1"></typeparam>
    ///// <typeparam name="T2"></typeparam>
    ///// <param name="action"></param>
    ///// <returns></returns>
    //public virtual Action<T1, T2> CompileAction<T1, T2>(Expression<Action<T1, T2>> action)
    //    => action.Compile();
    //#endregion
    ///// <summary>
    ///// 调用
    ///// </summary>
    ///// <param name="lambda"></param>
    ///// <param name="arguments"></param>
    ///// <returns></returns>
    //public virtual Expression Call(LambdaExpression lambda, params Expression[] arguments)
    //{
    //    var expression = lambda.Body;
    //    // 通过参数替换调用
    //    var replaceVisitor = ReplaceVisitor.Create(true, lambda.Parameters, arguments);
    //    if (replaceVisitor is null)
    //        return expression;
    //    var expression2 = ReBuildVisitor.Empty.Visit(replaceVisitor.Visit(expression));
    //    if (expression == expression2)
    //    {
    //        return expression;
    //    }
    //    return expression2;
    //}
    #region Compile
    /// <summary>
    /// 编译成员读取器
    /// </summary>
    /// <typeparam name="TInstance"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="reader"></param>
    /// <returns></returns>
    public static Func<TInstance, TValue> CompileFunc<TInstance, TValue>(IEmitMemberReader reader)
        => _instance.CompileDelegate(reader.Build<TInstance, TValue>());
    /// <summary>
    /// 编译成员写入器
    /// </summary>
    /// <typeparam name="TInstance"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="writer"></param>
    /// <returns></returns>
    public static Action<TInstance, TValue> CompileAction<TInstance, TValue>(IEmitMemberWriter writer)
        => _instance.CompileDelegate(writer.Build<TInstance, TValue>());
    #endregion
    #region Instance
    /// <summary>
    /// 编译器 
    /// </summary>
    internal static Compiler _instance = new();
    /// <summary>
    /// 编译器 
    /// </summary>
    public static Compiler Instance
    {
        get => _instance;
        set => _instance = value ?? throw new ArgumentNullException(nameof(value));
    }
    #endregion
}
