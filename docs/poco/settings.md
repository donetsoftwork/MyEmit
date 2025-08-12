# 配置
>* 各项配置应该在程序启动时尽早配置
>* 已经生成的缓存修改相关配置不会生效

## 一、配置类型转化器
>通过IPoco配置

### 1. SetConvertFunc
>* 配置类型转化委托
>* 配置后按此委托生成类型转化委托或类型转化接口对象

```csharp
Poco.Global.SetConvertFunc<string, bool>(System.Convert.ToBoolean);
```

### 2. AddStaticConverter
>* 反射获取类型转化的静态方法

```csharp
options.AddStaticConverter<UserConverter>();
```

### 3. AddConverter
>* 反射获取类型转化的实例方法

```csharp
TimeConverter timeConverter = new();
options.AddConverter(timeConverter);
```

## 二、配置反射读、写成员
### 1. 通过接口IReflectionMember配置
#### 1.1 接口IReflectionMember
```csharp
interface IReflectionMember
{
    MemberBundle GetMembers(Type instanceType);
}
public record MemberBundle(
    IDictionary<string, MemberInfo> ReadMembers, 
    IDictionary<string, IEmitMemberReader> EmitReaders, 
    IDictionary<string, MemberInfo> WriteMembers, 
    IDictionary<string, IEmitMemberWriter> EmitWriters);
```

#### 1.2 DefaultReflectionMember
>* DefaultReflectionMember是IReflectionMember的默认实现
>* comparer配置成员名匹配,默认忽略大小写
>* includeField配置是否反射字段,默认反射字段

```csharp
public class DefaultReflectionMember(IEqualityComparer<string> comparer, bool includeField = true)
    : IReflectionMember
{
    public DefaultReflectionMember() : this(StringComparer.OrdinalIgnoreCase, true) {}
    // ...
}
```

#### 1.3 支持方法反射
>* 通过实现IReflectionMember可以支持方法反射作为读、写成员

### 2. 通过Poco构造函数配置
```csharp
class Poco(IReflectionMember reflection);
```

### 3. 通过静态属性DefaultReflectMember配置
```csharp
class Poco
{
    public static IReflectionMember DefaultReflectMember { get; set; }
}
```

#### 3.1 Poco.Global的ReflectionMember
>* 修改静态属性Poco.DefaultReflectMember会同步修改Global的ReflectionMember

#### 3.2 Poco默认的ReflectionMember
>* Poco无参构造函数调用的是Poco.DefaultReflectMember

