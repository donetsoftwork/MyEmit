using FastEndpoints;
using Microsoft.AspNetCore.Mvc;
using PocoEmit;
using PocoEmit.ServiceProvider;

namespace TestApi.Detail;

public sealed class Endpoint : Endpoint<Request, Response, Mapper>
{
    public override void Configure()
    {
        Get("users/{Id}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(Request req, CancellationToken c)
    {
        var test = PocoEmit.Mapper.Default.Convert<Request, TestRequest>(req);
        var root = test.Wrapper.Root;
        //var s = test.Wrapper.GetServiceProvider();
        //var s1 = test.Wrapper.GetServiceProvider();
        //var state0 = s == root;
        //var state1 = s == s1;
        //var sp = HttpContext.RequestServices;
        //var state = s == sp;
        var s1 = root.GetService<IServiceScope>();
        var s2 = root.GetService<IServiceScope>();
        var state1 = s1 == s2;
        //FromKeyedServicesAttribute
        //FromServicesAttribute
        //IHttpContextAccessor contextAccessor = null;
        var entity = Map.ToEntity(req);
        if (entity is null)
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

public class TestRequest(ScopeBuilder wrapper)
{
    public ScopeBuilder Wrapper { get; private set; } = wrapper;
}