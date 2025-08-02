# 快速上手
>* 通过静态类[InstanceHelper](xref:PocoEmit.InstanceHelper)的静态方法快捷读写成员

## 一、成员读取委托
>按属性或字段名读取

### 1 按实际类型读取
```csharp
Func<Test, int> readFunc = InstanceHelper.GetReadFunc<User, int>("Id");
int id = readFunc(user);
```
### 2 按object类型读取
```csharp
// 按实际类型读取
Func<Test, object> readFunc = InstanceHelper.GetReadFunc<User, object>("Id");
object id = reader(user);
```

## 二、 成员写入托
> 按属性或字段名写入

### 1 按实际类型写入
```csharp
Action<Test, int> writeAction = InstanceHelper.GetWriteAction<User, int>("Id");
int id = 1;
writeAction(user, id);
```
### 2 按object类型写入
```csharp
// 按实际类型写入
Action<Test, object> writeAction = InstanceHelper.GetWriteAction<User, object>("Id");
string name = "Jxj";
writeAction(user, name);
```

## 三、InstanceHelper说明
>* InstanceHelper实际是通过Poco.Global来操作的
>* InstanceHelper的方法实际调用Poco.Global的同名方法