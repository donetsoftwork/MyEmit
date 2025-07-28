namespace PocoEmit.MapperUnitTests.Supports;

public class User
{
    public int Id { get; set; }
    public string Name { get; set; }
}
public record UserDTO
{
    public int Id { get; set; }
    public string Name { get; set; }
}
