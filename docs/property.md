# 操作属性
>通过Emit读写POCO对象属性


## 1. 按属性读取
```csharp
// 获取属性
var property = typeof(Test).GetProperty("Id");
var instance = new Test { Id = 40 };
```

## 1.1 按实际类型读取
```csharp
Func<Test, int> getter = InstancePropertyHelper.EmitGetter<Test, int>(property);
int id = getter(instance);
```

## 1.2 按object类型读取
```csharp
// 按实际类型读取
Func<object, object> getter = InstancePropertyHelper.EmitGetter<object, object>(property);
object id = getter(instance);
```

## 2. 按属性名读取
## 2.1 按实际类型读取
```csharp
Func<Test, int> getter = InstancePropertyHelper.EmitGetter<Test, int>("Id");
int id = getter(instance);
```
## 2.2 按object类型读取
```csharp
// 按实际类型读取
Func<Test, object> getter = InstancePropertyHelper.EmitGetter<Test, object>("Id");
object id = getter(instance);
```

## 3. 按属性写入
## 3.1 按实际类型写入
```csharp
Action<Test, int> setter = InstancePropertyHelper.EmitSetter<Test, int>(property);
int id = 1;
setter(instance, id);
```

## 3.2 按object类型写入
```csharp
// 按实际类型写入
Action<object, object> setter = InstancePropertyHelper.EmitSetter<object, object>(property);
object id = 2;
setter(instance, id);
```

## 4. 按属性名写入
## 4.1 按实际类型写入
```csharp
Action<Test, int> setter = InstancePropertyHelper.EmitSetter<Test, int>("Id");
int id = 1;
setter(instance, id);
```
## 4.2 按object类型写入
```csharp
// 按实际类型写入
Action<Test, object> setter = InstancePropertyHelper.EmitSetter<Test, object>("Id");
object id = 2;
setter(instance, id);
```
