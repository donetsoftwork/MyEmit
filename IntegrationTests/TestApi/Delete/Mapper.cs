using FastEndpoints;
using PocoEmit;
using TestApi.Models;
using TestApi.Repositories;

namespace TestApi.Delete;

public sealed class Mapper(UserRepository repository, IPocoConverter<User, Response> _responseConverter)
    : Mapper<Request, Response, User?>
{
    #region 配置
    private readonly UserRepository _repository = repository;
    private readonly IPocoConverter<User, Response> _responseConverter = _responseConverter;
    #endregion

    public override User? ToEntity(Request req)
        => Delete(req.Id);
    public override Response FromEntity(User? e)
        => _responseConverter.Convert(e!);
    #region Data
    User? Delete(int id)
        => _repository.Delete(id);
    #endregion
}