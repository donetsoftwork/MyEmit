# IOC注册Emit映射工具

## 一、容器注册转化器
### 1. 默认注册
>通过容器中默认的IMapper对象或Mapper.Global构造转化器

~~~csharp
services.UseConverter();
~~~

### 2. 指定IPocoOptions对象注册
~~~csharp
services.UseConverter(PocoEmit.Mapper.Global);
~~~

>注: PocoEmit.Mapper.Global继承IPocoOptions接口

### 3. 隔离注册
>指定IPocoOptions和serviceKey注册

~~~csharp
IPocoOptions poco = specialMapper;
services.UseConverter(poco, "special");
~~~

### 4. 通过IPocoConverter注入
>通过构造函数参数、属性等方式注入
~~~csharp
public sealed class Mapper(IPocoConverter<User, UserListDTO> converter)
    : Mapper<Request, Response, IEnumerable<User>>
{
    // ...
}
~~~

## 二、容器注册复制器
### 1. 默认注册
>通过容器中默认的IMapper对象或Mapper.Global构造复制器

~~~csharp
services.UseCopier();
~~~

### 2. 指定IMapper对象注册
~~~csharp
services.UseCopier(PocoEmit.Mapper.Global);
~~~

>注: PocoEmit.Mapper.Global继承IMapper接口

### 3. 隔离注册
>指定IPocoOptions和serviceKey注册

~~~csharp
IPocoOptions poco = specialMapper;
services.UseCopier(poco, "special");
~~~

### 4. 通过IPocoCopier注入
>通过构造函数参数、属性等方式注入
~~~csharp
public sealed class Mapper(IPocoCopier<User, UserListDTO> copier)
    : Mapper<Request, Response, IEnumerable<User>>
{
    // ...
}
~~~