using FastEndpoints;
using MyDeltas;
using TestApi.Models;

namespace TestApi.Modify;


public sealed class Endpoint(UserModifyDTOValidator validationRules)
    : Endpoint<Request, Response, Mapper>
{
    private readonly UserModifyDTOValidator _validationRules = validationRules;

    public override void Configure()
    {
        Patch("/users/{Id}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(Request req, CancellationToken c)
    {
        MyDelta<UserModifyDTO> dto = req.User;
        dto.Patch(dto.Instance);
        var result = _validationRules.Validate(dto);
        if(!result.IsValid)
        {
            ThrowError(result.Errors[0]);
        }
        User? entity = Map.ToEntity(req);
        if (entity is null)
        {
            ThrowError($"Id = {req.Id} 的User不存在");
        }
        var res = Map.FromEntity(entity);
        await Send.OkAsync(res, c);
    }
}