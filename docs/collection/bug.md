# 微软.net表达式编译居然有bug?

微软.net表达式编译问题困扰本人很久了,
为此我整理了以下case给大家分享

## 1. 可行性调研
>* 用表达式把一个对象转化为另一个对象
>* 当一个类含有多个同类型属性时,把相同类型转化提取为公共方法
>* LambdaExpression可以用来定义复用的公共方法
>* 一切看起来都很完美,但是居然翻车了！！！

## 2. 示例说明
### 2.1 Customer多个属性包含Address
>对应CustomerDTO多个属性包含AddressDTO

~~~csharp
public class Customer
{
    public string Name { get; set; }
    public Address Address { get; set; }
    public Address[] Addresses { get; set; }
    public List<Address> AddressList { get; set; }
}
~~~

~~~csharp
public class CustomerDTO
{
    public string Name { get; set; }
    public AddressDTO Address { get; set; }
    public AddressDTO[] Addresses { get; set; }
    public List<AddressDTO> AddressList { get; set; }
}
~~~

### 2.2 定义公共方法把Address转化为AddressDTO
~~~csharp
/// <summary>
/// 定义转化 Address -> AddressDTO
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
~~~

### 2.3 调用公共方法
~~~csharp
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
    // 可以复用的功能方法
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
~~~

以上看上去是不是很完美！！！
但是马上就要翻车了...


### 2.4 测试一下
~~~csharp
var expression = CreateCustomerDTO();
var func = expression.Compile();
Customer _customer = new()
{
    Name = "jxj",
    Address = new() { City = "gz" },
    AddressList = [new() { City = "bj" }],
    Addresses = [new() { City = "sh" }]
};
var dto = func(_customer);
// {"Name":"jxj","Address":{"City":"gz"},"Addresses":[{"City":"sh"}],"AddressList":[]}
~~~

#### 2.4.1 请大家围观翻车现场
>* Address和Addresses转化成功了,但是AddressList转化失败了
>* 如果说LambdaExpression不能复用,为什么Address和Addresses共用LambdaExpression能成功
>* 而且如果删掉Addresses属性AddressList就能转化成功

### 2.5 换成FastExpressionCompiler再测试一下
~~~csharp
var expression = CreateCustomerDTO();
var func = FastExpressionCompiler.ExpressionCompiler.CompileFast<Func<Customer, CustomerDTO>>(expression);
Customer _customer = new()
{
    Name = "jxj",
    Address = new() { City = "gz" },
    AddressList = [new() { City = "bj" }],
    Addresses = [new() { City = "sh" }]
};
var dto = func(_customer);
// {"Name":"jxj","Address":{"City":"gz"},"Addresses":[{"City":"sh"}],"AddressList":[{"City":"bj"}]}
~~~

换成FastExpressionCompiler全部成功,这是不是实锤是微软的bug


## 3. 附两个note对比示例
>* [expression_fast.dib](https://github.com/donetsoftwork/MyEmit/tree/main/Notes/expression_fast.dib)是微软转化失败示例
>* [expression_fast.dib](https://github.com/donetsoftwork/MyEmit/tree/main/Notes/expression_fast.dib)是FastExpressionCompiler转化成功示例
>* 大家可以下载本地执行

