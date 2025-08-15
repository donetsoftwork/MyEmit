# Emit集合扩展
>* 支持跨平台
>* 支持net4.5;net5.0;net6.0;net7.0;net8.0;net9.0
>* 支持netstandard1.1;netstandard1.3;netstandard1.6;netstandard2.0;netstandard2.1
>* Nuget包名: PocoEmit.Collections
>* 在PocoEmit.Mapper基础上增加集合类型的支持


## Mapper启用集合扩展
### 1. 对单个Mapper启用集合扩展
```csharp
Mapper.Default.UseCollection();
```

### 2. 全局启用集合扩展
```csharp
CollectionContainer.GlobalUseCollection();
```
