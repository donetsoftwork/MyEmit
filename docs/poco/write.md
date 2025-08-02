# 写入成员

## 1. 用委托
>调用GetWriteAction方法

### 1.1 按实际类型写入
```csharp
Action<Test, int> writeAction = Poco.Global.GetWriteAction<User, int>("Id");
int id = 1;
writeAction(user, id);
```
### 2.2 按object类型写入
```csharp
// 按实际类型写入
Action<Test, object> writeAction = Poco.Global.GetWriteAction<User, object>("Id");
string name = "Jxj";
writeAction(user, name);
```

## 2. 用接口
>* 调用GetMemberWriter方法
>* 参考[IMemberWriter\<TInstance, TValue\>](xref:PocoEmit.IMemberWriter%602)

```csharp
IMemberWriter<User, string> writer = Poco.Global.GetMemberWriter<User, string>("Name");
writer.Write(user, name);
```