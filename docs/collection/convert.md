# 类型转化
>* 调用[IMapper](xref:PocoEmit.IMapper)的方法来转化
>* IMapper继承[IPoco](xref:PocoEmit.IPoco)
>* 一般使用[Mapper](xref:PocoEmit.Mapper)的实例或Mapper.Global


## 一、转化方式
>* 调用Convert方法直接转化
>* 调用GetConvertFunc方法委托转化
>* 调用GetConverter方法接口转化
>* 以上继承自PocoEmit,与[PocoEmit转化方式](../poco/convert.md)一致

## 二、支持范围

### 1. 基础类型转化
>* 继承自PocoEmit,与[PocoEmit转化方式](../poco/convert.md)一致

### 2. 支持单参数转化
>* 继承自PocoEmit,与[PocoEmit转化方式](../poco/convert.md)一致

### 3. 支持可空类型
>* 继承自PocoEmit,与[PocoEmit转化方式](../poco/convert.md)一致

### 4. 支持集合类型转化
```csharp
User[] source = [new User { Id = 1, Name = "Jxj" }, new User { Id = 2, Name = "张三" }];
IList<UserDTO> result = _mapper.Convert<User[], IList<UserDTO>>(source);
```

```csharp
Dictionary<int, User> source = new() { { 1, new User { Id = 1, Name = "Jxj" } } };
Dictionary<int, UserDTO> result = _mapper.Convert<Dictionary<int, User>, Dictionary<int, UserDTO>>(source);
```

```csharp
List<User> source = [new User { Id = 1, Name = "Jxj" }, new User { Id = 2, Name = "张三" }];
Dictionary<int, UserDTO> result = _mapper.Convert<List<User>, Dictionary<int, UserDTO>>(source);
```

### 5. 支持成员转化
>* 继承自PocoEmit,与[PocoEmit转化方式](../poco/convert.md)一致

### 6. 成员复制器转化
>* 继承自PocoEmit.Mapper,与[Mapper转化方式](../mapper/convert.md)一致

### 7. 构造函数转化
>* 继承自PocoEmit.Mapper,与[Mapper转化方式](../mapper/convert.md)一致