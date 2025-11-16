using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using MyDeltas;
using TestApi2.App.Command;
using TestApi2.App.Query;
using TestApi2.DTO;

namespace TestApi2.Controllers;

[Route("[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    [HttpPut]
    [ProducesResponseType<UserCreateResult>(200)]
    public UserCreateResult Create(UserCreateCmd cmd)
        => cmd.Execute();

    [HttpDelete("{id}")]
    [ProducesResponseType<UserDeleteResult>(200)]
    public UserDeleteResult Delete([FromRoute] int id)
    {
        var query = new UserDeleteCmd { Id = id };
        return query.Execute();
    }

    [HttpPatch("{id}")]
    [ProducesResponseType<UserModifyResult>(200)]
    public UserModifyResult Modify([FromRoute] int id, [FromBody][ValidateNever] MyDelta<UserDto> delta)
    {
        ModelState.Clear();
        var cmd = new UserModifyCmd(id, delta.Data);
        return cmd.Execute();
    }

    [HttpGet]
    [ProducesResponseType<UserListResult>(200)]
    public UserListResult List(UserListQuery query)
        => query.Get();

    [HttpGet("{id}")]
    [ProducesResponseType<UserDetailResult>(200)]
    public UserDetailResult Detail([FromRoute] int id)
    {
        var query = new UserDetailQuery { Id = id };
        return query.Get();
    }
}
