# 婶可忍叔不可忍的AutoMapper的你还在用吗?

## AutoMapper是让人又爱又恨的项目
>* 爱它是因为它解决了一些问题,很多项目都有用,下载量很大,受众很广。
>* 恨它是因为它诸多反人类的设计。
>* 为此本人开源项目PocoEmit对标AutoMapper。

## 1. AutoMapper反人类设计
### 1.1 AutoMapper注册代码
```csharp
services.AddAutoMapper(cfg => cfg.CreateMap<User, UserDTO>());
```

User和UserDTO除了类名不一样,其他都一样,怎么看这行代码都多余。
需要转化的类型越多,多余的代码就越多。
类型转化不应该就是个静态方法吗?而且AutoMapper注册却依赖容器,Mapper对象也是从容器获取。
本人觉得AutoMapper设计的太反人类了。

### 1.2 PocoEmit对于大部分转化是不需要手动配置
>* PocoEmit可以轻松的定义静态实例。
>* PocoEmit静态实例可以用来定义静态委托字段,当静态方法用。
```csharp
UserDTO dto = PocoEmit.Mapper.Default.Convert<User, UserDTO>(new User());
```

```csharp
public static readonly Func<User, UserDTO> UserDTOConvert = PocoEmit.Mapper.Default.GetConvertFunc<User, UserDTO>();
```

## 2. AutoMapper的性能差强人意
### 2.1 以下是AutoMapper官网例子与PocoEmit.Mapper的对比
>* Customer转化为CustomerDTO(嵌套多个子对象、数组及列表)。
>* Auto是执行AutoMapper的IMapper.Map方法。
>* Poco是执行PocoEmit.Mapper的IMapper.Convert方法。
>* PocoFunc是执行PocoEmit.Mapper生成的委托。

| Method      | Mean     | Error    | StdDev   | Median   | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|------------ |---------:|---------:|---------:|---------:|------:|--------:|-------:|----------:|------------:|
| Auto        | 86.58 ns | 0.724 ns | 0.774 ns | 86.59 ns |  1.73 |    0.02 | 0.0260 |     448 B |        1.33 |
| Poco        | 50.00 ns | 0.401 ns | 0.462 ns | 49.93 ns |  1.00 |    0.01 | 0.0195 |     336 B |        1.00 |
| PocoFunc    | 30.38 ns | 0.084 ns | 0.097 ns | 30.38 ns |  0.61 |    0.01 | 0.0176 |     304 B |        0.90 |

>* Auto耗时比Poco多50%左右。
>* Auto耗时是PocoFunc的两倍多。

### 2.2 能不能用AutoMapper生成委托来提高性能呢
>* 既可以说能也可以说不能。
>* 说能是因为AutoMapper确实提供了该功能。
>* 说不能是因为AutoMapper没打算给用户用。

### 2.2.1 AutoMapper生成委托有点麻烦
```csharp
var configuration = _auto.ConfigurationProvider.Internal();
var mapRequest = new MapRequest(new TypePair(typeof(Customer), typeof(CustomerDTO)));
Func<Customer, CustomerDTO, ResolutionContext, CustomerDTO> autoFunc = configuration.GetExecutionPlan<Customer, CustomerDTO>(mapRequest);
```

作为对比PocoEmit.Mapper就简单的多了
```csharp
Func<Customer, CustomerDTO> pocoFunc = PocoEmit.Mapper.Default.GetConvertFunc<Customer, CustomerDTO>();
```

### 2.2.2 调用AutoMapper生成的委托更麻烦
>* 参数ResolutionContext没有公开的构造函数,也找不到公开的实例。
>* 只能通过反射获得ResolutionContext的实例。

```csharp
var field = typeof(AutoMapper.Mapper).GetField("_defaultContext", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
ResolutionContext resolutionContext = field.GetValue(_auto) as ResolutionContext;
```

