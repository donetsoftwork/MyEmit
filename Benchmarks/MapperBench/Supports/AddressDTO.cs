namespace MapperBench.Supports;

public class Address
{
    public int Id { get; set; }
    public string Street { get; set; }
    public string City { get; set; }
    public string Country { get; set; }
}

public class AddressDTO
{
    public int Id { get; set; }
    public string City { get; set; }
    public string Country { get; set; }
}
