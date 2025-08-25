namespace MapperBench.Supports;

public class Customer
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal? Credit { get; set; }
    public Address Address { get; set; }
    public Address HomeAddress { get; set; }
    public Address[] Addresses { get; set; }
    public List<Address> WorkAddresses { get; set; }
}

public class CustomerDTO
{
    public int Id { get; set; }
    public string Name { get; set; }
    public Address Address { get; set; }
    public AddressDTO HomeAddress { get; set; }
    public AddressDTO[] Addresses { get; set; }
    public List<AddressDTO> WorkAddresses { get; set; }
    public string AddressCity { get; set; }
}
