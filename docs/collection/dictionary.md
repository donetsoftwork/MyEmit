## 一、PocoEmit集合扩展中支持字典
>* 支持字典互转
>* 支持数组转字典(索引转化为字典主键)
>* 支持列表转字典(索引转化为字典主键)

默认集合功能不支持实体属性转字典
为此开发了以下字典增强功能来实现实体转字典

## 二、实体和字典互转
### 1. ToDictionary
>实体成员转化为object字典

```csharp
User user = new() { Id = 3, Name = "张三" };
IDictionary<string, object> dic = _mapper.ToDictionary(user);
```

### 2. GetToDictionaryFunc
>实体成员转化为object字典

```csharp
var func = _mapper.GetToDictionaryFunc<User>();
User user = new() { Id = 3, Name = "张三" };
IDictionary<string, object> dic = func(user);
```

## 3. FromDictionary
>object字典转化为实体

```csharp
Dictionary<string, object> dic = new() { { nameof(User.Id), "3" }, { nameof(User.Name), "张三" } };
User user = _mapper.FromDictionary<User>(dic);
```

## 4. GetFromDictionaryFunc
>object字典转化为实体

```csharp
var func = _mapper.GetFromDictionaryFunc<User>();
Dictionary<string, object> dic = new() { { nameof(User.Id), "3" }, { nameof(User.Name), "张三" } };
User user = func(dic);
```

## 三、实体属性平铺
## 1. CreateDictionaryConvertFunc
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
    Score = new() { { "语文", 95 }, { "数学", 96 } }
};
Dictionary<string, int> result = func(source);
// result.Count == 4

```

```json
{
	"UserId": 2,
	"Age": 17,
	"Score语文": 95,
	"Score数学": 96
}
```

## 2. CreateDictionaryConvertFunc重载方法
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

```json
{
	"UserId": "2",
	"Age": "17",
	"Score语文": "95",
	"Score数学": "96"
}
```


源码托管地址: https://github.com/donetsoftwork/MyEmit ，也欢迎大家直接查看源码。
gitee同步更新:https://gitee.com/donetsoftwork/MyEmit

如果大家喜欢请动动您发财的小手手帮忙点一下Star。
