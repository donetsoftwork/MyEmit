using PocoEmit.Members;
using System.Collections.Generic;
using System.Reflection;

namespace PocoEmit.Collections;

#if NET7_0_OR_GREATER
/// <summary>
/// 成员集合
/// </summary>
/// <param name="ReadMembers">可读成员</param>
/// <param name="EmitReaders">读取器</param>
/// <param name="WriteMembers">可写成员</param>
/// <param name="EmitWriters">写入器</param>
public record MemberBundle(IDictionary<string, MemberInfo> ReadMembers, IDictionary<string, IEmitMemberReader> EmitReaders, IDictionary<string, MemberInfo> WriteMembers, IDictionary<string, IEmitMemberWriter> EmitWriters);
#else
/// <summary>
/// 成员集合
/// </summary>
/// <param name="readMembers">可读成员</param>
/// <param name="emitReaders">读取器</param>
/// <param name="writeMembers">可写成员</param>
/// <param name="emitWriters">写入器</param>
public class MemberBundle(IDictionary<string, MemberInfo> readMembers, IDictionary<string, IEmitMemberReader> emitReaders, IDictionary<string, MemberInfo> writeMembers, IDictionary<string, IEmitMemberWriter> emitWriters)
{
    #region 配置
    private readonly IDictionary<string, MemberInfo> _readMembers = readMembers;
    private readonly IDictionary<string, IEmitMemberReader> _emitReaders = emitReaders;
    private readonly IDictionary<string, MemberInfo> _writeMembers = writeMembers;
    private readonly IDictionary<string, IEmitMemberWriter> _emitWriters = emitWriters;
    /// <summary>
    /// 可读成员
    /// </summary>
    public IDictionary<string, MemberInfo> ReadMembers
        => _readMembers;
    /// <summary>
    /// 读取器
    /// </summary>
    public IDictionary<string, IEmitMemberReader> EmitReaders
        => _emitReaders;
    /// <summary>
    /// 可写成员
    /// </summary>
    public IDictionary<string, MemberInfo> WriteMembers
        => _writeMembers;
    /// <summary>
    /// 写入器
    /// </summary>
    public IDictionary<string, IEmitMemberWriter> EmitWriters
        => _emitWriters;
    #endregion
}
#endif