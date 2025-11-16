namespace TestApi2.Contract.Events;

public class UserModifiedEvent(long id, IDictionary<string, object?> data)
     : IDomainEvent
{
    long IDomainEvent.RootId
        => Id;
    public long Id { get; } = id;
    /// <summary>
    /// 变化的数据
    /// </summary>
    public IDictionary<string, object?> Data { get; } = data;
}
