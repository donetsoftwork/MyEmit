using System;

namespace PocoEmit.Members;

/// <summary>
/// 重命名成员
/// </summary>
/// <typeparam name="TMember"></typeparam>
/// <param name="inner"></param>
/// <param name="name"></param>
public class RenameMember<TMember>(TMember inner, string name)
    : IMember
    where TMember : IMember
{
    #region 配置
    /// <summary>
    /// 内部成员
    /// </summary>
    protected readonly TMember _inner = inner;
    private readonly string _name = name;

    /// <inheritdoc />
    public Type InstanceType
        => _inner.InstanceType;
    /// <inheritdoc />
    public string Name
        => _name;
    /// <inheritdoc />
    public Type ValueType
        => _inner.ValueType;
    #endregion
}
