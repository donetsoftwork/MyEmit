using PocoEmit.Builders;
using PocoEmit.CollectionsUnitTests.Supports;
using PocoEmit.Visitors;
using System.Linq.Expressions;

namespace PocoEmit.CollectionsUnitTests.Expressions;

public class LambdaExpressionTests : CollectionTestBase
{
    private static Customer _customer = GetCustomer();
    [Fact]
    public void ByPoco()
    {
        Expression<Func<Customer, CustomerDTO>> expression = _mapper.BuildConverter<Customer, CustomerDTO>();
        var code = FastExpressionCompiler.ToCSharpPrinter.ToCSharpString(expression);
        Assert.NotNull(code);
        var func = expression.Compile();
        Assert.NotNull(func);
        var dto = func(_customer);
        Assert.NotNull(dto);
    }
    [Fact]
    public void BySys()
    {
        var expression = CreateCustomerDTO();
        var func = expression.Compile();
        Assert.NotNull(func);
        var dto = func(_customer);
        //var info = func.GetMethodInfo();
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
        //LambdaExpression lambda = expression;
        //FastExpressionCompiler.ExpressionCompiler.CompileFast(lambda);
        var dto = func(_customer);
        Assert.NotNull(dto);
        Assert.NotNull(dto.Address);
        Assert.Equal(_customer.AddressList.Count, dto.AddressList.Count);
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
        var addressDTOConvertFunc = CreateAddressDTO();
        var body = Expression.Block(
            [dto],
            // dto = new AddressDTO();
            Expression.Assign(dto, Expression.New(dtoType)),
            // dto.Name = customer.Name;
            Expression.Assign(Expression.Property(dto, "Name"), Expression.Property(customer, "Name")),
            // dto.AddressList
            ConvertAddressList(addressDTOConvertFunc, customer, dto),
            // dto.Addresses
            ConvertAddresses(addressDTOConvertFunc, customer, dto),
            // dto.Address = addressDTOConvertFunc.Invoke(customer.Address);
            ConvertAddress(addressDTOConvertFunc, customer, dto),

            // return dto
            dto
        );
        return Expression.Lambda<Func<Customer, CustomerDTO>>(body, customer);
    }
    public static Expression CallFunc5<TArgument, TResult>(Expression<Func<Address, AddressDTO>> func, Expression argument, string destName)
    {
        var customer = Expression.Parameter(typeof(Address), "source");
        var checkFun = func.Update(func.Body, [customer]);
        return Expression.Invoke(checkFun, argument);
    }
    public static Expression CallFunc4<TArgument, TResult>(Expression<Func<Address, AddressDTO>> func, Expression argument, string destName)
    {
        var replace = new ReplaceVisitor(func.Parameters[0], argument);
        Expression expression = replace.Visit(func.Body);
        return expression;
    }
    public static Expression CallFunc6<TArgument, TResult>(Expression<Func<Address, AddressDTO>> func, Expression argument, string destName)
    {
        return Expression.Invoke(func, argument);
        //return Compiler.Instance.Call(func, argument);
    }
    public static Expression CallFunc<TArgument, TResult>(Expression<Func<Address, AddressDTO>> func, Expression argument, string destName)
    {
        return Expression.Invoke(func, argument);
    }
    public static Expression CallFunc2<TArgument, TResult>(Expression<Func<Address, AddressDTO>> func, Expression argument, string destName)
    {
        //var method = func.GetMethodInfo();
        //if (method.IsStatic)
        //    return Expression.Call(null, method, Expression.Constant(func.Target), argument);
        //else
        //    return Expression.Call(Expression.Constant(func.Target), method, argument);

        //return Expression.Invoke(func, argument);
        var body = (BlockExpression)func.Body;
        //var parameters = body.Variables;
        var dest = Expression.Parameter(typeof(AddressDTO), destName);

        var replace = new ComplexReplaceVisitor(new ReplaceVisitor(func.Parameters[0], argument), body.Variables[0], dest);
        Expression expression = replace.Visit(body);
        //var replace = new ReplaceVisitor(body.Variables[0], dest);
        //Expression expression = replace.Visit(body);
        //var replace2 = new ReplaceVisitor(func.Parameters[0], argument);
        //expression = replace2.Visit(expression);
        return expression;
    }
    public static Expression CallFunc3<TArgument, TResult>(Expression<Func<Address, AddressDTO>> func, Expression argument, string destName)
    {
        var destType = typeof(AddressDTO);
        // Address source;
        //var source = Expression.Parameter(sourceType, "source");
        // AddressDTO dest;
        var dest = Expression.Parameter(destType, "dest");
        return Expression.Block(
            [dest],
            // dest = new AddressDTO();
            Expression.Assign(dest, Expression.New(destType)),
            // dest.City = source.City;
            Expression.Assign(Expression.Property(dest, "City"), Expression.Property(argument, "City")),
            // return dest;
            dest
        );
    }
    /// <summary>
    /// 转化 customer.Address -> dto.Address
    /// </summary>
    /// <param name="addressDTOConvertFunc">共用方法</param>
    /// <param name="customer"></param>
    /// <param name="dto"></param>
    /// <returns></returns>
    public static Expression ConvertAddress(Expression<Func<Address, AddressDTO>> addressDTOConvertFunc, ParameterExpression customer, ParameterExpression dto)
    {
        // dto.Address = addressDTOConvertFunc.Invoke(customer.Address);
        return Expression.Assign(Expression.Property(dto, "Address"), CallFunc<Address, AddressDTO>(addressDTOConvertFunc, Expression.Property(customer, "Address"), "a"));        
    }
    /// <summary>
    /// 转化 customer.Addresses -> dto.Addresses
    /// </summary>
    /// <param name="addressDTOConvertFunc">共用方法</param>
    /// <param name="customer"></param>
    /// <param name="dto"></param>
    /// <returns></returns>
    public static BlockExpression ConvertAddresses(Expression<Func<Address, AddressDTO>> addressDTOConvertFunc, ParameterExpression customer, ParameterExpression dto)
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
            Expression.Assign(dtoArrayIndex, Expression.Constant(0)),
            // dto.Addresses = dtoArray;
            Expression.Assign(Expression.Property(dto, "Addresses"), dtoArray),
            Expression.Assign(dtoArrayIndex, Expression.Constant(0)),
            Expression.Loop(
                Expression.IfThenElse(
                    // dtoArrayIndex < length
                    Expression.LessThan(dtoArrayIndex, length),                    
                    Expression.Block(
                        // dtoList[i] = addressDTOConvertFunc.Invoke(addressList[i]);
                        Expression.Assign(Expression.ArrayAccess(dtoArray, dtoArrayIndex), CallFunc<Address, AddressDTO>(addressDTOConvertFunc, Expression.ArrayAccess(addresses, dtoArrayIndex), "b")),
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
    public static BlockExpression ConvertAddressList(Expression<Func<Address, AddressDTO>> addressDTOConvertFunc, ParameterExpression customer, ParameterExpression dto)
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
                        Expression.Assign(item, CallFunc<Address, AddressDTO>(addressDTOConvertFunc, Expression.MakeIndex(addressList, typeof(List<Address>).GetProperty("Item"), [i]), "c")),
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
    /// <summary>
    /// 转化 Address -> AddressDTO
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
    public static Customer GetCustomer()
    {
        return new()
        {
            Name = "张三",
            Address = new() { City = "广州" },
            AddressList = [new() { City = "北京" }],
            Addresses = [new() { City = "上海" }]
        };
    }
}
