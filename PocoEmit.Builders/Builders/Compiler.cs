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
