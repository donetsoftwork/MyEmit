using PocoEmit.Converters;
using PocoEmit.Members;
using System;
using System.Linq.Expressions;

namespace PocoEmit.Builders;

/// <summary>
/// 编译器
/// </summary>
public class Compiler
{
    #region CompileFunc
    /// <summary>
    /// 编译Func
    /// </summary>
    /// <typeparam name="TArgument"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="func"></param>
    /// <returns></returns>
    public virtual Func<TArgument, TResult> CompileFunc<TArgument, TResult>(Expression<Func<TArgument, TResult>> func)
        => func.Compile();
    #endregion
    #region CompileAction
    /// <summary>
    /// 编译Action
    /// </summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <param name="action"></param>
    /// <returns></returns>
    public virtual Action<T1, T2> CompileAction<T1, T2>(Expression<Action<T1, T2>> action)
        => action.Compile();
    #endregion
    #region Compile
    /// <summary>
    /// 编译成员读取器
    /// </summary>
    /// <typeparam name="TInstance"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="reader"></param>
    /// <returns></returns>
    public static Func<TInstance, TValue> CompileFunc<TInstance, TValue>(IEmitMemberReader reader)
        => _instance.CompileFunc(reader.Build<TInstance, TValue>());
    /// <summary>
    /// 编译成员写入器
    /// </summary>
    /// <typeparam name="TInstance"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="writer"></param>
    /// <returns></returns>
    public static Action<TInstance, TValue> CompileAction<TInstance, TValue>(IEmitMemberWriter writer)
        => _instance.CompileAction(writer.Build<TInstance, TValue>());
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
