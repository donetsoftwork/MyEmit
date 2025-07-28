using PocoEmit.Mapper.Services;
using System;

namespace PocoEmit.Mapper.Members;

/// <summary>
/// 成员
/// </summary>
public interface IMember
{
    /// <summary>
    /// 成员名
    /// </summary>
    string MemberName { get; }
    /// <summary>
    /// 成员名
    /// </summary>
    Type MemberType { get; }
    /// <summary>
    /// 接受源映射
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    bool Accept(SourceTypeDescriptor source);
    /// <summary>
    /// 接受目标映射
    /// </summary>
    /// <param name="dest"></param>
    /// <returns></returns>
    bool Accept(DestTypeDescriptor dest);
}
