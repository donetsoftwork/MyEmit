using PocoEmit.Builders;
using System.Linq.Expressions;

namespace PocoEmit.Configuration;

/// <summary>
/// 执行器
/// </summary>
public interface IEmitExecuter
{
    /// <summary>
    /// 执行
    /// </summary>
    /// <param name="builder"></param>
    /// <returns></returns>
    Expression Execute(IEmitBuilder builder);
}
