using Hand.Structural;
using PocoEmit.Builders;
using System;

namespace PocoEmit.Members;

/// <summary>
/// 重命名成员
/// </summary>
/// <typeparam name="TMember"></typeparam>
/// <param name="original"></param>
/// <param name="name"></param>
public class RenameMember<TMember>(TMember original, string name)
    : IMember
    , IWrapper<TMember>
    where TMember : IMember
{
    #region 配置
    /// <summary>
    /// 原始成员
    /// </summary>
    protected readonly TMember _original = original;
    private readonly string _name = name;
    /// <inheritdoc />
    public TMember Original
        => _original;
    /// <inheritdoc />
    public Type InstanceType
        => _original.InstanceType;
    /// <inheritdoc />
    public string Name
        => _name;
    /// <inheritdoc />
    public Type ValueType
        => _original.ValueType;
    #endregion
}
