using PocoEmit.Mapper.Members;
using System;
using System.Collections.Generic;

namespace PocoEmit.Mapper.Services;

/// <summary>
/// 目标映射类型信息
/// </summary>
/// <param name="mapType"></param>
public class DestTypeDescriptor(Type mapType)
{
    #region 配置
    private readonly Type _mapType = mapType;
    private readonly List<IWriteMember> _members = [];
    /// <summary>
    /// 映射类型
    /// </summary>
    public Type MapType
        => _mapType;
    /// <summary>
    /// 可写成员
    /// </summary>
    public IEnumerable<IWriteMember> Members
        => _members;
    #endregion
    /// <summary>
    /// 添加
    /// </summary>
    /// <param name="member"></param>
    public void Add(IWriteMember member)
    {
        if (_members.Contains(member))
            return;
        _members.Add(member);
    }
}
