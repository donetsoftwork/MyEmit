using PocoEmit.Configuration;
using System;
using System.Linq.Expressions;
using System.Reflection;

namespace PocoEmit.Members;

/// <summary>
/// 已编译读取器
/// </summary>
/// <typeparam name="TInstance"></typeparam>
/// <typeparam name="TValue"></typeparam>
/// <param name="inner"></param>
/// <param name="readFunc"></param>
public sealed class CompiledReader<TInstance, TValue>(IEmitMemberReader inner, Func<TInstance, TValue> readFunc)
    : MemberAccessor<MethodInfo>(inner.InstanceType, readFunc.GetMethodInfo(), inner.Name, typeof(TValue))
    , ICompiledReader<TInstance, TValue>
{
    #region 配置
    private readonly IEmitMemberReader _inner = inner;
    /// <summary>
    /// 原始成员读取器
    /// </summary>
    public IEmitMemberReader Inner
        => _inner;
    private readonly Func<TInstance, TValue> _readFunc = readFunc;
    /// <inheritdoc />
    public Func<TInstance, TValue> ReadFunc
        => _readFunc;
    /// <inheritdoc />
    MemberInfo IEmitMemberReader.Info
        => _inner.Info;
    /// <inheritdoc />
    bool ICompileInfo.Compiled
        => true;
    #endregion
    /// <inheritdoc />
    public Expression Read(Expression instance)
        => _inner.Read(instance);
    /// <inheritdoc />
    TValue IMemberReader<TInstance, TValue>.Read(TInstance instance)
        => _readFunc(instance);
}

