# Emit工具

## 一、成员读取
>按属性或字段名读取

### 1. 用委托
>调用GetReadFunc方法

#### 1.1 按实际类型读取
```csharp
Func<Test, int> readFunc = Poco.Global.GetReadFunc<User, int>("Id");
int id = readFunc(user);
```
#### 1.2 按object类型读取
```csharp
// 按实际类型读取
Func<Test, object> readFunc = Poco.Global.GetReadFunc<User, object>("Id");
object id = reader(user);
```

### 2. 用接口
>调用GetMemberReader方法

```csharp
IMemberReader<User, string> reader = Poco.Global.GetMemberReader<User, string>("Name");
string name = reader.Read(user);
```

## 二、 成员写入
> 按属性或字段名写入

## 1.1 按实际类型写入
```csharp
Action<Test, int> writeAction = Poco.Global.GetWriteAction<User, int>("Id");
int id = 1;
writeAction(user, id);
```
## 2.2 按object类型写入
```csharp
// 按实际类型写入
Action<Test, object> writeAction = Poco.Global.GetWriteAction<User, object>("Id");
string name = "Jxj";
writeAction(user, name);
```

### 2. 用接口
>调用GetMemberWriter方法

```csharp
IMemberWriter<User, string> writer = Poco.Global.GetMemberWriter<User, string>("Name");
writer.Write(user, name);
```

## 三、复制
### 1. 直接复制
>调用Copy方法

```csharp
var user = new User { Id = 1, Name = "Jxj" };
var dto = new UserDTO();
// 复制属性
Mapper.Global.Copy(user, dto);
// dto.Id = 1, dto.Name = "Jxj"
```

### 2. 用委托
>调用GetCopyAction方法

```csharp
var user = new User { Id = 1, Name = "Jxj" };
var dto = new UserDTO();
Action<User, UserDTO> copyAction = Mapper.Global.GetCopyAction<User, UserDTO>();
// 复制属性
copyAction(user, dto);
// dto.Id = 1, dto.Name = "Jxj"
```

### 3. 用接口
>调用GetCopier方法

```csharp
var user = new User { Id = 1, Name = "Jxj" };
var dto = new UserDTO();
IPocoCopier<User, UserDTO> copier = Mapper.Global.GetCopier<User, UserDTO>();
// 复制属性
copier.Copy(user, dto);
// dto.Id = 1, dto.Name = "Jxj"
```

## 四、转化
### 1. 直接转化
>调用Convert方法

```csharp
var user = new User { Id = 1, Name = "Jxj" };
// 转化
var dto = Mapper.Global.Convert<User, UserDTO>(user);
// dto.Id = 1, dto.Name = "Jxj"
```

### 2. 用委托
>调用GetConvertFunc方法

```csharp
var user = new User { Id = 1, Name = "Jxj" };
var dto = new UserDTO();
Func<User, UserDTO> convertFunc = Mapper.Global.GetConvertFunc<User, UserDTO>();
// 转化
var dto = convertFunc(user);
// dto.Id = 1, dto.Name = "Jxj"
```

### 3. 用接口
>调用GetConverter方法

```csharp
var user = new User { Id = 1, Name = "Jxj" };
IPocoConverter<User, UserDTO> converter = Mapper.Global.GetConverter<User, UserDTO>();
// 转化
var dto = converter.Convert(user);
// dto.Id = 1, dto.Name = "Jxj"
```

## 五、容器注册转化器
## 1. 默认注册
>通过容器中默认的IMapper对象或Mapper.Global构造转化器

~~~csharp
services.UseConverter();
~~~

## 2. 指定IPoco对象注册
~~~csharp
services.UseConverter(PocoEmit.Mapper.Global);
~~~

>注: PocoEmit.Mapper.Global继承IPoco接口

## 3. 隔离注册
>指定IPoco和serviceKey注册

~~~csharp
IPoco poco = specialMapper;
services.UseConverter(poco, "special");
~~~

## 4. 通过IPocoConverter注入
>通过构造函数参数、属性等方式注入
~~~csharp
public sealed class Mapper(IPocoConverter<User, UserListDTO> converter)
    : Mapper<Request, Response, IEnumerable<User>>
{
    // ...
}
~~~

## 六、容器注册复制器
## 1. 默认注册
>通过容器中默认的IMapper对象或Mapper.Global构造复制器

~~~csharp
services.UseCopier();
~~~

## 2. 指定IMapper对象注册
~~~csharp
services.UseCopier(PocoEmit.Mapper.Global);
~~~

>注: PocoEmit.Mapper.Global继承IMapper接口

## 3. 隔离注册
>指定IPoco和serviceKey注册

~~~csharp
IPoco poco = specialMapper;
services.UseCopier(poco, "special");
~~~

## 4. 通过IPocoCopier注入
>通过构造函数参数、属性等方式注入
~~~csharp
public sealed class Mapper(IPocoCopier<User, UserListDTO> copier)
    : Mapper<Request, Response, IEnumerable<User>>
{
    // ...
}
~~~