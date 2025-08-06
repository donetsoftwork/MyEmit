using PocoEmit.Configuration;
using System.Linq.Expressions;

namespace PocoEmit.Members;

/// <summary>
/// 成员读取接口
/// </summary>
public interface IEmitReader
    : ICompileInfo
{
    /// <summary>
    /// 读取
    /// </summary>
    /// <param name="instance"></param>
    Expression Read(Expression instance);
}
