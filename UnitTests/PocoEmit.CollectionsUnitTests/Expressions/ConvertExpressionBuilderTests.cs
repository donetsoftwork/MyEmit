using PocoEmit.CollectionsUnitTests.Supports;
using System.Linq.Expressions;
using System.Reflection;

namespace PocoEmit.CollectionsUnitTests.Expressions;

public class ConvertExpressionBuilderTests
{
    private static Customer _customer = LambdaExpressionTests.GetCustomer();
    [Fact]
    public void BySys()
    {
        var expression = CreateCustomerDTO();
        var func = expression.Compile();
        Assert.NotNull(func);
        var dto = func(_customer);
        var info = func.GetMethodInfo();
        Assert.NotNull(dto);
        // Console.ReadLine();
        Assert.NotNull(dto.Address);
        Assert.Equal(_customer.AddressList.Count, dto.AddressList.Count);
    }
    [Fact]
    public void ByFast()
    {
        var expression = CreateCustomerDTO();
        var code = FastExpressionCompiler.ToCSharpPrinter.ToCSharpString(expression);
        Assert.NotNull(code);
        var func = FastExpressionCompiler.ExpressionCompiler.CompileFast<Func<Customer, CustomerDTO>>(expression);
        Assert.NotNull(func);
        var dto = func(_customer);
        Assert.NotNull(dto);
        Assert.NotNull(dto.Address);
        Assert.Equal(_customer.AddressList.Count, dto.AddressList.Count);
    }
    [Fact]
    public void ByToDTO()
    {
        var dto = ToDTO(_customer);
        Assert.NotNull(dto);
        Assert.NotNull(dto.Address);
        Assert.Equal(_customer.AddressList.Count, dto.AddressList.Count);
    }
    [Fact]
    public void Convert()
    {
        var customer = new Customer() { Address = new() {  City ="bj"} };
        var dto = new CustomerDTO();
        var copier = CreateAddressCopier();
        copier(customer, dto);
        var address = dto.Address;
        Assert.NotNull(address);
        Assert.Equal(customer.Address.City, address.City);
    }
    public static CustomerDTO ToDTO(Customer customer)
    {
        var dto = new CustomerDTO();
        dto.Name = customer.Name;
        var source2 = customer.Address;
        var dest2 = new AddressDTO();
        dest2.City = source2.City;
        dto.Address = dest2;
        int dtoArrayIndex = 0;
        var addresses = customer.Addresses;
        var length = addresses.Length;
        var dtoArray = new AddressDTO[length];
        dto.Addresses = dtoArray;
        while (true)
        {
            if (dtoArrayIndex < length)
            {
                var source2_1 = addresses[dtoArrayIndex];
                var dest2_1 = new AddressDTO();
                dest2_1.City = source2_1.City;
                dtoArray[dtoArrayIndex] = dest2_1;
                dtoArrayIndex++;
            }
            else
            {
                break;
            }
        }
        List<Address> addressList;
        List<AddressDTO> dtoList;
        int count, i = 0;
        AddressDTO item;
        addressList = customer.AddressList;
        dtoList = new List<AddressDTO>();
        dto.AddressList = dtoList;
        count = addressList.Count;
        while (true)
        {
            if (i < count)
            {
                Address source2_2;
                source2_2 = addressList[i];
                item = new AddressDTO();
                item.City = source2_2.City;
                dtoList.Add(item);
                i++;
            }
            else
            {
                break;
            }
        }
        return dto;
    }


