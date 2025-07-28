using System;

namespace PocoEmit.Members;

/// <summary>
/// 已编译成员读取器
/// </summary>
/// <typeparam name="TInstance">实体类型</typeparam>
/// <typeparam name="TValue">成员类型</typeparam>
public interface ICompiledReader<TInstance, TValue>
    : IEmitMemberReader
    , IMemberReader<TInstance, TValue>
{
    /// <summary>
    /// 读取委托
    /// </summary>
    Func<TInstance, TValue> ReadFunc { get; }
}
