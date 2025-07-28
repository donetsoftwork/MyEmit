using FastEndpoints;

namespace TestApi.Delete;

public sealed class Endpoint : Endpoint<Request, Response, Mapper>
{
    public override void Configure()
    {
        Delete("users/{Id}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(Request req, CancellationToken c)
    {
        var entity = Map.ToEntity(req);
        if(entity is null)
        {
            ThrowError($"Id = {req.Id} 的User不存在");
        }
        else
        {
            var res = Map.FromEntity(entity);
            await Send.OkAsync(res, c);
        }
    }
}