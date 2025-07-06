# 操作字段
>通过Emit读写POCO对象字段


## 1. 按字段读取
```csharp
// 获取字段
var field = typeof(Test).GetField("Id");
var instance = new Test { Id = 40 };
```

## 1.1 按实际类型读取
```csharp
Func<Test, int> getter = InstanceFieldHelper.EmitGetter<Test, int>(field);
int id = getter(instance);
```

## 1.2 按object类型读取
```csharp
// 按实际类型读取
Func<object, object> getter = InstanceFieldHelper.EmitGetter<object, object>(field);
object id = getter(instance);
```

## 2. 按字段名读取
## 2.1 按实际类型读取
```csharp
Func<Test, int> getter = InstanceFieldHelper.EmitGetter<Test, int>("Id");
int id = getter(instance);
```
## 2.2 按object类型读取
```csharp
// 按实际类型读取
Func<Test, object> getter = InstanceFieldHelper.EmitGetter<Test, object>("Id");
object id = getter(instance);
```

## 3. 按字段写入
## 3.1 按实际类型写入
```csharp
Action<Test, int> setter = InstanceFieldHelper.EmitSetter<Test, int>(field);
int id = 1;
setter(instance, id);
```

## 3.2 按object类型写入
```csharp
// 按实际类型写入
Action<object, object> setter = InstanceFieldHelper.EmitSetter<object, object>(field);
object id = 2;
setter(instance, id);
```

## 4. 按字段名写入
## 4.1 按实际类型写入
```csharp
Action<Test, int> setter = InstanceFieldHelper.EmitSetter<Test, int>("Id");
int id = 1;
setter(instance, id);
```
## 4.2 按object类型写入
```csharp
// 按实际类型写入
Action<Test, object> setter = InstanceFieldHelper.EmitSetter<Test, object>("Id");
object id = 2;
setter(instance, id);
```