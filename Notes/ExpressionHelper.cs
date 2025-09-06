using System.Linq.Expressions;

public class ExpressionHelper
{
    /// <summary>
    /// 定义转化委托 Customer -> CustomerDTO
    /// </summary>
    /// <returns></returns>
    public static Expression<Func<Customer, CustomerDTO>> CreateCustomerDTO()
    {
        var customerType = typeof(Customer);
        var dtoType = typeof(CustomerDTO);
        // Customer customer;
        var customer = Expression.Parameter(customerType, "customer");
        // CustomerDTO dto;
        var dto = Expression.Parameter(dtoType, "dto");
        var addressDTOConvertFunc = CreateAddressDTO();
        var body = Expression.Block(
            [dto],
            // dto = new AddressDTO();
            Expression.Assign(dto, Expression.New(dtoType)),
            // dto.Name = customer.Name;
            Expression.Assign(Expression.Property(dto, "Name"), Expression.Property(customer, "Name")),
            // dto.Address = addressDTOConvertFunc.Invoke(customer.Address);
            ConvertAddress(addressDTOConvertFunc, customer, dto),
            // dto.Addresses
            ConvertAddresses(addressDTOConvertFunc, customer, dto),
            // dto.AddressList
            ConvertAddressList(addressDTOConvertFunc, customer, dto),
            // return dto
            dto
        );
        return Expression.Lambda<Func<Customer, CustomerDTO>>(body, customer);
    }
    /// <summary>
    /// 转化 customer.Address -> dto.Address
    /// </summary>
    /// <param name="addressDTOConvertFunc"></param>
    /// <param name="customer"></param>
    /// <param name="dto"></param>
    /// <returns></returns>
    public static Expression ConvertAddress(Expression<Func<Address, AddressDTO>> addressDTOConvertFunc, ParameterExpression customer, ParameterExpression dto)
    {
        // dto.Address = addressDTOConvertFunc.Invoke(customer.Address);
        return Expression.Assign(Expression.Property(dto, "Address"), Expression.Invoke(addressDTOConvertFunc, Expression.Property(customer, "Address")));
    }
    /// <summary>
    /// 转化 customer.AddressList -> dto.AddressList
    /// </summary>
    /// <param name="customer"></param>
    /// <param name="dto"></param>
    /// <returns></returns>
    public static BlockExpression ConvertAddresses(Expression<Func<Address, AddressDTO>> addressDTOConvertFunc, ParameterExpression customer, ParameterExpression dto)
    {
        // Address[] addresses;
        var addresses = Expression.Parameter(typeof(Address[]), "addresses");
        // int length;
        var length = Expression.Variable(typeof(int), "length");
        // AddressDTO[] dtoList;
        var dtoList = Expression.Parameter(typeof(AddressDTO[]), "dtoList");
        //// Address item;
        //var item = Expression.Parameter(typeof(Address), "item");
        var forLabel = Expression.Label("forLabel");
        var i = Expression.Variable(typeof(int), "i");
        return Expression.Block(
            [addresses, dtoList, length, i],
            // addressList = customer.AddressList;
            Expression.Assign(addresses, Expression.Property(customer, "Addresses")),
            // length = addresses.Length;
            Expression.Assign(length, Expression.ArrayLength(addresses)),
            // dtoList = new AddressDTO[length];
            Expression.Assign(dtoList, Expression.NewArrayBounds(typeof(AddressDTO), length)),
            // dto.Addresses = dtoList;
            Expression.Assign(Expression.Property(dto, "Addresses"), dtoList),

            Expression.Loop(
                Expression.IfThenElse(
                    // i < addressCount
                    Expression.LessThan(i, length),

                    Expression.Block(
                        // dtoList[i] = addressDTOConvertFunc.Invoke(addressList[i]);
                        Expression.Assign(Expression.ArrayAccess(dtoList, i), Expression.Invoke(addressDTOConvertFunc, Expression.ArrayAccess(addresses, i))),
                        // i++;
                        Expression.PostIncrementAssign(i)
                    ),
                    Expression.Break(forLabel)
                ),
                forLabel)
        );
    }
    /// <summary>
    /// 转化 customer.AddressList -> dto.AddressList
    /// </summary>
    /// <param name="customer"></param>
    /// <param name="dto"></param>
    /// <returns></returns>
    public static BlockExpression ConvertAddressList(Expression<Func<Address, AddressDTO>> addressDTOConvertFunc, ParameterExpression customer, ParameterExpression dto)
    {
        // List<Address> addressList;
        var addressList = Expression.Parameter(typeof(List<Address>), "addressList");

        // List<AddressDTO> dtoList;
        var dtoList = Expression.Parameter(typeof(List<AddressDTO>), "dtoList");
        // int count;
        var count = Expression.Variable(typeof(int), "count");
        //// Address item;
        //var item = Expression.Parameter(typeof(Address), "item");
        var forLabel = Expression.Label("forLabel");
        var i = Expression.Variable(typeof(int), "i");
        return Expression.Block(
            [addressList, dtoList, count, i],
            // addressList = customer.AddressList;
            Expression.Assign(addressList, Expression.Property(customer, "AddressList")),
            // dtoList = new List<AddressDTO>();
            Expression.Assign(dtoList, Expression.New(typeof(List<AddressDTO>))),
            // dto.AddressList = dtoList;
            Expression.Assign(Expression.Property(dto, "AddressList"), dtoList),
            // addressCount = addressList.Count;
            Expression.Assign(count, Expression.Property(addressList, "Count")),
            Expression.Loop(
                Expression.IfThenElse(
                    // i < addressCount
                    Expression.LessThan(i, count),
                    // dtoList.Add(addressDTOConvertFunc.Invoke(addressList[i++]));
                    Expression.Call(
                        dtoList,
                        typeof(List<AddressDTO>).GetMethod("Add")!,
                        Expression.Invoke(addressDTOConvertFunc, Expression.MakeIndex(addressList, typeof(List<Address>).GetProperty("Item"), [Expression.PostIncrementAssign(i)]))),
                    Expression.Break(forLabel)
                ),
                forLabel)
        );
    }
    /// <summary>
    /// 定义转化委托 Address -> AddressDTO
    /// </summary>
    /// <returns></returns>
    public static Expression<Func<Address, AddressDTO>> CreateAddressDTO()
    {
        var sourceType = typeof(Address);
        var destType = typeof(AddressDTO);
        // Address source;
        var source = Expression.Parameter(sourceType, "source");
        // AddressDTO dest;
        var dest = Expression.Parameter(destType, "dest");
        var body = Expression.Block(
            [dest],
            // dest = new AddressDTO();
            Expression.Assign(dest, Expression.New(destType)),
            // dest.City = source.City;
            Expression.Assign(Expression.Property(dest, "City"), Expression.Property(source, "City")),
            // return dest;
            dest
        );
        return Expression.Lambda<Func<Address, AddressDTO>>(body, source);
    }
}
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
public class AddressDTO
{
    public string City { get; set; }
}
public class Address
{
    public string City { get; set; }
}