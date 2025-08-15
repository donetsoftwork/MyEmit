using System.Linq.Expressions;

namespace PocoEmit.Collections.Counters;

/// <summary>
/// 元素数量获取器
/// </summary>
public interface IEmitCounter
{
    /// <summary>
    /// 是否按属性获取
    /// </summary>
    bool CountByProperty { get; }
    /// <summary>
    /// 获取数量
    /// </summary>
    /// <param name="collection"></param>
    /// <returns></returns>
    Expression Count(Expression collection);
}
