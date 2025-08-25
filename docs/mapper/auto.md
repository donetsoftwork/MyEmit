# 婶可忍叔不可忍的AutoMapper的你还在用吗?

## 1. 简单类型转化也需要提前注册

```csharp
Mapper mapper = new();
MapHelper<User, UserDTO> map = mapper.Configure<User, UserDTO>();
```