# Mapper配置
>* 各项配置应该在程序启动时尽早配置
>* 已经生成的缓存修改相关配置不会生效

## 一、配置类型转化器
>* 通过[IMapper](xref:PocoEmit.Configuration.IMapper)配置(IMapper继承[IPoco](xref:PocoEmit.Configuration.IPoco))
>* SetConvertFunc
>* AddStaticConverter
>* AddConverter
>* 以上继承自PocoEmit,与[PocoEmit配置类型转化器](../poco/settings.md)一致


## 二、配置反射读、写成员
>* 配置反射读、写成员,与[PocoEmit配置反射读、写成员](../poco/settings.md)基本一致
>* 本配置操作的是[Mapper](xref:PocoEmit.Mapper)的实例或Mapper.Global

## 三、配置反射构造函数
### 1. 通过接口IReflectionConstructor配置
#### 1.1 接口IReflectionConstructor
>[IReflectionConstructor](xref:PocoEmit.Reflection.IReflectionConstructor)

```csharp
interface IReflectionConstructor
{
    ConstructorInfo GetConstructor(Type instanceType);
}
```

#### 1.2 DefaultReflectionConstructor
>* [DefaultReflectionConstructor](xref:PocoEmit.Reflection.DefaultReflectionConstructor)是[IReflectionConstructor](xref:PocoEmit.Reflection.IReflectionConstructor)的默认实现
>* parameterCountDesc配置获取参数最多的还是最少的,默认最少的

```csharp
DefaultReflectionConstructor(bool parameterCountDesc)
    : IReflectionConstructor
{
    // ...
}
```

### 2. 通过Mapper构造函数reflectionConstructor配置
>参考[Mapper](xref:PocoEmit.Mapper)

```csharp
class Mapper(IReflectionMember reflectionMember, IReflectionConstructor reflectionConstructor);
```

### 3. 通过静态属性DefaultReflectConstructor配置
>参考[Mapper](xref:PocoEmit.Mapper)

```csharp
class Mapper
{
    public static IReflectionMember DefaultReflectConstructor { get; set; }
}
```

#### 3.1 Mapper.Global的ReflectConstructor
>* 修改静态属性Mapper.DefaultReflectConstructor会同步修改Global的ReflectConstructor

#### 3.2 Mapper默认的ReflectConstructor
>* Mapper无参构造函数调用的是Mapper.DefaultReflectConstructor

## 四、配置成员匹配规则
### 1. 通过接口IMemberMatch配置

#### 1.1 接口IMemberMatch
>* 参考[IMemberMatch](xref:PocoEmit.Maping.IMemberMatch)
>* [IMember](xref:PocoEmit.Members.IMember)表示读、写成员或构造函数参数

```csharp
interface IMemberMatch
{
    bool Match(IMember source, IMember dest);
}
```

#### 1.2 MemberNameMatcher
>* 参考[MemberNameMatcher](xref:PocoEmit.Maping.MemberNameMatcher)
>* MemberNameMatcher是IReflectionConstructor的默认实现
>* 默认INameMatch是忽略大小写的成员名匹配

```csharp
class MemberNameMatcher(INameMatch nameMatch)
    : IMemberMatch
{
    // ...
}
```

### 2. 通过Set方法配置匹配规则
```csharp
MapTypeKey key = new(typeof(TSource), typeof(TDest));
mapper.Set(key, matcher);
```

### 3. 配置默认匹配规则
>* 通过[IMapper](xref:PocoEmit.Configuration.IMapper)的属性DefaultMatch
>* 通过[Mapper](xref:PocoEmit.Mapper)构造函数参数defaultMatch
>* Mapper默认Global的属性DefaultMatch
>* 未按源类型和目标类型配置匹配规则时,按默认匹配规则

