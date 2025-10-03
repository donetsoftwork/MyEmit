# PocoEmit遥遥领先于AutoMapper之依赖注入

## 什么是依赖注入
>* 这里说的依赖注入不是把PocoEmit注入到容器中,那是小儿科问题
>* 这里说的依赖注入是把外部的服务注入到PocoEmit
>* 把容器注入到PocoEmit中
>* 把容器中的服务注入到PocoEmit中
>* 此功能AutoMapper是不支持的

## 一、首先来个Case演示一下
>* Dto转化为实体
>* 但是实体有更多逻辑依赖外部服务,这些外部服务Dto不见得提供的了
>* 这就需要注入
>* PocoEmit支持构造函数参数注入和属性注入
>* IMapper对象是默认支持注入的服务

### 1. Entity比Dto多出来的Mapper可以注入
~~~csharp
class MessageDto
{
    public string Message { get; set; }
}
class MessageEntity
{
    public IMapper Mapper { get; set; }
    public string Message { get; set; }
}
~~~

### 2. 转化并注入的代码
~~~csharp
var mapper = Mapper.Create();
var dto = new MessageDto { Message = "Hello UseMapper" };
MessageEntity message = mapper.Convert<MessageDto, MessageEntity>(dto);
Assert.NotNull(message.Mapper);
~~~

## 二、再演示注入自定义的服务
### 1. UserDomain比Dto多出来的Repository可以注入
~~~csharp
public record UserDTO
{
    public int Id { get; set; }
    public string Name { get; set; }
}
class UserDomain(UserRepository repository, int id, string name)
{
    private readonly UserRepository _repository = repository;
    public UserRepository Repository
        => _repository;
    public int Id { get; } = id;
    public string Name { get; } = name;
    // ...
}
class UserRepository
{
    void Add(UserDomain user) { }
    void Update(UserDomain entity) { }
    void Remove(UserDomain entity) { }
    public static readonly UserRepository Instance = new();
}
~~~

### 2. 注册、转化并注入的代码
>* 通过UseDefault可以注入服务

~~~csharp
IMapper mapper = Mapper.Create()
    .UseDefault(UserRepository.Instance);
var dto = new UserDTO { Id = 1, Name = "Jxj" };
UserDomain user = mapper.Convert<UserDTO, UserDomain>(dto);
Assert.NotNull(user.Repository);
~~~

## 三、注入IOC容器的Case
>* 注入IOC容器需要安装nuget包PocoEmit.ServiceProvider

### 1. 包含IOC容器的实体
~~~csharp
class UserWithServiceProvider
{
    public int Id { get; set; }
    public string Name { get; set; }
    public IServiceProvider ServiceProvider { get; set; }
}
~~~

### 2. 注册、转化并注入的代码
>* UseSingleton是把容器作为唯一容器注入
>* UseScope是使用当前Scope的子容器
>* UseContext是在Mvc下,使用当前HttpContext的RequestServices子容器

~~~csharp
var services = new ServiceCollection();
var serviceProvider = services.BuildServiceProvider();
var mapper = Mapper.Create();
mapper.UseSingleton(serviceProvider);
var dto = new UserDTO { Id = 1, Name = "Jxj" };
UserWithServiceProvider user = mapper.Convert<UserDTO, UserWithServiceProvider>(dto);
Assert.NotNull(user.ServiceProvider);
~~~

## 四、当然还可以注入容器内的服务
### 1. UserDomain多出来的UserDomain需要注入
>* 这次我们用IOC来管理UserRepository
>* 这样才能更好的利用依赖注入
>* UserRepository可能还会依赖其他的
>* 手动维护对象可能会很麻烦,IOC容器擅长维护这些复杂关系

~~~csharp
    class UserDomain(UserRepository repository, int id, string name)
    {
        private readonly UserRepository _repository = repository;
        public UserRepository Repository
            => _repository;
        public int Id { get; } = id;
        public string Name { get; } = name;
        // ...
    }
    class UserRepository
    {
        void Add(UserDomain user) { }
        void Update(UserDomain entity) { }
        void Remove(UserDomain entity) { }
    }
