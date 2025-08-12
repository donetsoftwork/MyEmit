# 类型转化
>* 调用[IPoco](xref:PocoEmit.Configuration.IPoco)的方法来转化
>* 一般使用[Poco](xref:PocoEmit.Poco)的实例或Poco.Global

## 一、转化方式
### 1. 直接转化
>* 调用Convert方法

```csharp
long result = _poco.Convert<int, long>(123);
```

### 2. 委托转化
>* 调用GetConvertFunc方法

```csharp
Func<int, long> convertFun = _poco.GetConvertFunc<int, long>();
long result = convertFun(123);
```

### 3. 接口转化
>* 调用GetConverter方法
>* 参考[IPocoConverter\<TSource, TDest\>](xref:PocoEmit.IPocoConverter%602)

```csharp
IPocoConverter<int, long> converter = _poco.GetConverter<int, long>();
long result = converter.Convert(123);
```

## 二、支持范围

### 1. 基础类型转化
>* 调用Emit转化基础类型
>* 通过配置可以增强或调整转化行为

```csharp
Func<int, long> convertFun = _poco.GetConvertFunc<int, long>();
long result = convertFun(123);
```

### 2. 支持单参数转化
```csharp
class PocoId(int id) {
    public int Id { get; } = id;
}
Func<int, PocoId> convertFun = _poco.GetConvertFunc<int, PocoId>();
PocoId result = convertFun(1);
```

### 3. 支持可空类型
>* 如果该非空类型转化支持,可空类型就也支持

```csharp
Func<int, long?> convertFun = _poco.GetConvertFunc<int, long?>();
long? result = convertFun(123);
```
```csharp
Func<int?, long> convertFun = _poco.GetConvertFunc<int?, long>();
long result = convertFun(123);
```