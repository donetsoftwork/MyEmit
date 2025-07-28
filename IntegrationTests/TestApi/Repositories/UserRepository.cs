using FastEndpoints;
using MyDeltas;
using TestApi.Models;

namespace TestApi.Repositories;

public class UserRepository
{
    private readonly List<User> _users = [];
    public IQueryable<User> List()
        => _users.AsQueryable();
    public User? GetByName(string name)
        => _users.FirstOrDefault(u => string.Equals( u.Name, name, StringComparison.OrdinalIgnoreCase));
    public User? GetById(int id)
        => _users.FirstOrDefault(u => u.Id == id);
    private int _idGenerator = 0;

    public User Add(User user)
    {
        user.Id = Interlocked.Increment(ref _idGenerator);
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
    public User? Delete(int id)
    {
        var user = GetById(id);
        if (user == null)
            return null;
        _users.Remove(user);
        return user;
    }
}
