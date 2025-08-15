# 复制
>* 调用[IMapper](xref:PocoEmit.IMapper)的方法来转化
>* IMapper继承[IPoco](xref:PocoEmit.IPoco)
>* 一般使用Mapper.Global或[Mapper](xref:PocoEmit.Mapper)的实例

## 一、复制方式
>* 调用Copy方法直接转化
>* 调用GetCopyAction方法委托转化
>* 调用GetCopier方法接口转化

## 二、支持范围

### 1. 支持成员复制
>* 如果源类型含目标类型的成员,按该成员转化

#### 1.1 Copy
>调用Copy方法

```csharp
var user = new User { Id = 1, Name = "Jxj" };
var dto = new UserDTO();
// 复制属性
Mapper.Global.Copy(user, dto);
// dto.Id = 1, dto.Name = "Jxj"
```

#### 1.2 GetCopyAction
>调用GetCopyAction方法

```csharp
var user = new User { Id = 1, Name = "Jxj" };
var dto = new UserDTO();
Action<User, UserDTO> copyAction = Mapper.Global.GetCopyAction<User, UserDTO>();
// 复制属性
copyAction(user, dto);
// dto.Id = 1, dto.Name = "Jxj"
```

#### 1.3. GetCopier
>* 参考[IPocoCopier\<TSource, TDest\>](xref:PocoEmit.IPocoCopier%602)

```csharp
var user = new User { Id = 1, Name = "Jxj" };
var dto = new UserDTO();
IPocoCopier<User, UserDTO> copier = Mapper.Global.GetCopier<User, UserDTO>();
// 复制属性
copier.Copy(user, dto);
// dto.Id = 1, dto.Name = "Jxj"
```