### 2.2.3 加入AutoMapper生成委托再对比一下
| Method      | Mean     | Error    | StdDev   | Median   | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|------------ |---------:|---------:|---------:|---------:|------:|--------:|-------:|----------:|------------:|
| Auto        | 86.58 ns | 0.724 ns | 0.774 ns | 86.59 ns |  1.73 |    0.02 | 0.0260 |     448 B |        1.33 |
| AutoFunc    | 55.00 ns | 0.153 ns | 0.176 ns | 54.96 ns |  1.10 |    0.01 | 0.0260 |     448 B |        1.33 |
| Poco        | 50.00 ns | 0.401 ns | 0.462 ns | 49.93 ns |  1.00 |    0.01 | 0.0195 |     336 B |        1.00 |
| PocoFunc    | 30.38 ns | 0.084 ns | 0.097 ns | 30.38 ns |  0.61 |    0.01 | 0.0176 |     304 B |        0.90 |

>* AutoMapper生成委托确实也快了不少。
>* 从百分比来看即使不生成委托,AutoMapper也慢不了多少？没有数量级的区别,能忍? --- 反问句

### 2.3 简单类型转化对比
>* User转UserDTO,只有两个简单属性

| Method    | Mean      | Error     | StdDev    | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|---------- |----------:|----------:|----------:|------:|--------:|-------:|----------:|------------:|
| Auto      | 35.409 ns | 0.1376 ns | 0.1585 ns |  1.64 |    0.02 | 0.0019 |      32 B |        0.50 |
| AutoFunc  |  4.033 ns | 0.0970 ns | 0.1038 ns |  0.19 |    0.01 | 0.0019 |      32 B |        0.50 |
| Poco      | 21.659 ns | 0.2595 ns | 0.2777 ns |  1.00 |    0.02 | 0.0037 |      64 B |        1.00 |
| PocoFunc  |  3.659 ns | 0.0248 ns | 0.0276 ns |  0.17 |    0.00 | 0.0019 |      32 B |        0.50 |

>* Auto耗时是AutoFunc差不多十倍,差出一个数量级了(回答了前面的反问)
>* AutoFunc耗时比PocoFunc稍多,这说明AutoMapper复杂类型转化性能非常不好,简单类型转化可能还能凑合
>* 关键是性能好生成的委托AutoMapper不给用啊,“婶可忍叔不可忍”啊!

