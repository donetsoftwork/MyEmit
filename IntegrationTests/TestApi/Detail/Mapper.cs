using FastEndpoints;
using PocoEmit;
using TestApi.Models;
using TestApi.Repositories;

namespace TestApi.Detail;

public sealed class Mapper(UserRepository repository, IPocoConverter<User, Response> responseConverter)
    : Mapper<Request, Response, User?>
{
    #region 配置
    private readonly UserRepository _repository = repository;
    private readonly IPocoConverter<User, Response> _responseConverter = responseConverter;
    #endregion

    public override User? ToEntity(Request req)
        => GetUser(req.Id);
    public override Response FromEntity(User? e)
        => _responseConverter.Convert(e!);
    #region Data
    User? GetUser(int id)
        => _repository.GetById(id);
    #endregion
}