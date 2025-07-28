using PocoEmit.Configuration;
using System;
using System.Linq.Expressions;
using System.Reflection;

namespace PocoEmit.Members;

/// <summary>
/// 已编译写入器
/// </summary>
/// <typeparam name="TInstance"></typeparam>
/// <typeparam name="TValue"></typeparam>
/// <param name="inner"></param>
/// <param name="writeAction"></param>
public sealed class CompiledWriter<TInstance, TValue>(IEmitMemberWriter inner, Action<TInstance, TValue> writeAction)
    : MemberAccessor<MethodInfo>(inner.InstanceType, writeAction.GetMethodInfo(), inner.Name, typeof(TValue))
    , ICompiledWriter<TInstance, TValue>
    , IEmitMemberWriter
{
    #region 配置
    private readonly IEmitMemberWriter _inner = inner;
    /// <summary>
    /// 原始成员写入器
    /// </summary>
    public IEmitMemberWriter Inner
        => _inner;
    private readonly Action<TInstance, TValue> _writeAction = writeAction;
    /// <inheritdoc />
    public Action<TInstance, TValue> WriteAction
        => _writeAction;    
    /// <inheritdoc />
    bool IEmitInfo.Compiled
        => true;
    #endregion
    /// <inheritdoc />
    public Expression Write(Expression instance, Expression value)
        => _inner.Write(instance, value);
    /// <inheritdoc />
    void IMemberWriter<TInstance, TValue>.Write(TInstance instance, TValue value)
        => _writeAction(instance, value);
}
