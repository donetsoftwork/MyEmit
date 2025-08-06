using PocoEmit.Configuration;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace PocoEmit.Copies;

/// <summary>
/// 复制成员
/// </summary>
/// <param name="members"></param>
public class ComplexTypeCopier(IEnumerable<IMemberConverter> members)
    : IEmitCopier
{
    private  IEnumerable<IMemberConverter> _members = members;
    /// <summary>
    /// 成员
    /// </summary>
    public IEnumerable<IMemberConverter> Members 
        => _members;
    /// <inheritdoc />
    bool ICompileInfo.Compiled
        => false;
    /// <inheritdoc />
    public IEnumerable<Expression> Copy(Expression source, Expression dest)
    {
        foreach (var member in _members) 
            yield return member.Convert(source, dest);
    }
    /// <summary>
    /// 完成
    /// </summary>
    internal void MembersToArray()
    {        
        _members = [.. _members];// 加速
    }
}
