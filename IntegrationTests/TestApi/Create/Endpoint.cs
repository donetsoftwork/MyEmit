using FastEndpoints;

namespace TestApi.Create;

public sealed class Endpoint : Endpoint<Request, Response, Mapper>
{
    public override void Configure()
    {
        Put("/users");
        AllowAnonymous();
    }

    public override async Task HandleAsync(Request req, CancellationToken c)
    {
        var entity = Map.ToEntity(req);
        var res = Map.FromEntity(entity);
        await Send.OkAsync(res, c);
    }
}