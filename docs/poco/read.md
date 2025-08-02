# 读取成员

## 1. 用委托读取
>调用GetReadFunc方法

### 1.1 按实际类型读取
```csharp
Func<Test, int> readFunc = Poco.Global.GetReadFunc<User, int>("Id");
int id = readFunc(user);
```
### 1.2 按object类型读取
```csharp
// 按实际类型读取
Func<Test, object> readFunc = Poco.Global.GetReadFunc<User, object>("Id");
object id = reader(user);
```

## 2. 用接口读取
>* 调用GetMemberReader方法
>* 参考[IMemberReader\<TInstance, TValue\>](xref:PocoEmit.IMemberReader%602)

```csharp
IMemberReader<User, string> reader = Poco.Global.GetMemberReader<User, string>("Name");
string name = reader.Read(user);
```