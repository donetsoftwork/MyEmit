using System.Collections.Generic;
using System.Reflection;

namespace PocoEmit.Collections;

#if NET7_0_OR_GREATER
/// <summary>
/// 成员集合
/// </summary>
/// <param name="ReadMembers">可读成员</param>
/// <param name="WriteMembers">可写成员</param>
public record MemberBundle(IDictionary<string, MemberInfo> ReadMembers, IDictionary<string, MemberInfo> WriteMembers);
#else
/// <summary>
/// 成员集合
/// </summary>
/// <param name="readMembers">可读成员</param>
/// <param name="writeMembers">可写成员</param>
public class MemberBundle(IDictionary<string, MemberInfo> readMembers, IDictionary<string, MemberInfo> writeMembers)
{
    #region 配置
    private readonly IDictionary<string, MemberInfo> _readMembers = readMembers;
    private readonly IDictionary<string, MemberInfo> _writeMembers = writeMembers;
    /// <summary>
    /// 可读成员
    /// </summary>
    public IDictionary<string, MemberInfo> ReadMembers
        => _readMembers;
    /// <summary>
    /// 可写成员
    /// </summary>
    public IDictionary<string, MemberInfo> WriteMembers
        => _writeMembers;
    #endregion
}
#endif