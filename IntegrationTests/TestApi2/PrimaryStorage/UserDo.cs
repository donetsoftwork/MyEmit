using TestApi2.Contract.Infrastructure;
using TestApi2.Contract.Models;

namespace TestApi2.PrimaryStorage;

/// <summary>
/// 数据处理对象
/// </summary>
/// <param name="repository"></param>
public class UserDo(IRepository<User> repository)
    : User
{
    private readonly IRepository<User> _repository = repository;

    public IRepository<User> Repository 
        => _repository;

    public Task AddAsync()
        => _repository.InsertAsync(this);

    public Task ModifyAsync(params IEnumerable<string> fieldNames)
        => _repository.UpdateAsync(this, fieldNames);

    public Task DeleteAsync()
        => _repository.DeleteAsync(this);
}
