using FastEndpoints;

namespace TestApi.List;

public sealed class Endpoint : Endpoint<Request, Response, Mapper>
{
    public override void Configure()
    {
        Get("/users");
        AllowAnonymous();
    }

    public override async Task HandleAsync(Request req, CancellationToken c)
    {
        var entity = Map.ToEntity(req);
        var res = Map.FromEntity(entity);
        await Send.OkAsync(res, c);
    }
}
