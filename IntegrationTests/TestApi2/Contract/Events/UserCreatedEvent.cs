using TestApi2.Contract.Models;

namespace TestApi2.Contract.Events;

/// <summary>
/// 
/// </summary>
public class UserCreatedEvent
    : User, IDomainEvent
{
    long IDomainEvent.RootId
        => Id;
}
