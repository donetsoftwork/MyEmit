using PocoEmit.Mapper.Services;
using System;

namespace PocoEmit.Mapper.Members;

/// <summary>
/// 成员描述
/// </summary>
/// <param name="memberName"></param>
/// <param name="memberType"></param>
internal abstract class MemberDescriptor(string memberName, Type memberType)
    : IMember
{
    #region 配置
    private readonly string _memberName = memberName;
    /// <summary>
    /// 成员名
    /// </summary>
    public string MemberName
        => _memberName;
    private readonly Type _memberType = memberType;
    /// <summary>
    /// 成员类型
    /// </summary>
    public Type MemberType
        => _memberType;
    /// <inheritdoc />
    public abstract bool Accept(SourceTypeDescriptor source);
    /// <inheritdoc />
    public abstract bool Accept(DestTypeDescriptor dest);
    #endregion
}
