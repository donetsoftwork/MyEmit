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
>* 继承自PocoEmit.Mapper,与[Mapper复制范围](../mapper/copy.md)一致

### 2.支持集合元素复制
>复制前会尝试清空原元素

### 2.1 Copy
```csharp
User[] source = [new User { Id = 1, Name = "Jxj" }, new User { Id = 2, Name = "张三" }];
List<UserDTO> result = [];
_mapper.Copy(source, result);
```

#### 2.2 GetCopyAction
```csharp
User[] source = [new User { Id = 1, Name = "Jxj" }, new User { Id = 2, Name = "张三" }];
List<UserDTO> result = [];
var copyAction = _mapper.GetCopyAction<User[], List<UserDTO>>();
copyAction(source, result);
```

#### 2.3 GetCopier
>* 参考[IPocoCopier\<TSource, TDest\>](xref:PocoEmit.IPocoCopier%602)

```csharp
AutoUserDTO[] source = [new AutoUserDTO { UserId = "222", UserName = "Jxj" }, new AutoUserDTO { UserId = "333", UserName = "李四" }];
IList<User> result = [];
var copier = mapper.GetCopier<AutoUserDTO[], IList<User>>();
copier.Copy(source, result);
```