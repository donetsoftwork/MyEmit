# Emit集合扩展
>复制和转化集合对象

## 一、Mapper启用集合扩展
### 1. 对单个Mapper启用集合扩展
```csharp
Mapper.Default.UseCollection();
```

### 2. 全局启用集合扩展
```csharp
CollectionContainer.GlobalUseCollection();
```

## 二、转化
### 1. 直接转化
>调用Convert方法

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

### 2. 用委托
>调用GetConvertFunc方法

```csharp
IList<User> source = [new User { Id = 1, Name = "Jxj" }, new User { Id = 2, Name = "张三" }];
var converter = _mapper.GetConvertFunc<IList<User>, UserDTO[]>();
UserDTO[] result = converter(source);
```

### 3. 用接口
>调用GetConverter方法

```csharp
IList<AutoUserDTO> source = [new AutoUserDTO { UserId = "222", UserName = "Jxj" }, new AutoUserDTO { UserId = "333", UserName = "李四" }];
var converter = mapper.GetConverter<IList<AutoUserDTO>, User[]>();
var result = converter.Convert(source);
```

## 三、复制

### 1. 直接复制
>调用Copy方法

```csharp
User[] source = [new User { Id = 1, Name = "Jxj" }, new User { Id = 2, Name = "张三" }];
List<UserDTO> result = [];
_mapper.Copy(source, result);
```

### 2. 用委托
>调用GetCopyAction方法

```csharp
User[] source = [new User { Id = 1, Name = "Jxj" }, new User { Id = 2, Name = "张三" }];
List<UserDTO> result = [];
var copyAction = _mapper.GetCopyAction<User[], List<UserDTO>>();
copyAction(source, result);
```

### 3. 用接口
>调用GetCopier方法

```csharp
AutoUserDTO[] source = [new AutoUserDTO { UserId = "222", UserName = "Jxj" }, new AutoUserDTO { UserId = "333", UserName = "李四" }];
IList<User> result = [];
var copier = mapper.GetCopier<AutoUserDTO[], IList<User>>();
copier.Copy(source, result);
```

## 四、字典复制强化
### 1. DictionaryCopy方法
>复制实体成员到object字典

```csharp
User user = new() { Id = 3, Name = "张三" };
var dic = new Dictionary<string, object>();
_mapper.DictionaryCopy(user, dic);
```

### 2. GetDictionaryCopyAction方法
>复制实体成员到object字典

```csharp
var action = _mapper.GetDictionaryCopyAction<User>();
User user = new() { Id = 3, Name = "张三" };
var dic = new Dictionary<string, object>();
action(user, dic);
```

## 五、字典转化强化
### 1. ToDictionary
>实体成员转化为object字典

```csharp
User user = new() { Id = 3, Name = "张三" };
IDictionary<string, object> dic = _mapper.ToDictionary(user);
```

### 2. GetToDictionaryFunc
>实体成员转化为object字典

```csharp
var func = _mapper.GetToDictionaryFunc<User>();
User user = new() { Id = 3, Name = "张三" };
IDictionary<string, object> dic = func(user);
```

### 3. FromDictionary
>object字典转化为实体

```csharp
Dictionary<string, object> dic = new() { { nameof(User.Id), "3" }, { nameof(User.Name), "张三" } };
User user = _mapper.FromDictionary<User>(dic);
```

### 4. GetFromDictionaryFunc
>object字典转化为实体

```csharp
var func = _mapper.GetFromDictionaryFunc<User>();
Dictionary<string, object> dic = new() { { nameof(User.Id), "3" }, { nameof(User.Name), "张三" } };
User user = func(dic);
```

