# Emit映射集成IOC服务

## 一、注入IOC容器
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

## 二、当然还可以注入容器内的服务
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

## 三、支持IOC容器的特性
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
