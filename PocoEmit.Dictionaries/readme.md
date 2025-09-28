# Emit字典扩展

## 一、实体复制到字典
### 1. CreatetDictionaryCopyAction方法
>* 委托编译生成,重复使用需要缓存
>* 递归复制指定类型成员
>* 支持同类型字典
>* 不支持数组和其他集合
>* 相当于彻底平铺展开

```csharp
var action = _mapper.CreatetDictionaryCopyAction<Student, Dictionary<string, string>>();
Student source = new()
{
    User = new() { Id = 2, Name = "Jxj" },
    Role = "班长",
    Skills = new() { { "足球", "很好" }, { "篮球", "优秀" } }
};
var result = new Dictionary<string, string>();
action(source, result);
// result.Count == 4
```

```csharp
var action = _mapper.CreatetDictionaryCopyAction<Student, Dictionary<string, int>>();
Student source = new()
{
    User = new() { Id = 2, Name = "Jxj" },
    Age = 17,
    Scores = new() { { "语文", 95 }, { "数学", 96 } }
};
var dic = new Dictionary<string, int>();
action(source, dic);
// result.Count == 4
```

### 2. CreatetDictionaryCopyAction重载方法
>* 委托编译生成,重复使用需要缓存
>* 递归复制指定类型成员并转化为另一类型
>* 支持同类型数组
>* 不支持数组和其他集合
>* 相当于彻底平铺展开

```csharp
var action = _mapper.CreatetDictionaryCopyAction<Student, int, IDictionary<string, string>>();
Assert.NotNull(action);
Student source = new()
{
    User = new() { Id = 2, Name = "Jxj" },
    Age = 17,
    Scores = new() { { "语文", 95 }, { "数学", 96 } }
};
var dic = new Dictionary<string, string>();
action(source, dic);
// result.Count == 4
```

## 五、实体转化为字典

### 1. CreateDictionaryConvertFunc
>* 委托编译生成,重复使用需要缓存
>* 递归转化指定类型成员为字典
>* 支持同类型字典
>* 不支持数组和其他集合
>* 相当于彻底平铺展开

```csharp
var func = _mapper.CreateDictionaryConvertFunc<Student, Dictionary<string, int>>();
Student source = new()
{
    User = new() { Id = 2, Name = "Jxj" },
    Age = 17,
    Scores = new() { { "语文", 95 }, { "数学", 96 } }
};
Dictionary<string, int> result = func(source);
// result.Count == 4
```

### 2. CreateDictionaryConvertFunc重载方法
>* 委托编译生成,重复使用需要缓存
>* 递归转化指定类型成员为字典
>* 支持同类型字典
>* 不支持数组和其他集合
>* 相当于彻底平铺展开

```csharp
var func = _mapper.CreateDictionaryConvertFunc<Student, int, IDictionary<string, string>>();
Student source = new()
{
    User = new() { Id = 2, Name = "Jxj" },
    Age = 17,
    Scores = new() { { "语文", 95 }, { "数学", 96 } }
};
IDictionary<string, string> result = func(source);
// result.Count == 4
```
