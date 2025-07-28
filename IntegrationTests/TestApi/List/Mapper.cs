using FastEndpoints;
using PocoEmit;
using TestApi.Models;
using TestApi.Repositories;

namespace TestApi.List;

public sealed class Mapper(UserRepository repository, IPocoConverter<User, UserListDTO> converter)
    : Mapper<Request, Response, IEnumerable<User>>
{
    public override IEnumerable<User> ToEntity(Request req)
        => GetList(req.Page, req.Size);

    public override Response FromEntity(IEnumerable<User> e) => new()
    {
         Result = [.. e.Select(converter.Convert)],
    };

    #region Data
    IEnumerable<User> GetList(int page, int size)
    {
        var skip = page > 0 ?
            page * size - size
            : 0;
        return repository.List()
            .Skip(skip)
            .Take(size);
    }
    #endregion
}