# 辅助配置
>通过类[MapHelper\<TSource, TDest\>](xref:PocoEmit.Helpers.MapHelper%602)实现方便快捷的配置

## 一、构造MapHelper
>[MapHelper\<TSource, TDest\>](xref:PocoEmit.Helpers.MapHelper%602)

```csharp
Mapper mapper = new();
MapHelper<User, UserDTO> map = mapper.Configure<User, UserDTO>();
```

## 二、按来源配置
### 1. 获取来源配置
>[MapHelper\<TSource, TDest\>.SourceHelper](xref:PocoEmit.Helpers.MapHelper%602.SourceHelper)

```csharp
Mapper mapper = new();
MapHelper<User, UserDTO>.SourceHelper source = mapper.Configure<User, UserDTO>()
    .Source;
```

### 2. 获取来源成员
```csharp
Mapper mapper = new();
var source = mapper.Configure<User, UserDTO>()
    .Source
    .ForMember(nameof(User.Name));
```

### 3. 设置忽略来源某成员
#### 3.1 通过来源配置设置
```csharp
Mapper mapper = new();
var source = mapper.Configure<User, UserDTO>()
    .Source
    .Ignore(nameof(User.Name));
```

#### 3.2 通过来源成员设置
```csharp
Mapper mapper = new();
var source = mapper.Configure<User, UserDTO>()
    .Source
    .ForMember(nameof(User.Name)).Ignore();
```

### 4. 设置指定成员名映射
#### 4.1 通过来源配置设置
```csharp
Mapper mapper = new();
mapper.Configure<User, UserCustomDTO>()
    .Source
    .MapTo(nameof(User.Id), nameof(UserCustomDTO.UserId))
    .MapTo(nameof(User.Name), nameof(UserCustomDTO.UserName));
```

#### 4.2 通过来源成员设置
```csharp
Mapper mapper = new();
mapper.Configure<User, UserCustomDTO>()
    .Source
    .ForMember(nameof(User.Id)).MapTo(nameof(UserCustomDTO.UserId))
    .ForMember(nameof(User.Name)).MapTo(nameof(UserCustomDTO.UserName));
```

## 三、按目标配置
### 1. 获取目标配置
>[MapHelper\<TSource, TDest\>.DestHelper](xref:PocoEmit.Helpers.MapHelper%602.DestHelper)

```csharp
Mapper mapper = new();
MapHelper<User, UserDTO>.Dest dest = mapper.Configure<UserDTO, User>()
    .Dest;
```

### 2. 获取目标成员
```csharp
Mapper mapper = new();
var dest = mapper.Configure<UserDTO, User>()
    .Dest
    .ForMember(nameof(User.Name));
```

### 3. 设置忽略目标某成员
#### 3.1 通过目标配置设置
```csharp
Mapper mapper = new();
var dest = mapper.Configure<UserDTO, User>()
    .Dest
    .Ignore(nameof(User.Name));
```

#### 3.2 通过目标成员设置
```csharp
Mapper mapper = new();
var dest = mapper.Configure<UserDTO, User>()
    .Dest
    .ForMember(nameof(User.Name)).Ignore();
```

### 4. 设置指定成员名映射
#### 4.1 通过目标配置设置
```csharp
Mapper mapper = new();
mapper.Configure<UserCustomDTO, User>()
    .Dest
    .MapFrom(nameof(User.Id), nameof(UserCustomDTO.UserId))
    .MapFrom(nameof(User.Name), nameof(UserCustomDTO.UserName));
```

#### 4.2 通过目标成员设置
```csharp
Mapper mapper = new();
mapper.Configure<UserCustomDTO, User>()
    .Dest
    .ForMember(nameof(User.Id)).MapFrom(nameof(UserCustomDTO.UserId))
    .ForMember(nameof(User.Name)).MapFrom(nameof(UserCustomDTO.UserName));
```