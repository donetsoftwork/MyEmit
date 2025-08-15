# Emit映射工具
>转化和复制POCO对象

## 一、转化
### 1. 直接转化
>调用Convert方法

```csharp
var user = new User { Id = 1, Name = "Jxj" };
// 转化
var dto = Mapper.Default.Convert<User, UserDTO>(user);
// dto.Id = 1, dto.Name = "Jxj"
```

### 2. 用委托
>调用GetConvertFunc方法

```csharp
var user = new User { Id = 1, Name = "Jxj" };
var dto = new UserDTO();
Func<User, UserDTO> convertFunc = Mapper.Default.GetConvertFunc<User, UserDTO>();
// 转化
var dto = convertFunc(user);
// dto.Id = 1, dto.Name = "Jxj"
```

### 3. 用接口
>调用GetConverter方法

```csharp
var user = new User { Id = 1, Name = "Jxj" };
IPocoConverter<User, UserDTO> converter = Mapper.Default.GetConverter<User, UserDTO>();
// 转化
var dto = converter.Convert(user);
// dto.Id = 1, dto.Name = "Jxj"
```

## 二、复制
### 1. 直接复制
>调用Copy方法

```csharp
var user = new User { Id = 1, Name = "Jxj" };
var dto = new UserDTO();
// 复制属性
Mapper.Default.Copy(user, dto);
// dto.Id = 1, dto.Name = "Jxj"
```

### 2. 用委托
>调用GetCopyAction方法

```csharp
var user = new User { Id = 1, Name = "Jxj" };
var dto = new UserDTO();
Action<User, UserDTO> copyAction = Mapper.Default.GetCopyAction<User, UserDTO>();
// 复制属性
copyAction(user, dto);
// dto.Id = 1, dto.Name = "Jxj"
```

### 3. 用接口
>调用GetCopier方法

```csharp
var user = new User { Id = 1, Name = "Jxj" };
var dto = new UserDTO();
IPocoCopier<User, UserDTO> copier = Mapper.Default.GetCopier<User, UserDTO>();
// 复制属性
copier.Copy(user, dto);
// dto.Id = 1, dto.Name = "Jxj"
```
