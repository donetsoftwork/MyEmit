# PocoEmit
>* 简单对象Emit工具
>* 用来生成读写POCO对象属性和字段的委托
>* 用来生成基础类型转化
>* 生成的委托性能比反射快
>* 支持跨平台
>* 支持net4.5;net5.0;net6.0;net7.0;net8.0;net9.0
>* 支持netstandard1.1;netstandard1.3;netstandard1.6;netstandard2.0;netstandard2.1


## 一、成员读取
>按属性或字段名读取

### 1. 用委托
>调用GetReadFunc方法

#### 1.1 按实际类型读取
```csharp
Func<Test, int> readFunc = Poco.Default.GetReadFunc<User, int>("Id");
int id = readFunc(user);
```
#### 1.2 按object类型读取
```csharp
// 按实际类型读取
Func<Test, object> readFunc = Poco.Default.GetReadFunc<User, object>("Id");
object id = reader(user);
```

### 2. 用接口
>调用GetMemberReader方法

```csharp
IMemberReader<User, string> reader = Poco.Default.GetMemberReader<User, string>("Name");
string name = reader.Read(user);
```

## 二、 成员写入
> 按属性或字段名写入

### 1. 用委托
>调用GetWriteAction方法

#### 1.1 按实际类型写入
```csharp
Action<Test, int> writeAction = Poco.Default.GetWriteAction<User, int>("Id");
int id = 1;
writeAction(user, id);
```
#### 1.2 按object类型写入
```csharp
// 按实际类型写入
Action<Test, object> writeAction = Poco.Default.GetWriteAction<User, object>("Id");
string name = "Jxj";
writeAction(user, name);
```

### 2. 用接口
>调用GetMemberWriter方法

```csharp
IMemberWriter<User, string> writer = Poco.Default.GetMemberWriter<User, string>("Name");
writer.Write(user, name);
```