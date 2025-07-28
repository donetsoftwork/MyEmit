using System.Linq.Expressions;

namespace PocoEmit.Mapper.Members;

/// <summary>
/// 可写成员
/// </summary>
public interface IWriteMember
{
    /// <summary>
    /// 写入
    /// </summary>
    /// <param name="instance"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    Expression EmitWrite(Expression instance, Expression value);
}
