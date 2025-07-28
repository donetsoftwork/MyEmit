using PocoEmit.Configuration;
using System.Linq.Expressions;

namespace PocoEmit.Members;

/// <summary>
/// 成员写入接口
/// </summary>
public interface IEmitWriter : IEmitInfo
{
    /// <summary>
    /// 写入
    /// </summary>
    /// <param name="instance"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    Expression Write(Expression instance, Expression value);
}
