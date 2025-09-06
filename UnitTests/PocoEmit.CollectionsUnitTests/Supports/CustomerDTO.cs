namespace PocoEmit.CollectionsUnitTests.Supports;


public class CustomerDTO
{
    public string Name { get; set; }
    public AddressDTO Address { get; set; }
    public AddressDTO[] Addresses { get; set; }
    public List<AddressDTO> AddressList { get; set; }
}

public class Customer
{
    public string Name { get; set; }
    public Address Address { get; set; }
    public Address[] Addresses { get; set; }
    public List<Address> AddressList { get; set; }
}
