using FastEndpoints;
using PocoEmit;
using TestApi.Models;
using TestApi.Repositories;

namespace TestApi.Create;

[RegisterService<Mapper>(LifeTime.Singleton)]
public sealed class Mapper(
    UserRepository repository, 
    IPocoConverter<Request, User> requestConverter, 
    IPocoConverter<User, Response> responseConverter)
    : Mapper<Request, Response, User>
{
    #region 配置
    private readonly UserRepository _repository = repository;
    private readonly IPocoConverter<Request, User> _requestConverter = requestConverter;
    private readonly IPocoConverter<User, Response> _responseConverter = responseConverter;
    #endregion
    public override User ToEntity(Request r)
    {
        User entity = _requestConverter.Convert(r);
        return Save(entity);
    }
    public override Response FromEntity(User e)
        => _responseConverter.Convert(e);
    #region Data
    User Save(User entity)
        => _repository.Add(entity);
    #endregion
}
