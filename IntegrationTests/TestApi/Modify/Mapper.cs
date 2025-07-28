using FastEndpoints;
using MyDeltas;
using PocoEmit;
using TestApi.Models;
using TestApi.Repositories;

namespace TestApi.Modify;

[RegisterService<Mapper>(LifeTime.Singleton)]
public sealed class Mapper(
    UserRepository repository,
    IMyDeltaFactory deltaFactory,
    IPocoConverter<User, Response> responseConverter)
    : Mapper<Request, Response, User?>
{
    #region 配置
    private readonly UserRepository _repository = repository;
    private readonly IMyDeltaFactory _deltaFactory = deltaFactory;
    private readonly IPocoConverter<User, Response> _responseConverter = responseConverter;
    #endregion

    public override User? ToEntity(Request r)
    {
        var user0 = GetById(r.Id);
        if (user0 == null)
            return null;
        var delta = _deltaFactory.Create(user0, r.User.Data);
        return Modify(delta);
    }
    public override Response FromEntity(User? e)
        => _responseConverter.Convert(e!);

    #region Data
    User? GetById(int id)
        => _repository.GetById(id);
    User? Modify(MyDelta<User> user)
        => _repository.Modify(user);
    #endregion
}

