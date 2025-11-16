using TestApi2.Contract.Models;

namespace TestApi2.Infrastructure;

public class UserQueryService(UserRepository repository)
{
    private readonly List<User> _users = repository.Users;


}