## 3. AutoMapper生成的代码能通过代码审核吗?
### 3.1 AutoMapper官网那个例子生成以下代码
```csharp
T __f<T>(System.Func<T> f) => f();
CustomerDTO _autoMap(Customer source, CustomerDTO destination, ResolutionContext context)
{
    return (source == null) ?
        (destination == null) ? (CustomerDTO)null : destination :
        __f(() => {
            CustomerDTO typeMapDestination = null;
            typeMapDestination = destination ?? new CustomerDTO();
            try
            {
                typeMapDestination.Id = source.Id;
            }
            catch (Exception ex)
            {
                throw TypeMapPlanBuilder.MemberMappingError(
                    ex,
                    default(PropertyMap)/*NOTE: Provide the non-default value for the Constant!*/);
            }
            try
            {
                typeMapDestination.Name = source.Name;
            }
            catch (Exception ex)
            {
                throw TypeMapPlanBuilder.MemberMappingError(
                    ex,
                    default(PropertyMap)/*NOTE: Provide the non-default value for the Constant!*/);
            }
            try
            {
                Address resolvedValue = null;
                Address mappedValue = null;
                resolvedValue = source.Address;
                mappedValue = (resolvedValue == null) ? (Address)null :
                    ((Func<Address, Address, ResolutionContext, Address>)((
                        Address source_1,
                        Address destination_1,
                        ResolutionContext context) => //Address
                        (source_1 == null) ?
                            (destination_1 == null) ? (Address)null : destination_1 :
                            __f(() => {
                                Address typeMapDestination_1 = null;
                                typeMapDestination_1 = destination_1 ?? new Address();
                                try
                                {
                                    typeMapDestination_1.Id = source_1.Id;
                                }
                                catch (Exception ex)
                                {
                                    throw TypeMapPlanBuilder.MemberMappingError(
                                        ex,
                                        default(PropertyMap)/*NOTE: Provide the non-default value for the Constant!*/);
                                }
                                try
                                {
                                    typeMapDestination_1.Street = source_1.Street;
                                }
                                catch (Exception ex)
                                {
                                    throw TypeMapPlanBuilder.MemberMappingError(
                                        ex,
                                        default(PropertyMap)/*NOTE: Provide the non-default value for the Constant!*/);
                                }
                                try
                                {
                                    typeMapDestination_1.City = source_1.City;
                                }
                                catch (Exception ex)
                                {
                                    throw TypeMapPlanBuilder.MemberMappingError(
                                        ex,
                                        default(PropertyMap)/*NOTE: Provide the non-default value for the Constant!*/);
                                }
                                try
                                {
                                    typeMapDestination_1.Country = source_1.Country;
                                }
                                catch (Exception ex)
                                {
                                    throw TypeMapPlanBuilder.MemberMappingError(
                                        ex,
                                        default(PropertyMap)/*NOTE: Provide the non-default value for the Constant!*/);
                                }
                                return typeMapDestination_1;
                            })))
                    .Invoke(
                        resolvedValue,
                        (destination == null) ? (Address)null :
                            typeMapDestination.Address,
                        context);
                typeMapDestination.Address = mappedValue;
            }
            catch (Exception ex)
            {
                throw TypeMapPlanBuilder.MemberMappingError(
                    ex,
                    default(PropertyMap)/*NOTE: Provide the non-default value for the Constant!*/);
            }
            try
            {
                Address resolvedValue_1 = null;
                AddressDTO mappedValue_1 = null;
                resolvedValue_1 = source.HomeAddress;
                mappedValue_1 = (resolvedValue_1 == null) ? (AddressDTO)null :
                    ((Func<Address, AddressDTO, ResolutionContext, AddressDTO>)((
                        Address source_2,
                        AddressDTO destination_2,
                        ResolutionContext context) => //AddressDTO
                        (source_2 == null) ?
                            (destination_2 == null) ? (AddressDTO)null : destination_2 :
                            __f(() => {
                                AddressDTO typeMapDestination_2 = null;
                                typeMapDestination_2 = destination_2 ?? new AddressDTO();
                                try
                                {
                                    typeMapDestination_2.Id = source_2.Id;
                                }
                                catch (Exception ex)
                                {
                                    throw TypeMapPlanBuilder.MemberMappingError(
                                        ex,
                                        default(PropertyMap)/*NOTE: Provide the non-default value for the Constant!*/);
                                }
                                try
                                {
                                    typeMapDestination_2.City = source_2.City;
                                }
                                catch (Exception ex)
                                {
                                    throw TypeMapPlanBuilder.MemberMappingError(
                                        ex,
                                        default(PropertyMap)/*NOTE: Provide the non-default value for the Constant!*/);
                                }
                                try
                                {
                                    typeMapDestination_2.Country = source_2.Country;
                                }
                                catch (Exception ex)
                                {
                                    throw TypeMapPlanBuilder.MemberMappingError(
                                        ex,
                                        default(PropertyMap)/*NOTE: Provide the non-default value for the Constant!*/);
                                }
                                return typeMapDestination_2;
                            })))
                    .Invoke(
                        resolvedValue_1,
                        (destination == null) ? (AddressDTO)null :
                            typeMapDestination.HomeAddress,
                        context);
                typeMapDestination.HomeAddress = mappedValue_1;
            }
            catch (Exception ex)
            {
                throw TypeMapPlanBuilder.MemberMappingError(
                    ex,
                    default(PropertyMap)/*NOTE: Provide the non-default value for the Constant!*/);
            }
            try
            {
                Address[] resolvedValue_2 = null;
                AddressDTO[] mappedValue_2 = null;
                resolvedValue_2 = source.Addresses;
                mappedValue_2 = (resolvedValue_2 == null) ?
                    Array.Empty<AddressDTO>() :
                    __f(() => {
                        AddressDTO[] destinationArray = null;
                        int destinationArrayIndex = default;
                        destinationArray = new AddressDTO[resolvedValue_2.Length];
                        destinationArrayIndex = default(int);
                        int sourceArrayIndex = default;
                        Address sourceItem = null;
                        sourceArrayIndex = default(int);
                        while (true)
                        {
                            if ((sourceArrayIndex < resolvedValue_2.Length))
                            {
                                sourceItem = resolvedValue_2[sourceArrayIndex];
                                destinationArray[destinationArrayIndex++] = ((Func<Address, AddressDTO, ResolutionContext, AddressDTO>)((
                                    Address source_2,
                                    AddressDTO destination_2,
                                    ResolutionContext context) => //AddressDTO
                                    (source_2 == null) ?
                                        (destination_2 == null) ? (AddressDTO)null : destination_2 :
                                        __f(() => {
                                            AddressDTO typeMapDestination_2 = null;
                                            typeMapDestination_2 = destination_2 ?? new AddressDTO();
                                            try
                                            {
                                                typeMapDestination_2.Id = source_2.Id;
                                            }
                                            catch (Exception ex)
                                            {
                                                throw TypeMapPlanBuilder.MemberMappingError(
                                                    ex,
                                                    default(PropertyMap)/*NOTE: Provide the non-default value for the Constant!*/);
                                            }
                                            try
                                            {
                                                typeMapDestination_2.City = source_2.City;
                                            }
                                            catch (Exception ex)
                                            {
                                                throw TypeMapPlanBuilder.MemberMappingError(
                                                    ex,
                                                    default(PropertyMap)/*NOTE: Provide the non-default value for the Constant!*/);
                                            }
                                            try
                                            {
                                                typeMapDestination_2.Country = source_2.Country;
                                            }
                                            catch (Exception ex)
                                            {
                                                throw TypeMapPlanBuilder.MemberMappingError(
                                                    ex,
                                                    default(PropertyMap)/*NOTE: Provide the non-default value for the Constant!*/);
                                            }
                                            return typeMapDestination_2;
                                        })))
                                .Invoke(
                                    sourceItem,
                                    (AddressDTO)null,
                                    context);
                                sourceArrayIndex++;
                            }
                            else
                            {
                                goto LoopBreak;
                            }
                        }
                    LoopBreak:;
                        return destinationArray;
                    });
                typeMapDestination.Addresses = mappedValue_2;
            }
            catch (Exception ex)
            {
                throw TypeMapPlanBuilder.MemberMappingError(
                    ex,
                    default(PropertyMap)/*NOTE: Provide the non-default value for the Constant!*/);
            }
            try
            {
                List<Address> resolvedValue_3 = null;
                List<AddressDTO> mappedValue_3 = null;
                resolvedValue_3 = source.WorkAddresses;
                mappedValue_3 = (resolvedValue_3 == null) ?
                    new List<AddressDTO>() :
                    __f(() => {
                        List<AddressDTO> collectionDestination = null;
                        List<AddressDTO> passedDestination = null;
                        passedDestination = (destination == null) ? (List<AddressDTO>)null :
                            typeMapDestination.WorkAddresses;
                        collectionDestination = passedDestination ?? new List<AddressDTO>();
                        collectionDestination.Clear();
                        List<Address>.Enumerator enumerator = default;
                        Address item = null;
                        enumerator = resolvedValue_3.GetEnumerator();
                        try
                        {
                            while (true)
                            {
                                if (enumerator.MoveNext())
                                {
                                    item = enumerator.Current;
                                    collectionDestination.Add(((Func<Address, AddressDTO, ResolutionContext, AddressDTO>)((
                                        Address source_2,
                                        AddressDTO destination_2,
                                        ResolutionContext context) => //AddressDTO
                                        (source_2 == null) ?
                                            (destination_2 == null) ? (AddressDTO)null : destination_2 :
                                            __f(() => {
                                                AddressDTO typeMapDestination_2 = null;
                                                typeMapDestination_2 = destination_2 ?? new AddressDTO();
                                                try
                                                {
                                                    typeMapDestination_2.Id = source_2.Id;
                                                }
                                                catch (Exception ex)
                                                {
                                                    throw TypeMapPlanBuilder.MemberMappingError(
                                                        ex,
                                                        default(PropertyMap)/*NOTE: Provide the non-default value for the Constant!*/);
                                                }
                                                try
                                                {
                                                    typeMapDestination_2.City = source_2.City;
                                                }
                                                catch (Exception ex)
                                                {
                                                    throw TypeMapPlanBuilder.MemberMappingError(
                                                        ex,
                                                        default(PropertyMap)/*NOTE: Provide the non-default value for the Constant!*/);
                                                }
                                                try
                                                {
                                                    typeMapDestination_2.Country = source_2.Country;
                                                }
                                                catch (Exception ex)
                                                {
                                                    throw TypeMapPlanBuilder.MemberMappingError(
                                                        ex,
                                                        default(PropertyMap)/*NOTE: Provide the non-default value for the Constant!*/);
                                                }
                                                return typeMapDestination_2;
                                            })))
                                    .Invoke(
                                        item,
                                        (AddressDTO)null,
                                        context));
                                }
                                else
                                {
                                    goto LoopBreak_1;
                                }
                            }
                        LoopBreak_1:;
                        }
                        finally
                        {
                            enumerator.Dispose();
                        }
                        return collectionDestination;
                    });
                typeMapDestination.WorkAddresses = mappedValue_3;
            }
            catch (Exception ex)
            {
                throw TypeMapPlanBuilder.MemberMappingError(
                    ex,
                    default(PropertyMap)/*NOTE: Provide the non-default value for the Constant!*/);
            }
            return typeMapDestination;
        });
}    
```

