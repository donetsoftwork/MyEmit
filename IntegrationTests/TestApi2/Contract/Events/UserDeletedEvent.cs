namespace TestApi2.Contract.Events;

public class UserDeletedEvent : IDomainEvent
{
    long IDomainEvent.RootId
        => Id;
    public long Id { get; set; }
}
