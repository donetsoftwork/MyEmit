namespace TestApi2.Contract.Models;

public class User
    : IEntity<long>
{
    public long Id { get; set; }
    public string Name { get; set; }
    public int? Age { get; set; }
}