### 3.2 以下是PocoEmit.Mapper生成的代码
```csharp
CustomerDTO _pocoConvert(Customer source)
{
    CustomerDTO dest = null;
    if ((source != (Customer)null))
    {
        dest = new CustomerDTO();
        Address member0 = null;
        Address member1 = null;
        Address[] member2 = null;
        List<Address> member3 = null;
        dest.Id = source.Id;
        dest.Name = source.Name;
        member0 = source.Address;
        if ((member0 != null))
        {
            dest.Address = member0;
        }
        member1 = source.HomeAddress;
        if ((member1 != null))
        {
            // { The block result will be assigned to `dest.HomeAddress`
            AddressDTO dest_1 = null;
            if ((member1 != (Address)null))
            {
                dest_1 = new AddressDTO();
                dest_1.Id = member1.Id;
                dest_1.City = member1.City;
                dest_1.Country = member1.Country;
            }
            dest.HomeAddress = dest_1;
            // } end of block assignment;
        }
        member2 = source.Addresses;
        if ((member2 != null))
        {
            // { The block result will be assigned to `dest.Addresses`
            int count = default;
            AddressDTO[] dest_2 = null;
            int index = default;
            Address sourceItem = null;
            count = member2.Length;
            dest_2 = new AddressDTO[count];
            while (true)
            {
                if ((index < count))
                {
                    sourceItem = member2[index];
                    // { The block result will be assigned to `dest_2[index]`
                    AddressDTO dest_3 = null;
                    if ((sourceItem != (Address)null))
                    {
                        dest_3 = new AddressDTO();
                        dest_3.Id = sourceItem.Id;
                        dest_3.City = sourceItem.City;
                        dest_3.Country = sourceItem.Country;
                    }
                    dest_2[index] = dest_3;
                    // } end of block assignment
                    index++;
                }
                else
                {
                    goto forLabel;
                }
            }
            forLabel:;
            dest.Addresses = dest_2;
            // } end of block assignment;
        }
        member3 = source.WorkAddresses;
        if ((member3 != null))
        {
            // { The block result will be assigned to `dest.WorkAddresses`
            List<AddressDTO> dest_4 = null;
            dest_4 = new List<AddressDTO>(member3.Count);
            dest_4;
            int index_1 = default;
            int len = default;
            len = member3.Count;
            while (true)
            {
                if ((index_1 < len))
                {
                    Address sourceItem_1 = null;
                    AddressDTO destItem = null;
                    sourceItem_1 = member3[index_1];
                    // { The block result will be assigned to `destItem`
                        AddressDTO dest_5 = null;
                        if ((sourceItem_1 != (Address)null))
                        {
                            dest_5 = new AddressDTO();
                            dest_5.Id = sourceItem_1.Id;
                            dest_5.City = sourceItem_1.City;
                            dest_5.Country = sourceItem_1.Country;
                        }
                        destItem = dest_5;
                        // } end of block assignment;
                    dest_4.Add(destItem);
                    index_1++;
                }
                else
                {
                    goto forLabel_1;
                }
            }
            forLabel_1:;
            dest.WorkAddresses = dest_4;
            // } end of block assignment;
        }
        CustomerConvertBench.ConvertAddressCity(
            source,
            dest);
    }
    return dest;
}
```

