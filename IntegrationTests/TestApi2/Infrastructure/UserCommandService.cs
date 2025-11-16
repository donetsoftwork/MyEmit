using MyDeltas;
using PocoEmit;
using TestApi2.Contract.Infrastructure;
using TestApi2.Contract.Models;

namespace TestApi2.Infrastructure;

/// <summary>
/// Command
/// </summary>
/// <param name="mapper"></param>
/// <param name="repository"></param>
public class UserCommandService(IMapper mapper, UserRepository repository)
    : IRepository<User>
{
    private readonly List<User> _users = repository.Users;
    private readonly IMapper _mapper = mapper;

    public IQueryable<User> List()
        => _users.AsQueryable();
    public User? GetByName(string name)
        => _users.FirstOrDefault(u => string.Equals(u.Name, name, StringComparison.OrdinalIgnoreCase));
    public User? GetById(long id)
        => _users.FirstOrDefault(u => u.Id == id);
    //private long _idGenerator = 0;

    public User Add(User user)
    {
        //user.Id = Interlocked.Increment(ref _idGenerator);
        _users.Add(user);
        return user;
    }
    public User? Modify(MyDelta<User> user)
    {
        var existingUser = GetById(user.Instance.Id);
        if (existingUser == null)
            return null;
        user.Patch(existingUser);
        return existingUser;
    }
    public User? Delete(long id)
    {
        var user = GetById(id);
        if (user == null)
            return null;
        _users.Remove(user);
        return user;
    }

    Task<User?> IRepository<User>.GetByIdAsync(long id)
        => Task.FromResult(GetById(id));

    Task IRepository<User>.InsertAsync(User entity)
    {
        Add(entity);
        return Task.CompletedTask;
    }

    Task IRepository<User>.UpdateAsync(User entity, IEnumerable<string> fieldNames)
    {
        var existingUser = GetById(entity.Id);
        if (existingUser != null)
        {
            var members = _mapper.GetTypeMembers<User>();
            //var readers = members.
            foreach (var fieldName in fieldNames)
            {
                var member = members.GetReadMember(fieldName);
                if (member is null)
                    continue;
                var readFunc = _mapper.GetReadFunc(member);
                if (readFunc is null)
                    continue;
                var writeFun = _mapper.GetWriteAction(member);
                if (writeFun is null)
                    continue;
                writeFun(existingUser, readFunc(entity));
            }
        }
        return Task.CompletedTask;
    }

    Task IRepository<User>.DeleteAsync(User entity)
    {
        Delete(entity.Id);
        return Task.CompletedTask;
    }
}
