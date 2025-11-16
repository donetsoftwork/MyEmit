using Hand.Structural;
using PocoEmit.Builders;
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
/// <param name="original"></param>
/// <param name="readFunc"></param>
public sealed class CompiledReader<TInstance, TValue>(IEmitMemberReader original, Func<TInstance, TValue> readFunc)
    : MemberAccessor<MethodInfo>(original.InstanceType, readFunc.GetMethodInfo(), original.Name, typeof(TValue))
    , ICompiledReader<TInstance, TValue>
    , IWrapper<IEmitMemberReader>
{
    #region 配置
    private readonly IEmitMemberReader _original = original;
    /// <inheritdoc />
    public IEmitMemberReader Original
        => _original;
    private readonly Func<TInstance, TValue> _readFunc = readFunc;
    /// <inheritdoc />
    public Func<TInstance, TValue> ReadFunc
        => _readFunc;
    /// <inheritdoc />
    MemberInfo IEmitMemberReader.Info
        => _original.Info;
    /// <inheritdoc />
    bool ICompileInfo.Compiled
        => true;
    #endregion
    /// <inheritdoc />
    public Expression Read(Expression instance)
        => _original.Read(instance);
    /// <inheritdoc />
    TValue IMemberReader<TInstance, TValue>.Read(TInstance instance)
        => _readFunc(instance);
}