### 3.3 简单对比如下
>* AutoMapper生成代码三百多行,PocoEmit.Mapper一百多行,AutoMapper代码量是两倍以上
>* AutoMapper生成大量try catch,哪怕是int对int赋值也要try
>* AutoMapper用迭代器Enumerator访问列表,PocoEmit.Mapper用索引器
>* AutoMapper这些区别应该是导致性能差的部分原因

### 3.4 如何获取AutoMapper生成的代码
```csharp
LambdaExpression expression = _auto.ConfigurationProvider.BuildExecutionPlan(typeof(Customer), typeof(CustomerDTO));   
```

#### 3.4.1 如果要查看更可读的代码推荐使用FastExpressionCompiler
>* 可以使用nuget安装
>* 前面的例子就是使用FastExpressionCompiler再手动整理了一下

```csharp
string code = FastExpressionCompiler.ToCSharpPrinter.ToCSharpString(expression);
```

#### 3.4.2 PocoEmit获取生成代码更简单
```csharp
Expression<Func<Customer, CustomerDTO>> expression =  PocoEmit.Mapper.Default.BuildConverter<Customer, CustomerDTO>();
string code = FastExpressionCompiler.ToCSharpPrinter.ToCSharpString(expression);
```

#### 3.4.3 PocoEmit生成代码扩展性
>* PocoEmit可以获取委托表达式自己来编译委托
>* PocoEmit通过PocoEmit.Builders.Compiler.Instance来编译,可以对Instance进行覆盖来扩展
>* 通过实现Compiler类来扩展,只需要重写CompileFunc和CompileAction两个方法
>* 可以使用FastExpressionCompiler来实现Compiler类


