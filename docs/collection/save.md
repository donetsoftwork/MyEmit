# 保存
>* 添加集合元素
>* 适合在泛型环境下不明确具体集合类型时使用
>* 支持大部分集合类型
>* 支持类List、HashSet、Queue、Stack、BlockingCollection、ConcurrentQueue、ConcurrentStack、ConcurrentBag等
>* 支持接口IList、ISet、ICollection、IProducerConsumerCollection等
>* Emit编译,提供接近原生的性能

## 1. 调用Save方法保存
```csharp
IList<string> collection = ["a","b", "c"];
Save(collection, "d");

void Save<TCollection>(TCollection collection, string item)
{
    CollectionContainer.Instance.Save(collection, item);
}
```

## 2. 调用GetSaveAction方法保存
```csharp
List<string> collection = ["a","b", "c"];
SaveAction(collection, "d");

void SaveAction<TCollection>(TCollection collection, string item)
{
    var saveAction = _container.GetSaveAction<TCollection, string>();
    saveAction(collection, item);
}
```

## 3. 调用GetSaver方法保存
```csharp
List<string> collection = ["a","b", "c"];
Saver(collection, "d");

void Saver<TCollection>(TCollection collection, string item)
{
    var saver = _container.GetSaver<TCollection, string>();
    saver.Add(collection, item);
}
```