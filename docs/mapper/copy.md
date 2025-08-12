# 复制
>* 调用[IMapper](xref:PocoEmit.Configuration.IMapper)的方法来转化
>* IMapper继承[IPoco](xref:PocoEmit.Configuration.IPoco)
>* 一般使用Mapper.Global或[Mapper](xref:PocoEmit.Mapper)的实例

## 1. 直接复制
>调用Copy方法

```csharp
var user = new User { Id = 1, Name = "Jxj" };
var dto = new UserDTO();
// 复制属性
Mapper.Global.Copy(user, dto);
// dto.Id = 1, dto.Name = "Jxj"
```

## 2. 用委托
>调用GetCopyAction方法

```csharp
var user = new User { Id = 1, Name = "Jxj" };
var dto = new UserDTO();
Action<User, UserDTO> copyAction = Mapper.Global.GetCopyAction<User, UserDTO>();
// 复制属性
copyAction(user, dto);
// dto.Id = 1, dto.Name = "Jxj"
```

## 3. 用接口
>* 调用GetCopier方法
>* 参考[IPocoCopier\<TSource, TDest\>](xref:PocoEmit.IPocoCopier%602)

```csharp
var user = new User { Id = 1, Name = "Jxj" };
var dto = new UserDTO();
IPocoCopier<User, UserDTO> copier = Mapper.Global.GetCopier<User, UserDTO>();
// 复制属性
copier.Copy(user, dto);
// dto.Id = 1, dto.Name = "Jxj"
```