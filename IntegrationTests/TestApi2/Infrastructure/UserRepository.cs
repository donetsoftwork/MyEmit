using TestApi2.Contract.Models;

namespace TestApi2.Infrastructure;

public class UserRepository
{
    private readonly List<User> _users = [];

    public List<User> Users 
        => _users;
}
    
