using System;
using System.Reflection;

namespace PocoEmit.Members;

/// <summary>
/// 成员
/// </summary>
public interface IMember
{
    /// <summary>
    /// 实体类型
    /// </summary>
    Type InstanceType { get; }
    /// <summary>
    /// 成员名
    /// </summary>
    string Name { get; }
    /// <summary>
    /// 值类型
    /// </summary>
    Type ValueType { get; }
}
/// <summary>
/// 成员写入器
/// </summary>
public interface IEmitMemberWriter : IMember, IEmitWriter
{
    /// <summary>
    /// 写入目标信息
    /// </summary>
    MemberInfo Info { get; }
}
/// <summary>
/// 成员读取器
/// </summary>
public interface IEmitMemberReader : IMember, IEmitReader
{
    /// <summary>
    /// 读取目标信息
    /// </summary>
    MemberInfo Info { get; }
}