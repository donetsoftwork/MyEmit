using System;
using System.Linq.Expressions;

namespace PocoEmit.Mapper.Members;

/// <summary>
/// 可读成员
/// </summary>
public interface IReadMember : IMember
{
    /// <summary>
    /// 读取
    /// </summary>
    /// <param name="instance"></param>
    /// <returns></returns>
    Expression EmitRead(Expression instance);
}