    private Action<Customer, CustomerDTO> CreateAddressCopier()
    {
        var sourceType = typeof(Customer);
        var destType = typeof(CustomerDTO);
        var source = Expression.Parameter(sourceType, "source");
        var dest = Expression.Parameter(destType, "dest");
        var builder = CreateAddressBuilder();
        var converter = builder.Convert(Expression.Property(source, "Address"), Expression.Property(dest, "Address"));
        //var body = Expression.Block(
        //    [source, dest],
        //    converter);
        var lambda = Expression.Lambda<Action<Customer, CustomerDTO>>(converter, source, dest);
        var code = FastExpressionCompiler.ToCSharpPrinter.ToCSharpString(lambda);
        Assert.NotNull(code);
        return lambda.Compile();
    }
    /// <summary>
    /// 转化 Customer -> CustomerDTO
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
        var addressBuilder = CreateAddressBuilder();
        var body = Expression.Block(
            [dto],
            // dto = new AddressDTO();
            Expression.Assign(dto, Expression.New(dtoType)),
            // dto.Name = customer.Name;
            Expression.Assign(Expression.Property(dto, "Name"), Expression.Property(customer, "Name")),
            // dto.Address = addressDTOConvertFunc.Invoke(customer.Address);
            ConvertAddress(addressBuilder, customer, dto),
            // dto.Addresses
            ConvertAddresses(addressBuilder, customer, dto),
            // dto.AddressList
            ConvertAddressList(addressBuilder, customer, dto),
            // return dto
            dto
        );
        return Expression.Lambda<Func<Customer, CustomerDTO>>(body, customer);
    }
    /// <summary>
    /// 转化 customer.Address -> dto.Address
    /// </summary>
    /// <param name="addressBuilder">共用方法</param>
    /// <param name="customer"></param>
    /// <param name="dto"></param>
    /// <returns></returns>
    public static Expression ConvertAddress(ConvertExpressionBuilder addressBuilder, ParameterExpression customer, ParameterExpression dto)
    {
        // addressBuilder.Convert(customer.Address, dto.Address);
        return addressBuilder.Convert(Expression.Property(customer, "Address"), Expression.Property(dto, "Address"));
    }
    /// <summary>
    /// 转化 customer.Addresses -> dto.Addresses
    /// </summary>
    /// <param name="addressDTOConvertFunc">共用方法</param>
    /// <param name="customer"></param>
    /// <param name="dto"></param>
    /// <returns></returns>
    public static BlockExpression ConvertAddresses(ConvertExpressionBuilder addressBuilder, ParameterExpression customer, ParameterExpression dto)
    {
        // Address[] addresses;
        var addresses = Expression.Parameter(typeof(Address[]), "addresses");
        // int length;
        var length = Expression.Variable(typeof(int), "length");
        // AddressDTO[] dtoArray;
        var dtoArray = Expression.Parameter(typeof(AddressDTO[]), "dtoArray");
        //// Address item;
        //var item = Expression.Parameter(typeof(Address), "item");
        var addressesLabel = Expression.Label("addressesLabel");
        // int dtoArrayIndex;
        var dtoArrayIndex = Expression.Variable(typeof(int), "dtoArrayIndex");
        return Expression.Block(
            [addresses, dtoArray, length, dtoArrayIndex],
            // addresses = customer.Addresses;
            Expression.Assign(addresses, Expression.Property(customer, "Addresses")),
            // length = addresses.Length;
            Expression.Assign(length, Expression.ArrayLength(addresses)),
            // dtoArray = new AddressDTO[length];
            Expression.Assign(dtoArray, Expression.NewArrayBounds(typeof(AddressDTO), length)),
            // dto.Addresses = dtoArray;
            Expression.Assign(Expression.Property(dto, "Addresses"), dtoArray),

            Expression.Loop(
                Expression.IfThenElse(
                    // dtoArrayIndex < length
                    Expression.LessThan(dtoArrayIndex, length),
                    Expression.Block(
                        // addressBuilder.Convert(addresses[dtoArrayIndex], dtoArray[dtoArrayIndex]);
                        addressBuilder.Convert(Expression.ArrayAccess(addresses, dtoArrayIndex), Expression.ArrayAccess(dtoArray, dtoArrayIndex)),
                        // dtoArrayIndex++;
                        Expression.PostIncrementAssign(dtoArrayIndex)
                    ),
                    Expression.Break(addressesLabel)
                ),
                addressesLabel)
        );
    }
    /// <summary>
    /// 转化 customer.AddressList -> dto.AddressList
    /// </summary>
    /// <param name="addressDTOConvertFunc">共用方法</param>
    /// <param name="customer"></param>
    /// <param name="dto"></param>
    /// <returns></returns>
    public static BlockExpression ConvertAddressList(ConvertExpressionBuilder addressBuilder, ParameterExpression customer, ParameterExpression dto)
    {
        // List<Address> addressList;
        var addressList = Expression.Parameter(typeof(List<Address>), "addressList");

        // List<AddressDTO> dtoList;
        var dtoList = Expression.Parameter(typeof(List<AddressDTO>), "dtoList");
        // int count;
        var count = Expression.Variable(typeof(int), "count");
        //// AddressDTO item;
        var item = Expression.Parameter(typeof(AddressDTO), "item");
        var forLabel = Expression.Label("forLabel");
        var i = Expression.Variable(typeof(int), "i");
        return Expression.Block(
            [addressList, dtoList, count, i, item],
            // addressList = customer.AddressList;
            Expression.Assign(addressList, Expression.Property(customer, "AddressList")),
            // dtoList = new List<AddressDTO>();
            Expression.Assign(dtoList, Expression.New(typeof(List<AddressDTO>))),
            // dto.AddressList = dtoList;
            Expression.Assign(Expression.Property(dto, "AddressList"), dtoList),
            // addressCount = addressList.Count;
            Expression.Assign(count, Expression.Property(addressList, "Count")),
            Expression.Assign(i, Expression.Constant(0)),
            Expression.Loop(
                Expression.IfThenElse(
                    // i < addressCount
                    Expression.LessThan(i, count),
                    // dtoList.Add(addressDTOConvertFunc.Invoke(addressList[i++]));
                    Expression.Block(
                        // addressBuilder.Convert(addressList[i], item);
                        addressBuilder.Convert(Expression.MakeIndex(addressList, typeof(List<Address>).GetProperty("Item"), [i]), item),
                        Expression.Call(
                            dtoList,
                            typeof(List<AddressDTO>).GetMethod("Add")!,
                            item
                        ),
                        // i++;
                        Expression.PostIncrementAssign(i)
                    ),
                    Expression.Break(forLabel)
                ),
                forLabel)
        );
    }
    private static ConvertExpressionBuilder CreateAddressBuilder()
    {
        var sourceType = typeof(Address);
        var destType = typeof(AddressDTO);
        var source = Expression.Parameter(sourceType, "source");
        var dest = Expression.Parameter(destType, "dest");
        var activator = Expression.Assign(dest, Expression.New(destType));
        var city = Expression.Assign(Expression.Property(dest, "City"), Expression.Property(source, "City"));
        return new(source, dest, [activator, city]);
    }
    [Fact]
    public void Block()
    {
        var sum = CreateBlockFunc();
        var a = 1;
        var b = 2;
        var c = sum(a, b);
        Assert.Equal(a + b, c);
    }
    private static Func<int, int, int> CreateBlockFunc()
    {
        var a = Expression.Variable(typeof(int), "a");
        var b = Expression.Variable(typeof(int), "b");
        var result = Expression.Variable(typeof(int), "result");
        var sum = Expression.Variable(typeof(int), "sum");
        var add = Expression.Block(
            [sum],
            Expression.Assign(sum, Expression.Add(a, b)),
            Expression.Assign(result, sum)
            );
        var body = Expression.Block([result], add, result);
        var lambda = Expression.Lambda<Func<int, int, int>>(body, a, b);
        var func = lambda.Compile();
        return func;
    }
}


