using FastEndpoints;

namespace TestApi.List;

public sealed class Request
{
    [QueryParam]
    public int Page {  get; set; }
    [QueryParam]
    public int Size { get; set; } = 10;

}

public sealed class Response
{
    public UserListDTO[] Result { get; set; }
    public string Message { get; set; }
}

public sealed class UserListDTO
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int? Age { get; set; }
}
