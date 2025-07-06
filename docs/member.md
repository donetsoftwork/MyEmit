# 操作属性和字段
>通过Emit读写POCO对象属性和字段

## 1. 读取
>按属性或字段名读取

## 1.1 按实际类型读取
```csharp
Func<Test, int> getter = InstanceHelper.EmitGetter<Test, int>("Id");
int id = getter(instance);
```
## 1.2 按object类型读取
```csharp
// 按实际类型读取
Func<Test, object> getter = InstanceHelper.EmitGetter<Test, object>("Id");
object id = getter(instance);
```

## 2. 写入
> 按属性或字段名写入

## 2.1 按实际类型写入
```csharp
Action<Test, int> setter = InstanceHelper.EmitSetter<Test, int>("Id");
int id = 1;
setter(instance, id);
```
## 2.2 按object类型写入
```csharp
// 按实际类型写入
Action<Test, object> setter = InstanceHelper.EmitSetter<Test, object>("Id");
object id = 2;
setter(instance, id);
```
