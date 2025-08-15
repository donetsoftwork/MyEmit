# 数量
>* 获取集合元素数量
>* 适合在泛型环境下不明确具体集合类型时使用
>* 支持数组、集合、字典等
>* Emit编译,提供接近原生的性能

## 1. 调用Count方法获取
```csharp
IList<int> collection = [1, 2, 3];
int count = Count(collection);

int Count<TCollection>(TCollection collection)
{
    int count = CollectionContainer.Instance.Count(collection);
    return count;
}
```

## 2. 调用GetCountFunc方法委托获取
```csharp
List<int> collection = [1, 2, 3];
int count = CountFunc(collection);

int CountFunc<TCollection>(TCollection collection)
{
    var countFunc = CollectionContainer.Instance.GetCountFunc<TCollection>();
    int count = countFunc(collection);
}
```

## 3. 调用GetCounter方法接口获取
```csharp
int[] collection = [1, 2, 3];
int count = Counter(collection);

int Counter<TCollection>(TCollection collection)
{
    var counter = CollectionContainer.Instance.GetCounter<TCollection>();
    int count = counter.Count(collection);
}
```