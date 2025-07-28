using FastEndpoints;
using FluentValidation;

namespace TestApi.Delete;

public sealed class Request
{
    public int Id { get; set; }

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

public sealed class Response
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int? Age { get; set; }
    public string Message { get; set; }
}


