using PocoEmit;
using TestApi2.Contract.Events;
using TestApi2.Contract.Models;
using TestApi2.PrimaryStorage;

namespace TestApi2.Domain.Models;

public class UserRoot(UserId id, IMapper mapper)
    : IAggregateRoot
{
    #region 配置
    private readonly IMapper _mapper = mapper;
    public UserId Id { get; } = id;
    public UserName Name { get; set; }
    public UserAge Age { get; set; }
    long IAggregateRoot.RootId
    => Id.Value;
    #endregion

    public async Task AddAsync()
    {
        var instance = _mapper.Convert<UserRoot, UserDo>(this);
        var @event = _mapper.Convert<UserRoot, UserCreatedEvent>(this);
        await instance.AddAsync();
    }

    public async Task ModifyAsync(IDictionary<string, object?> data)
    {
        var instance = _mapper.Convert<UserRoot, UserDo>(this);
        var @event = new UserModifiedEvent(instance.Id, data);
        await instance.ModifyAsync(data.Keys);
    }

    public async Task DeleteAsync()
    {
        var instance = _mapper.Convert<UserRoot, UserDo>(this);
        var @enent = new UserDeletedEvent { Id = instance.Id };
        await instance.DeleteAsync();
    }

    #region Apply
    public async Task Apply(UserCreatedEvent @event)
    {
        _mapper.Copy(@event, this);
        var instance = _mapper.Convert<UserRoot, UserDo>(this);
        await instance.AddAsync();
    }
    public async Task Apply(UserModifiedEvent @event)
    {
        _mapper.Copy(@event, this);
        var instance = _mapper.Convert<UserRoot, UserDo>(this);
        await instance.ModifyAsync(@event.Data.Keys);
    }
    public async Task Apply(UserDeletedEvent @event)
    {
        _mapper.Copy(@event, this);
        var instance = _mapper.Convert<UserRoot, UserDo>(this);
        await instance.DeleteAsync();
    }

    Task IAggregateRoot.Apply(IDomainEvent @event)
    {
        throw new NotImplementedException();
    }
    #endregion
}

public class UserId(long value)
{
    public long Value { get; } = value;

    private static long seed = 1;
    public static UserId NewId()
        => new(seed++);
}

public class UserName(string value)
{
    public string Value { get; } = value;
}
public class UserAge(int? value)
{
    public int? Value { get; } = value;
}
