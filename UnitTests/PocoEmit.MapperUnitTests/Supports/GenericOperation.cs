namespace PocoEmit.MapperUnitTests.Supports;

public class GenericOperation<TRole, TThing>
{
    public TRole Role { get; set; }
    public string Name { get; set; }
    public TThing Thing { get; set; }
}

public class Book
{
    public int Id { get; set; }
    public string Title { get; set; }
}


public class GenericOperationDTO<TRole, TThing>
{
    public TRole Role { get; set; }
    public string Name { get; set; }
    public TThing Thing { get; set; }
}

public class BookDTO
{
    public int Id { get; set; }
    public string Title { get; set; }
}