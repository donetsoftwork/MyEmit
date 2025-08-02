# 类型转化
>* 调用[IMapperOptions](xref:PocoEmit.Configuration.IMapperOptions)的方法来转化
>* IMapperOptions继承[IPocoOptions](xref:PocoEmit.Configuration.IPocoOptions)
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

### 4. 支持成员转化
>* 如果源类型含目标类型的成员,按该成员转化

```csharp
class MyMapperId(int id)
{
    public int Id { get; } = id;
}
var source = new MyMapperId(22);
var result = _mapper.Convert<MyMapperId, int>(source);
```

### 5. 成员复制器转化
>* 通过两个类型的复制器来转化成员

```csharp
var user = new User { Id = 1, Name = "Jxj" };
// 转化
var dto = Mapper.Global.Convert<User, UserDTO>(user);
// dto.Id = 1, dto.Name = "Jxj"
```

### 6. 构造函数转化
>* 通过构造函数参数传入来转化成员
>* 可以部分字段通过构造函数参数,另一部分通过成员复制器转化

```csharp
class MyMapper(MyMapperId id)
{
    public MyMapperId Id { get; } = id;
    public string Name { get; set; }
}
var source = new MyMapperDTO { Id = 222, Name = nameof(MyMapperDTO) };
var result = _mapper.Convert<MyMapperDTO, MyMapper>(source);
```