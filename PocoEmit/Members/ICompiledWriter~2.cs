using System;

namespace PocoEmit.Members;

/// <summary>
/// 已编译成员写入器
/// </summary>
/// <typeparam name="TInstance">实体类型</typeparam>
/// <typeparam name="TValue">成员类型</typeparam>
public interface ICompiledWriter<TInstance, TValue>
    : IEmitMemberWriter
    , IMemberWriter<TInstance, TValue>
{
    /// <summary>
    /// 写入委托
    /// </summary>
    Action<TInstance, TValue> WriteAction { get; }
}
