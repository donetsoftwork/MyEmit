using TestApi2.Contract.Events;

namespace TestApi2.Contract.Models;

/// <summary>
/// 集合根
/// </summary>
public interface IAggregateRoot
{
    /// <summary>
    /// 聚合根Id
    /// </summary>
    long RootId { get; }
    /// <summary>
    /// 应用领域事件
    /// </summary>
    /// <param name="event"></param>
    /// <returns></returns>
    Task Apply(IDomainEvent @event);
}
