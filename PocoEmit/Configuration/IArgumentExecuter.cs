using PocoEmit.Builders;
using System.Linq.Expressions;

namespace PocoEmit.Configuration;

/// <summary>
/// 含参数执行器
/// </summary>
public interface IArgumentExecuter
{
    /// <summary>
    /// 执行
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="argument"></param>
    /// <returns></returns>
    Expression Execute(IEmitBuilder builder, Expression argument);
}
