using Hand.Creational;
using PocoEmit.Configuration;
using System;
using System.Linq.Expressions;

namespace PocoEmit.Builders;

/// <summary>
/// 已编译构造器
/// </summary>
/// <typeparam name="TValue"></typeparam>
/// <param name="original"></param>
/// <param name="_buildFunc"></param>
public class CompiledFunc<TValue>(ICreator<Expression<Func<TValue>>> original, Func<TValue> _buildFunc)
    : WrapExpressionBuilder<Expression<Func<TValue>>>(original)
    , IEmitFunc
    , IEmitExecuter
{
    #region 配置
    private readonly Func<TValue> _buildFunc = _buildFunc;
    /// <summary>
    /// 构造委托
    /// </summary>
    public Func<TValue> BuildFunc 
        => _buildFunc;
    /// <summary>
    /// 原始执行器
    /// </summary>
    private readonly IEmitExecuter _executer = original as IEmitExecuter;
    /// <inheritdoc />
    bool ICompileInfo.Compiled
        => true;
    #endregion
    /// <inheritdoc />
    LambdaExpression ICreator<LambdaExpression>.Create()
        => _original.Create();
    /// <inheritdoc />
    Expression IEmitExecuter.Execute(IEmitBuilder builder)
    {
        if (_executer is null)
            return _original.Create().Body;
        return _executer.Execute(builder);
    }
}
