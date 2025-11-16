namespace TestApi2.Contract.Events;

/// <summary>
/// 领域事件
/// </summary>
public interface IDomainEvent
{
    ///// <summary>
    ///// 领域事件标识
    ///// </summary>
    //long Id { get; }
    ///// <summary>
    ///// 事件类型
    ///// </summary>
    //string EventType { get; }
    ///// <summary>
    ///// 聚合根类型
    ///// </summary>
    //string RootType { get; }
    /// <summary>
    /// 聚合根标识
    /// </summary>
    long RootId { get; }
}