## 4. AutoMapper枚举逻辑问题
```csharp
public enum MyColor
{
    None = 0,
    Red = 1,
    Green = 2,
    Blue = 3,
}
ConsoleColor color = ConsoleColor.DarkBlue;
// Red
MyColor autoColor = _auto.Map<ConsoleColor, MyColor>(color);
// None
MyColor pocoColor = PocoEmit.Mapper.Default.Convert<ConsoleColor, MyColor>(color);
```

>* AutoMapper先按枚举名转化,失败再按值转化,不支持的DarkBlue被AutoMapper转化为Red
>* 不同类型的枚举值转化没有意义,定义枚举可以不指定值
>* AutoMapper这完全是犯了画蛇添足的错误
>* AutoMapper还有哪些槽点欢迎大家在评论区指出

## 5. PocoEmit可扩展架构
### 5.1 nuget安装PocoEmit可获得基础功能
>* 通过PocoEmit可以读写实体的属性
>* PocoEmit可以通过PocoEmit.Poco转化基础类型和枚举
>* PocoEmit.Poco支持注册转化表达式

### 5.2 nuget安装PocoEmit.Mapper获得更多功能
>* PocoEmit.Mapper可以支持PocoEmit.Poco的所有功能
>* PocoEmit.Mapper可以支持自定义实体类型(不支持集合(含数组、列表及字典)成员)的转化和复制

### 5.3 nuget安装PocoEmit.Collections扩展集合功能
>* 通过UseCollection扩展方法给PocoEmit.Mapper增加集合功能
>* 扩展后PocoEmit.Mapper支持集合(含数组、列表及字典)的转化和复制
>* 支持实体类型包含集合成员的转化和复制
>* 嫌麻烦的同学可以直接安装PocoEmit.Collections并配置UseCollection

源码托管地址: https://github.com/donetsoftwork/MyEmit ，也欢迎大家直接查看源码。
gitee同步更新:https://gitee.com/donetsoftwork/MyEmit

如果大家喜欢请动动您发财的小手手帮忙点一下Star。