~~~

### 2. 注册、转化并注入的代码
>* 通过UseScope注入IOC容器
>* 通过UseDefault告知这个类型从IOC容器中注入

~~~csharp
var services = new ServiceCollection()
    .AddScoped<UserRepository>();
var serviceProvider = services.BuildServiceProvider();
var mapper = Mapper.Create();
mapper.UseScope(serviceProvider)
     .UseDefault<UserRepository>();
var dto = new UserDTO { Id = 1, Name = "Jxj" };
UserDomain user = mapper.Convert<UserDTO, UserDomain>(dto);
Assert.NotNull(user.Repository);
~~~

## 五、支持IOC容器的特性
>* 支持FromKeyedServices
>* 支持FromServices

### 1. FromKeyedServices标记注入点和服务键
~~~csharp
class UserDomain1([FromKeyedServices("User1")]UserRepository repository, int id, string name)
    : UserDomain(repository, id, name)
{
}
class UserDomain2([FromKeyedServices("User2")] UserRepository repository, int id, string name)
    : UserDomain(repository, id, name)
{
}
class UserDomain(UserRepository repository, int id, string name)
{
    private readonly UserRepository _repository = repository;
    public UserRepository Repository
        => _repository;
    public int Id { get; } = id;
    public string Name { get; } = name;
    // ...
}
class UserRepository(string tableName)
{
    private readonly string _tableName = tableName;
    public string TableName 
        => _tableName;
    void Add(UserDomain user) { }
    void Update(UserDomain entity) { }
    void Remove(UserDomain entity) { }
}
~~~

### 2. 注册、转化并注入的代码
>* 由于识别出FromKeyedServices,就不需要UseDefault
>* 这样简洁由优雅

~~~csharp
string table1 = "User1";
string table2 = "User2";
var services = new ServiceCollection()
    .AddKeyedScoped(table1, (_, _) => new UserRepository(table1))
    .AddKeyedScoped(table2, (_, _) => new UserRepository(table2));
var serviceProvider = services.BuildServiceProvider();
var mapper = Mapper.Create();
mapper.UseScope(serviceProvider);
var dto = new UserDTO { Id = 1, Name = "Jxj" };
UserDomain user = mapper.Convert<UserDTO, UserDomain1>(dto);
Assert.NotNull(user.Repository);
UserDomain user2 = mapper.Convert<UserDTO, UserDomain2>(dto);
Assert.NotNull(user2.Repository);
~~~

## 六、竞品类似的功能
### 1. AutoMapper不支持
>* AutoMapper的NullSubstitute用来指定源属性为null时的默认值
>* PocoEmit在源无法匹配或源字段为null都可能触发依赖注入

### 2. EF有类似功能
>* 不过貌似只支持EF内部某些服务
>* 请参阅 [EF Core实体类的依赖注入](https://www.cnblogs.com/tcjiaan/p/19077173)

## 七、总结
### 1. OOM映射需要依赖注入
>* DTO、实体、领域模型如果有业务逻辑就需要依赖外部服务
>* 需要外部服务就需要依赖注入

### 2. IOC容器使用需要注意
>* 简单作业单容器,使用UseSingleton即可
>* 多线程需要使用UseScope
>* Mvc(含WebApi)逻辑处理使用UseContext
>* UseContext需要引用nuget包PocoEmit.Mvc
>* 如果是Mvc异步处理或Quartz类似作业不要用UseContext
>* 就怕异步中获取到了HttpContext,但执行中途被释放了,后面就可能异常了


另外源码托管地址: https://github.com/donetsoftwork/MyEmit ，欢迎大家直接查看源码。
gitee同步更新:https://gitee.com/donetsoftwork/MyEmit

如果大家喜欢请动动您发财的小手手帮忙点一下Star,谢谢！！！