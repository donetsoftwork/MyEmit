using PocoEmit.Configuration;
using System.Linq.Expressions;
using System.Reflection;

namespace PocoEmit.Members;

/// <summary>
/// 重命名读取器
/// </summary>
/// <param name="inner"></param>
/// <param name="name"></param>
public sealed class RenameReader(IEmitMemberReader inner, string name)
    : RenameMember<IEmitMemberReader>(inner, name), IEmitMemberReader
{
    #region 配置
    /// <inheritdoc />
    public MemberInfo Info 
        => _inner.Info;
    /// <inheritdoc />
    bool ICompileInfo.Compiled
        => false;
    #endregion
    /// <inheritdoc />
    public Expression Read(Expression instance)
        => _inner.Read(instance);
}
