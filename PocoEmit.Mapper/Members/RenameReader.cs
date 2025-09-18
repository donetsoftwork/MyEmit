using PocoEmit.Builders;
using PocoEmit.Configuration;
using System.Linq.Expressions;
using System.Reflection;

namespace PocoEmit.Members;

/// <summary>
/// 重命名读取器
/// </summary>
/// <param name="original"></param>
/// <param name="name"></param>
public sealed class RenameReader(IEmitMemberReader original, string name)
    : RenameMember<IEmitMemberReader>(original, name)
    , IEmitMemberReader
    , IWrapper<IEmitMemberReader>
{
    #region 配置
    /// <inheritdoc />
    public MemberInfo Info 
        => _original.Info;
    /// <inheritdoc />
    bool ICompileInfo.Compiled
        => false;
    #endregion
    /// <inheritdoc />
    public Expression Read(Expression instance)
        => _original.Read(instance);
}
