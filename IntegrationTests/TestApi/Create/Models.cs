using FastEndpoints;
using TestApi.Repositories;
using FluentValidation;

namespace TestApi.Create;

public sealed class Request
{
    public string Name { get; set; }
    public int? Age { get; set; }
    #region Validator
    /// <summary>
    /// 参数验证
    /// </summary>
    [RegisterService<Validator>(LifeTime.Singleton)]
    public sealed class Validator : Validator<Request>
    {
        public Validator(UserRepository repository)
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithName("NameNotEmpty")
                .WithMessage("Name不能为空");
            RuleFor(x => x.Name)
                .Must(name => repository.GetByName(name) is null)
                .WithName("NameIsUnique")
                .WithMessage("Name不能重复");
            RuleFor(x => x.Age)
                .Must(age => age is null || age.Value >= 0)
                .WithName("AgeValidator")
                .WithMessage("Age不能小于0");
        }
    }
    #endregion
}

public sealed class Response
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int? Age { get; set; }
}
