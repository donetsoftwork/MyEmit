using FastEndpoints;
using FluentValidation;
using MyDeltas;
using TestApi.Repositories;

namespace TestApi.Modify;

public sealed class Request
{
    [RouteParam]
    public int Id { get; set; }
    [FromBody]
    public MyDelta<UserModifyDTO> User {  get; set; }
    #region Validator
    /// <summary>
    /// 参数验证
    /// </summary>
    [RegisterService<Validator>(LifeTime.Singleton)]
    internal sealed class Validator : Validator<Request>
    {
        public Validator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithName("IdValidator")
                .WithMessage("Id必须大于0");
        }
    }
    #endregion
}

public sealed class UserModifyDTO
{
    public string Name { get; set; }
    public int Age { get; set; }
}

/// <summary>
/// 参数验证
/// </summary>
[RegisterService<UserModifyDTOValidator>(LifeTime.Singleton)]
public sealed class UserModifyDTOValidator
    : Validator<MyDelta<UserModifyDTO>>
{
    private readonly UserRepository _repository;

    public UserModifyDTOValidator(UserRepository repository)
    {
        _repository = repository;
        RuleFor(x => x.Instance.Name)
            .Must(VerifyName)
            .WithName("VerifyName")
            .WithMessage("Name不能为空或重复");
        RuleFor(x => x.Instance.Age)
            .Must(VerifyAge)
            .WithName("AgeValidator")
            .WithMessage("Age不能小于0");        
    }
    /// <summary>
    /// 验证用户名
    /// </summary>
    /// <param name="delta"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    private bool VerifyName(MyDelta<UserModifyDTO> delta, string name)
    {
        // 有修改才验证
        if(delta.HasChanged(nameof(UserModifyDTO.Name)))
        {
            if(string.IsNullOrWhiteSpace(name))
                return false;
            return _repository.GetByName(name) is null;
        }
        return true;
    }
    /// <summary>
    /// 验证用户年龄
    /// </summary>
    /// <param name="delta"></param>
    /// <param name="age"></param>
    /// <returns></returns>
    private static bool VerifyAge(MyDelta<UserModifyDTO> delta, int age)
    {
        // 有修改才验证
        if (delta.HasChanged(nameof(UserModifyDTO.Age)))
            return age >= 0;
        return true;
    }
}

public sealed class Response
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int? Age { get; set; }
}
