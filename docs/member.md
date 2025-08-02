---
_layout: landing
---

# 操作属性和字段
>通过Emit读写POCO对象属性和字段

## 1. 读取
>按属性或字段名读取

### 1.1 按实际类型读取
```csharp
Func<Test, int> readFunc = InstanceHelper.GetReadFunc<Test, int>("Id");
int id = readFunc(instance);
```
### 1.2 按object类型读取
```csharp
// 按实际类型读取
Func<Test, object> readFunc = InstanceHelper.GetReadFunc<Test, object>("Id");
object id = readFunc(instance);
```

## 2. 写入
> 按属性或字段名写入

### 2.1 按实际类型写入
```csharp
Action<Test, int> writeAction = InstanceHelper.GetWriteAction<Test, int>("Id");
int id = 1;
writeAction(instance, id);
```
### 2.2 按object类型写入
```csharp
// 按实际类型写入
Action<Test, object> writeAction = InstanceHelper.GetWriteAction<Test, object>("Id");
object id = 2;
writeAction(instance, id);
```
