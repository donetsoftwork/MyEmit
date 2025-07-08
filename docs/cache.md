---
_layout: landing
---

# Emit缓存
>* 使用缓存来存储生成的委托
>* 避免多次使用重复生成,以提高性能

## 1. 读取
>使用EmitGetter方法读取

### 1.1 读取属性
```csharp
Func<object, object> getter = EmitCaches.EmitGetter(property);
object id = getter(instance);
```

### 1.2 读取字段
```csharp
Func<object, object> getter = EmitCaches.EmitGetter(field);
object id = getter(instance);
```

## 2. 写入
>使用EmitSetter方法写入

### 2.1 写入属性
```csharp
Action<object, object> setter = EmitCaches.EmitSetter(property);
setter(instance, 1);
```

### 2.2 写入字段
```csharp
Action<object, object> setter = EmitCaches.EmitSetter(field);
setter(instance, 2);
```
