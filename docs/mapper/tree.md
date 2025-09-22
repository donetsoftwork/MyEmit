# Poco支持循环引用

## 一、循环引用
### 1. 什么是循环引用
>循环引用就是类型相互依赖

#### 1.1 比如A类有B类的属性,B类也有A类的属性
>* 这有什么问题呢?
>* 编写生成A的代码需要遍历A的所有属性
>* 构造B类型属性是A代码的一部分,B代码有含有A类型属性
>* 这就构成一个循环,处理不好就是个死循环

#### 1.2 其他循环引用例子
>* 链表结构只有一个类型也是循环引用
>* A-B-C-A等更长的引用链条也都会构成循环引用

## 二、举个链表的例子
>* 链表应该是最简单的循环引用
>* 中国传统有九九归一的说法,以此为例

### 1. 九九归一代码
~~~csharp
public class Node
{
    public int Id { get; set; }
    public string Name { get; set; }
    public Node Next { get; set; }
    public static Node GetNode()
    {
        Node node9 = new() { Id = 9, Name = "node9" };
        Node node8 = new() { Id = 8, Name = "node8", Next = node9 };
        Node node7 = new() { Id = 7, Name = "node7", Next = node8 };
        Node node6 = new() { Id = 6, Name = "node6", Next = node7 };
        Node node5 = new() { Id = 5, Name = "node5", Next = node6 };
        Node node4 = new() { Id = 4, Name = "node4", Next = node5 };
        Node node3 = new() { Id = 3, Name = "node3", Next = node4 };
        Node node2 = new() { Id = 2, Name = "node2", Next = node3 };
        Node node1 = new() { Id = 1, Name = "node1", Next = node2 };
        node9.Next = node1; // 形成环
        return node1;
    }
}
~~~

### 2. 把Node转化为NodeDTO


### 3. 与AutoMapper性能对比如下

| Method   | Mean     | Error    | StdDev   | Median   | Ratio | RatioSD | Gen0   | Gen1   | Allocated | Alloc Ratio |
|--------- |---------:|---------:|---------:|---------:|------:|--------:|-------:|-------:|----------:|------------:|
| Auto     | 670.6 ns | 16.80 ns | 17.98 ns | 670.8 ns |  1.34 |    0.04 | 0.0937 | 0.0004 |    1616 B |        4.12 |
| AutoFunc | 637.0 ns |  3.66 ns |  3.91 ns | 634.0 ns |  1.28 |    0.01 | 0.0937 | 0.0004 |    1616 B |        4.12 |
| Poco     | 499.4 ns |  1.46 ns |  1.63 ns | 500.0 ns |  1.00 |    0.00 | 0.0227 | 0.0001 |     392 B |        1.00 |
| PocoFunc | 482.2 ns |  3.52 ns |  3.91 ns | 480.3 ns |  0.97 |    0.01 | 0.0209 |      - |     360 B |        0.92 |

>* 首先可以看出Poco和AutoMapper执行耗时都挺高的,所以建议大家尽量避免循环引用
>* Poco性能较好,如果基础类型字段多一些,优势会更明显
>* 内存分配上Poco优势明显,AutoMapper分配了4倍多的内存

### 4. 与AutoMapper生成代码对比如下
#### 4.1 AutoMapper生成以下代码
~~~csharp
T __f<T>(System.Func<T> f) => f();
(Func<Node, NodeDTO, ResolutionContext, NodeDTO>)((
    Node source,
    NodeDTO destination,
    ResolutionContext context) => //NodeDTO
    (source == null) ?
        (destination == null) ? (NodeDTO)null : destination :
        __f(() => {
            NodeDTO typeMapDestination = null;
            ResolutionContext.CheckContext(ref context);
            return ((NodeDTO)context.GetDestination(
                source,
                typeof(NodeDTO))) ??
                __f(() => {
                    typeMapDestination = destination ?? new NodeDTO();
                    context.CacheDestination(
                        source,
                        typeof(NodeDTO),
                        typeMapDestination);
                    typeMapDestination;
                    try
                    {
                        typeMapDestination.Id = source.Id;
                    }
                    catch (Exception ex)
                    {
                        throw TypeMapPlanBuilder.MemberMappingError(
                            ex,
                            default(PropertyMap)/*NOTE: Provide the non-default value for the Constant!*/);
                    }
                    try
                    {
                        typeMapDestination.Name = source.Name;
                    }
                    catch (Exception ex)
                    {
                        throw TypeMapPlanBuilder.MemberMappingError(
                            ex,
                            default(PropertyMap)/*NOTE: Provide the non-default value for the Constant!*/);
                    }
                    try
                    {
                        Node resolvedValue = null;
                        NodeDTO mappedValue = null;
                        resolvedValue = source.Next;
                        mappedValue = (resolvedValue == null) ? (NodeDTO)null :
                            context.MapInternal<Node, NodeDTO>(
                                resolvedValue,
                                (destination == null) ? (NodeDTO)null :
                                    typeMapDestination.Next,
                                (MemberMap)default(PropertyMap)/*NOTE: Provide the non-default value for the Constant!*/);
                        typeMapDestination.Next = mappedValue;
                    }
                    catch (Exception ex)
                    {
                        throw TypeMapPlanBuilder.MemberMappingError(
                            ex,
                            default(PropertyMap)/*NOTE: Provide the non-default value for the Constant!*/);
                    }
                    return typeMapDestination;
                });
        }));
~~~

#### 4.2 Poco生成以下代码
~~~csharp
(Func<Node, NodeDTO>)((Node source) => //NodeDTO
{
    IConvertContext context = null;
    NodeDTO dest = null;
    context = ConvertContext.CreateSingle<Node, NodeDTO>();
    if ((source != (Node)null))
    {
        dest = new NodeDTO();
        context.SetCache<Node, NodeDTO>(
            source,
            dest);
        Node member0 = null;
        dest.Id = source.Id;
        dest.Name = source.Name;
        member0 = source.Next;
        if ((member0 != null))
        {
            dest.Next = (member0 == (Node)null) ? (NodeDTO)null :
                context.Convert<Node, NodeDTO>(
                    (IContextConverter)default(ContextAchieved)/*NOTE: Provide the non-default value for the Constant!*/,
                    member0);
        }
    }
    context.Dispose();
    return dest;
});
~~~

#### 4.3 生成代码对比
>* 首先可以看出Poco生成的代码更简洁,更易读,基本贴近手写代码
>* AutoMapper生成了两倍多的代码
>* 其次AutoMapper生成try-catch和闭包调用代码,可能会影响性能
>* AutoMapper的context.CacheDestination对应Poco的context.SetCache
>* AutoMapper的ResolutionContext.CheckContext对应Poco的ConvertContext.CreateSingle
>* Poco有context.Dispose,AutoMapper没有
>* AutoMapper的context.MapInternal对应Poco的context.Convert,都是“代理调用”
>* 由于代码生成顺序的原因,被调用代码尚未生成,只能使用“代理调用”，特别是调用自身(LambdaExpression不能调用自身)
>* AutoMapper和Poco处理循环引用的原理是差不多的


## 二、再举个树状结构的例子
>* 树状结构在实际应用中更常见

### 1. 导航菜单代码
>导航菜单是一个典型的树状结构

```csharp
public class Menu
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public List<Menu> Children { get; set; }
    public static Menu GetMenu()
    {
        var programs = new Menu { Id = 2, Name = "Programs", Description = "程序" };
        var documents = new Menu { Id = 3, Name = "Documents", Description = "文档" };
        var settings = new Menu { Id = 4, Name = "Settings", Description = "设置" };
        var help = new Menu { Id = 5, Name = "Help", Description = "帮助" };
        var run = new Menu { Id = 6, Name = "Run", Description = "运行" };
        var shutdown = new Menu { Id = 7, Name = "Shut Down", Description = "关闭" };
        var start = new Menu { Id = 1, Name = "Start", Description = "开始", Children = [programs, documents, settings, help, run, shutdown] };
        return start;
    }
}
```

### 2. 把Menu转化为MenuDTO


### 3. 与AutoMapper性能对比如下

| Method   | Mean     | Error   | StdDev  | Ratio | RatioSD | Gen0   | Gen1   | Allocated | Alloc Ratio |
|--------- |---------:|--------:|--------:|------:|--------:|-------:|-------:|----------:|------------:|
| Auto     | 321.0 ns | 0.88 ns | 1.01 ns |  1.19 |    0.01 | 0.0751 | 0.0003 |    1296 B |        1.78 |
| AutoFunc | 283.7 ns | 4.12 ns | 4.74 ns |  1.06 |    0.02 | 0.0751 | 0.0003 |    1296 B |        1.78 |
| Poco     | 268.8 ns | 1.72 ns | 1.92 ns |  1.00 |    0.01 | 0.0422 |      - |     728 B |        1.00 |
| PocoFunc | 245.4 ns | 1.74 ns | 2.00 ns |  0.91 |    0.01 | 0.0403 |      - |     696 B |        0.96 |

>* Poco性能较好
>* 内存分配上Poco有优势,AutoMapper分配了几乎2倍的内存

### 4. 与AutoMapper生成代码对比如下
#### 4.1 AutoMapper生成以下代码
```csharp
T __f<T>(System.Func<T> f) => f();
(Func<Menu, MenuDTO, ResolutionContext, MenuDTO>)((
    Menu source,
    MenuDTO destination,
    ResolutionContext context) => //MenuDTO
    (source == null) ?
        (destination == null) ? (MenuDTO)null : destination :
        __f(() => {
            MenuDTO typeMapDestination = null;
            ResolutionContext.CheckContext(ref context);
            return ((MenuDTO)context.GetDestination(
                source,
                typeof(MenuDTO))) ??
                __f(() => {
                    typeMapDestination = destination ?? new MenuDTO();
                    context.CacheDestination(
                        source,
                        typeof(MenuDTO),
                        typeMapDestination);
                    typeMapDestination;
                    try
                    {
                        typeMapDestination.Id = source.Id;
                    }
                    catch (Exception ex)
                    {
                        throw TypeMapPlanBuilder.MemberMappingError(
                            ex,
                            default(PropertyMap)/*NOTE: Provide the non-default value for the Constant!*/);
                    }
                    try
                    {
                        typeMapDestination.Name = source.Name;
                    }
                    catch (Exception ex)
                    {
                        throw TypeMapPlanBuilder.MemberMappingError(
                            ex,
                            default(PropertyMap)/*NOTE: Provide the non-default value for the Constant!*/);
                    }
                    try
                    {
                        typeMapDestination.Description = source.Description;
                    }
                    catch (Exception ex)
                    {
                        throw TypeMapPlanBuilder.MemberMappingError(
                            ex,
                            default(PropertyMap)/*NOTE: Provide the non-default value for the Constant!*/);
                    }
                    try
                    {
                        List<Menu> resolvedValue = null;
                        List<MenuDTO> mappedValue = null;
                        resolvedValue = source.Children;
                        mappedValue = (resolvedValue == null) ?
                            new List<MenuDTO>() :
                            context.MapInternal<List<Menu>, List<MenuDTO>>(
                                resolvedValue,
                                (destination == null) ? (List<MenuDTO>)null :
                                    typeMapDestination.Children,
                                (MemberMap)default(PropertyMap)/*NOTE: Provide the non-default value for the Constant!*/);
                        typeMapDestination.Children = mappedValue;
                    }
                    catch (Exception ex)
                    {
                        throw TypeMapPlanBuilder.MemberMappingError(
                            ex,
                            default(PropertyMap)/*NOTE: Provide the non-default value for the Constant!*/);
                    }
                    return typeMapDestination;
                });
        }));
```

#### 4.2 Poco生成以下代码
```csharp
(Func<Menu, MenuDTO>)((Menu source) => //MenuDTO
{
    IConvertContext context = null;
    MenuDTO dest = null;
    context = ConvertContext.Create();
    dest = ((Func<IConvertContext, Menu, MenuDTO>)((
        IConvertContext context_1, 
        Menu source_1) => //MenuDTO
    {
        MenuDTO dest_1 = null;
        if ((source_1 != (Menu)null))
        {
            dest_1 = new MenuDTO();
            context_1.SetCache<Menu, MenuDTO>(
                source_1,
                dest_1);
            List<Menu> member0 = null;
            dest_1.Id = source_1.Id;
            dest_1.Name = source_1.Name;
            dest_1.Description = source_1.Description;
            member0 = source_1.Children;
            if ((member0 != null))
            {
                dest_1.Children = (member0 == (List<Menu>)null) ? (List<MenuDTO>)null : 
                    context_1.Convert<List<Menu>, List<MenuDTO>>(
                        (IContextConverter)default(ContextAchieved)/*NOTE: Provide the non-default value for the Constant!*/,
                        member0);
            }
        }
        return dest_1;
    }))
    .Invoke(
        context,
        source);
    context.Dispose();
    return dest;
});
```

#### 4.3 生成代码对比
>* 首先Poco生成的代码还是更简洁
>* AutoMapper还是两倍多的代码
>* AutoMapper的MapInternal<List<Menu>, List<MenuDTO>>对应Poco的Convert<List<Menu>, List<MenuDTO>>
>* 以上AutoMapper和Poco的代码展示都不全,由于集合处理是通过代理调用,那部分代码未关联出来,这里就不再赘述

#### 4.4 树状结构和链表的差异
>* 树状结构例子使用的是ConvertContext.Create
>* 前面链表例子是ConvertContext.CreateSingle,CreateSingle性能更好,内存分配更少
>* 树状结构有两个循环引用类型,Menu和List<Menu>
>* 如果检测到只有一个循环引用类型,Poco就使用CreateSingle
>* 如果没检测到循环引用类型,Poco就不生成context代码

## 三、再举个逼真树的例子
### 1. 树的构成
>* 树有树根,树枝,叶子,花,果实等部分组成
>* 树根的分叉还是树根,构成循环引用
>* 树枝的分叉还是树枝,也构成循环引用

```csharp
/// <summary>
/// 树
/// </summary>
public class Tree
{
    public int Id { get; set; }
    public List<TreeRoot> Roots { get; set; }
    public TreeBranch Trunk { get; set; }
}
/// <summary>
/// 树根
/// </summary>
public class TreeRoot
{
    public int Id { get; set; }
    public List<TreeRoot> Roots { get; set; }
}
/// <summary>
/// 树枝
/// </summary>
public class TreeBranch
{
    public int Id { get; set; }
    public List<TreeBranch> Branches { get; set; }
    public List<Leaf> Leaves { get; set; }
    public List<Flower> Flowers { get; set; }
    public List<Fruit> Fruits { get; set; }
}
/// <summary>
/// 叶子
/// </summary>
public class Leaf
{
    public int Id { get; set; }
}
/// <summary>
/// 花
/// </summary>
public class Flower
{
    public int Id { get; set; }
}
/// <summary>
/// 果实
/// </summary>
public class Fruit
{
    public int Id { get; set; }
}
```

### 2. 如下生成一个小树的对象
```csharp
#region root1
var root11 = new TreeRoot { Id = 11 };
var root12 = new TreeRoot { Id = 12 };
var root1 = new TreeRoot { Id = 1, Roots = [root11, root12] };
#endregion
#region root2
var root21 = new TreeRoot { Id = 21 };
var root22 = new TreeRoot { Id = 22 };
var root2 = new TreeRoot { Id = 2, Roots = [root21, root22] };
#endregion
#region trunk
#region branch1
var leaf11 = new Leaf { Id = 111 };
var leaf12 = new Leaf { Id = 112 };
var flower11 = new Flower { Id = 111 };
var fruit11 = new Fruit { Id = 111 };
var branch1 = new TreeBranch { Id = 12, Leaves = [leaf11, leaf12], Flowers = [flower11], Fruits = [fruit11] };
#endregion
#region branch2
var leaf21 = new Leaf { Id = 121 };
var leaf22 = new Leaf { Id = 122 };
var flower21 = new Flower { Id = 121 };
var fruit21 = new Fruit { Id = 121 };
var branch2 = new TreeBranch { Id = 13, Leaves = [leaf21, leaf22], Flowers = [flower21], Fruits = [fruit21] };
#endregion
var trunk = new TreeBranch { Id = 11, Branches = [branch1, branch2] };
#endregion
var tree = new Tree { Id = 1, Roots = [root1, root2], Trunk = trunk };
```

### 3. 现在把树转化为结构相同的DTO
#### 3.1 Poco不要任何特殊配置就可以直接转化
```csharp
TreeDTO dto = PocoEmit.Mapper.Default.Convert<Tree, TreeDTO>(tree);
```

#### 3.2 AutoMapper需要配置Tree及其所有子类型的映射关系
```csharp
    cfg.CreateMap<Tree, TreeDTO>();
    cfg.CreateMap<TreeRoot, TreeRootDTO>();
    cfg.CreateMap<TreeBranch, TreeBranchDTO>();
    cfg.CreateMap<Leaf, LeafDTO>();
    cfg.CreateMap<Flower, FlowerDTO>();
    cfg.CreateMap<Fruit, FruitDTO>();
```

```csharp
IMapper mapper = config.CreateMapper();
TreeDTO dto = mapper.Map<Tree, TreeDTO>(tree);
```


| Method     | Mean     | Error   | StdDev  | Ratio | Gen0   | Gen1   | Allocated | Alloc Ratio |
|----------- |---------:|--------:|--------:|------:|-------:|-------:|----------:|------------:|
| Auto       | 583.3 ns | 4.10 ns | 4.72 ns |  1.16 | 0.1205 | 0.0006 |   2.03 KB |        0.86 |
| AutoFunc   | 560.5 ns | 1.48 ns | 1.71 ns |  1.12 | 0.1205 | 0.0006 |   2.03 KB |        0.86 |
| Poco       | 502.3 ns | 0.32 ns | 0.36 ns |  1.00 | 0.1395 | 0.0010 |   2.35 KB |        1.00 |
| PocoFunc   | 484.0 ns | 0.26 ns | 0.28 ns |  0.96 | 0.1377 | 0.0010 |   2.32 KB |        0.99 |
| Invoke     | 502.1 ns | 3.46 ns | 3.99 ns |  1.00 | 0.1395 | 0.0010 |   2.35 KB |        1.00 |
| InvokeFunc | 483.3 ns | 1.30 ns | 1.50 ns |  0.96 | 0.1377 | 0.0010 |   2.32 KB |        0.99 |

| Method     | Mean     | Error   | StdDev   | Median   | Ratio | RatioSD | Gen0   | Gen1   | Allocated | Alloc Ratio |
|----------- |---------:|--------:|---------:|---------:|------:|--------:|-------:|-------:|----------:|------------:|
| Auto       | 591.4 ns | 1.57 ns |  1.80 ns | 591.4 ns |  1.15 |    0.02 | 0.1205 | 0.0006 |   2.03 KB |        1.49 |
| AutoFunc   | 580.9 ns | 7.25 ns |  8.06 ns | 574.2 ns |  1.13 |    0.03 | 0.1205 | 0.0006 |   2.03 KB |        1.49 |
| Poco       | 515.8 ns | 8.81 ns | 10.15 ns | 515.3 ns |  1.00 |    0.03 | 0.0806 | 0.0003 |   1.36 KB |        1.00 |
| PocoFunc   | 488.7 ns | 1.84 ns |  2.12 ns | 487.6 ns |  0.95 |    0.02 | 0.0788 | 0.0003 |   1.33 KB |        0.98 |
| Invoke     | 510.3 ns | 0.74 ns |  0.86 ns | 510.3 ns |  0.99 |    0.02 | 0.0806 | 0.0003 |   1.36 KB |        1.00 |
| InvokeFunc | 484.8 ns | 3.66 ns |  4.07 ns | 481.2 ns |  0.94 |    0.02 | 0.0788 | 0.0003 |   1.33 KB |        0.98 |

| Method     | Mean     | Error    | StdDev   | Median   | Ratio | RatioSD | Gen0   | Gen1   | Allocated | Alloc Ratio |
|----------- |---------:|---------:|---------:|---------:|------:|--------:|-------:|-------:|----------:|------------:|
| Auto       | 689.0 ns | 14.30 ns | 15.89 ns | 675.3 ns |  1.28 |    0.03 | 0.1530 | 0.0012 |   2.58 KB |        1.47 |
| AutoFunc   | 636.6 ns |  2.02 ns |  2.16 ns | 635.2 ns |  1.19 |    0.01 | 0.1530 | 0.0012 |   2.58 KB |        1.47 |
| Poco       | 537.0 ns |  1.49 ns |  1.72 ns | 537.1 ns |  1.00 |    0.00 | 0.1038 | 0.0004 |   1.75 KB |        1.00 |
| PocoFunc   | 510.1 ns |  1.92 ns |  2.21 ns | 510.3 ns |  0.95 |    0.00 | 0.1020 | 0.0004 |   1.72 KB |        0.98 |
| Invoke     | 545.5 ns |  4.05 ns |  4.66 ns | 547.5 ns |  1.02 |    0.01 | 0.1038 | 0.0004 |   1.75 KB |        1.00 |
| InvokeFunc | 511.2 ns |  1.44 ns |  1.60 ns | 511.1 ns |  0.95 |    0.00 | 0.1020 | 0.0004 |   1.72 KB |        0.98 |

| Method     | Mean     | Error   | StdDev  | Ratio | Gen0   | Gen1   | Allocated | Alloc Ratio |
|----------- |---------:|--------:|--------:|------:|-------:|-------:|----------:|------------:|
| Auto       | 671.9 ns | 2.07 ns | 2.31 ns |  1.27 | 0.1530 | 0.0012 |   2.58 KB |        1.47 |
| AutoFunc   | 643.1 ns | 1.57 ns | 1.68 ns |  1.21 | 0.1530 | 0.0012 |   2.58 KB |        1.47 |
| Poco       | 529.9 ns | 0.53 ns | 0.61 ns |  1.00 | 0.1038 | 0.0004 |   1.75 KB |        1.00 |
| PocoFunc   | 506.8 ns | 1.52 ns | 1.75 ns |  0.96 | 0.1020 | 0.0004 |   1.72 KB |        0.98 |
| Invoke     | 531.7 ns | 1.73 ns | 1.99 ns |  1.00 | 0.1038 | 0.0004 |   1.75 KB |        1.00 |
| InvokeFunc | 509.7 ns | 1.15 ns | 1.33 ns |  0.96 | 0.1020 | 0.0004 |   1.72 KB |        0.98 |

| Method     | Mean     | Error   | StdDev  | Ratio | Gen0   | Gen1   | Allocated | Alloc Ratio |
|----------- |---------:|--------:|--------:|------:|-------:|-------:|----------:|------------:|
| Auto       | 675.5 ns | 0.75 ns | 0.83 ns |  1.27 | 0.1530 | 0.0012 |   2.58 KB |        1.47 |
| AutoFunc   | 639.9 ns | 2.58 ns | 2.97 ns |  1.20 | 0.1530 | 0.0012 |   2.58 KB |        1.47 |
| Poco       | 532.5 ns | 1.63 ns | 1.81 ns |  1.00 | 0.1038 | 0.0004 |   1.75 KB |        1.00 |
| PocoFunc   | 506.9 ns | 1.85 ns | 2.13 ns |  0.95 | 0.1020 | 0.0004 |   1.72 KB |        0.98 |
| Invoke     | 525.9 ns | 2.28 ns | 2.63 ns |  0.99 | 0.1038 | 0.0004 |   1.75 KB |        1.00 |
| InvokeFunc | 504.5 ns | 0.65 ns | 0.72 ns |  0.95 | 0.1020 | 0.0004 |   1.72 KB |        0.98 |

## Poco生成以下代码
```csharp
(Func<Tree2, TreeDTO2>)((Tree2 source) => //TreeDTO2
{
    IConvertContext context = null;
    TreeDTO2 dest = null;
    context = ConvertContext.Create();
    dest = ((Func<IConvertContext, Tree2, TreeDTO2>)((
        IConvertContext context_1,
        Tree2 source_1) => //TreeDTO2
    {
        TreeDTO2 dest_1 = null;
        if ((source_1 != (Tree2)null))
        {
            dest_1 = new TreeDTO2();
            context_1.SetCache<Tree2, TreeDTO2>(
                source_1,
                dest_1);
            List<TreeRoot2> member0 = null;
            TreeBranch2 member1 = null;
            dest_1.Id = source_1.Id;
            member0 = source_1.Roots;
            if ((member0 != null))
            {
                dest_1.Roots = (member0 == (List<TreeRoot2>)null) ? (List<TreeRootDTO2>)null :
                    context_1.Convert<List<TreeRoot2>, List<TreeRootDTO2>>(
                        (IContextConverter)default(ContextAchieved)/*NOTE: Provide the non-default value for the Constant!*/,
                        member0);
            }
            member1 = source_1.Trunk;
            if ((member1 != null))
            {
                dest_1.Trunk = (member1 == (TreeBranch2)null) ? (TreeBranchDTO2)null :
                    context_1.Convert<TreeBranch2, TreeBranchDTO2>(
                        (IContextConverter)default(ContextAchieved)/*NOTE: Provide the non-default value for the Constant!*/,
                        member1);
            }
        }
        return dest_1;
    }))
    .Invoke(
        context,
        source);
    context.Dispose();
    return dest;
});
```

## AutoMapper生成以下代码
```csharp
T __f<T>(System.Func<T> f) => f();
(Func<Tree2, TreeDTO2, ResolutionContext, TreeDTO2>)((
    Tree2 source,
    TreeDTO2 destination,
    ResolutionContext context) => //TreeDTO2
    (source == null) ?
        (destination == null) ? (TreeDTO2)null : destination :
        __f(() => {
            TreeDTO2 typeMapDestination = null;
            ResolutionContext.CheckContext(ref context);
            return ((TreeDTO2)context.GetDestination(
                source,
                typeof(TreeDTO2))) ??
                __f(() => {
                    typeMapDestination = destination ?? new TreeDTO2();
                    context.CacheDestination(
                        source,
                        typeof(TreeDTO2),
                        typeMapDestination);
                    typeMapDestination;
                    try
                    {
                        typeMapDestination.Id = source.Id;
                    }
                    catch (Exception ex)
                    {
                        throw TypeMapPlanBuilder.MemberMappingError(
                            ex,
                            default(PropertyMap)/*NOTE: Provide the non-default value for the Constant!*/);
                    }
                    try
                    {
                        List<TreeRoot2> resolvedValue = null;
                        List<TreeRootDTO2> mappedValue = null;
                        resolvedValue = source.Roots;
                        mappedValue = (resolvedValue == null) ?
                            new List<TreeRootDTO2>() :
                            context.MapInternal<List<TreeRoot2>, List<TreeRootDTO2>>(
                                resolvedValue,
                                (destination == null) ? (List<TreeRootDTO2>)null :
                                    typeMapDestination.Roots,
                                (MemberMap)default(PropertyMap)/*NOTE: Provide the non-default value for the Constant!*/);
                        typeMapDestination.Roots = mappedValue;
                    }
                    catch (Exception ex)
                    {
                        throw TypeMapPlanBuilder.MemberMappingError(
                            ex,
                            default(PropertyMap)/*NOTE: Provide the non-default value for the Constant!*/);
                    }
                    try
                    {
                        TreeBranch2 resolvedValue_1 = null;
                        TreeBranchDTO2 mappedValue_1 = null;
                        resolvedValue_1 = source.Trunk;
                        mappedValue_1 = (resolvedValue_1 == null) ? (TreeBranchDTO2)null :
                            context.MapInternal<TreeBranch2, TreeBranchDTO2>(
                                resolvedValue_1,
                                (destination == null) ? (TreeBranchDTO2)null :
                                    typeMapDestination.Trunk,
                                (MemberMap)default(PropertyMap)/*NOTE: Provide the non-default value for the Constant!*/);
                        typeMapDestination.Trunk = mappedValue_1;
                    }
                    catch (Exception ex)
                    {
                        throw TypeMapPlanBuilder.MemberMappingError(
                            ex,
                            default(PropertyMap)/*NOTE: Provide the non-default value for the Constant!*/);
                    }
                    return typeMapDestination;
                });
        }));
```

## AutoMapper生成List\<TreeRoot\>代码如下
```csharp
T __f<T>(System.Func<T> f) => f();
(Func<List<TreeRoot2>, List<TreeRootDTO2>, ResolutionContext, List<TreeRootDTO2>>)((
    List<TreeRoot2> source,
    List<TreeRootDTO2> mapperDestination,
    ResolutionContext context) => //List<TreeRootDTO2>
    (source == null) ?
        new List<TreeRootDTO2>() :
        __f(() => {
            try
            {
                List<TreeRootDTO2> collectionDestination = null;
                List<TreeRootDTO2> passedDestination = null;
                ResolutionContext.CheckContext(ref context);
                passedDestination = mapperDestination;
                collectionDestination = passedDestination ?? new List<TreeRootDTO2>();
                collectionDestination.Clear();
                List<TreeRoot2>.Enumerator enumerator = default;
                TreeRoot2 item = null;
                enumerator = source.GetEnumerator();
                try
                {
                    while (true)
                    {
                        if (enumerator.MoveNext())
                        {
                            item = enumerator.Current;
                            collectionDestination.Add(((Func<TreeRoot2, TreeRootDTO2, ResolutionContext, TreeRootDTO2>)((
                                TreeRoot2 source_1,
                                TreeRootDTO2 destination,
                                ResolutionContext context) => //TreeRootDTO2
                                (source_1 == null) ?
                                    (destination == null) ? (TreeRootDTO2)null : destination :
                                    __f(() => {
                                        TreeRootDTO2 typeMapDestination = null;
                                        ResolutionContext.CheckContext(ref context);
                                        return ((TreeRootDTO2)context.GetDestination(
                                            source_1,
                                            typeof(TreeRootDTO2))) ??
                                            __f(() => {
                                                typeMapDestination = destination ?? new TreeRootDTO2();
                                                context.CacheDestination(
                                                    source_1,
                                                    typeof(TreeRootDTO2),
                                                    typeMapDestination);
                                                typeMapDestination;
                                                try
                                                {
                                                    typeMapDestination.Id = source_1.Id;
                                                }
                                                catch (Exception ex)
                                                {
                                                    throw TypeMapPlanBuilder.MemberMappingError(
                                                        ex,
                                                        default(PropertyMap)/*NOTE: Provide the non-default value for the Constant!*/);
                                                }
                                                try
                                                {
                                                    Tree2 resolvedValue = null;
                                                    TreeDTO2 mappedValue = null;
                                                    resolvedValue = source_1.Tree;
                                                    mappedValue = (resolvedValue == null) ? (TreeDTO2)null :
                                                        context.MapInternal<Tree2, TreeDTO2>(
                                                            resolvedValue,
                                                            (destination == null) ? (TreeDTO2)null :
                                                                typeMapDestination.Tree,
                                                            (MemberMap)default(PropertyMap)/*NOTE: Provide the non-default value for the Constant!*/);
                                                    typeMapDestination.Tree = mappedValue;
                                                }
                                                catch (Exception ex)
                                                {
                                                    throw TypeMapPlanBuilder.MemberMappingError(
                                                        ex,
                                                        default(PropertyMap)/*NOTE: Provide the non-default value for the Constant!*/);
                                                }
                                                try
                                                {
                                                    TreeRoot2 resolvedValue_1 = null;
                                                    TreeRootDTO2 mappedValue_1 = null;
                                                    resolvedValue_1 = source_1.Parent;
                                                    mappedValue_1 = (resolvedValue_1 == null) ? (TreeRootDTO2)null :
                                                        context.MapInternal<TreeRoot2, TreeRootDTO2>(
                                                            resolvedValue_1,
                                                            (destination == null) ? (TreeRootDTO2)null :
                                                                typeMapDestination.Parent,
                                                            (MemberMap)default(PropertyMap)/*NOTE: Provide the non-default value for the Constant!*/);
                                                    typeMapDestination.Parent = mappedValue_1;
                                                }
                                                catch (Exception ex)
                                                {
                                                    throw TypeMapPlanBuilder.MemberMappingError(
                                                        ex,
                                                        default(PropertyMap)/*NOTE: Provide the non-default value for the Constant!*/);
                                                }
                                                try
                                                {
                                                    List<TreeRoot2> resolvedValue_2 = null;
                                                    List<TreeRootDTO2> mappedValue_2 = null;
                                                    resolvedValue_2 = source_1.Roots;
                                                    mappedValue_2 = (resolvedValue_2 == null) ?
                                                        new List<TreeRootDTO2>() :
                                                        context.MapInternal<List<TreeRoot2>, List<TreeRootDTO2>>(
                                                            resolvedValue_2,
                                                            (destination == null) ? (List<TreeRootDTO2>)null :
                                                                typeMapDestination.Roots,
                                                            (MemberMap)default(PropertyMap)/*NOTE: Provide the non-default value for the Constant!*/);
                                                    typeMapDestination.Roots = mappedValue_2;
                                                }
                                                catch (Exception ex)
                                                {
                                                    throw TypeMapPlanBuilder.MemberMappingError(
                                                        ex,
                                                        default(PropertyMap)/*NOTE: Provide the non-default value for the Constant!*/);
                                                }
                                                return typeMapDestination;
                                            });
                                    })))
                            .Invoke(
                                item,
                                (TreeRootDTO2)null,
                                context));
                        }
                        else
                        {
                            goto LoopBreak;
                        }
                    }
                    LoopBreak:;
                }
                finally
                {
                    enumerator.Dispose();
                }
                return collectionDestination;
            }
            catch (Exception ex)
            {
                throw MapperConfiguration.GetMappingError(
                    ex,
                    default(MapRequest)/*NOTE: Provide the non-default value for the Constant!*/);
            }
        }));
```

## AutoMapper生成TreeBranch代码如下
```csharp
T __f<T>(System.Func<T> f) => f();
(Func<TreeBranch, TreeBranchDTO, ResolutionContext, TreeBranchDTO>)((
    TreeBranch source, 
    TreeBranchDTO destination, 
    ResolutionContext context) => //TreeBranchDTO
    (source == null) ? 
        (destination == null) ? (TreeBranchDTO)null : destination : 
        __f(() => {
            TreeBranchDTO typeMapDestination = null;
            ResolutionContext.CheckContext(ref context);
            return ((TreeBranchDTO)context.GetDestination(
                source,
                typeof(TreeBranchDTO))) ?? 
                __f(() => {
                    typeMapDestination = destination ?? new TreeBranchDTO();
                    context.CacheDestination(
                        source,
                        typeof(TreeBranchDTO),
                        typeMapDestination);
                    typeMapDestination;
                    try
                    {
                        typeMapDestination.Id = source.Id;
                    }
                    catch (Exception ex)
                    {
                        throw TypeMapPlanBuilder.MemberMappingError(
                            ex,
                            default(PropertyMap)/*NOTE: Provide the non-default value for the Constant!*/);
                    }
                    try
                    {
                        List<TreeBranch> resolvedValue = null;
                        List<TreeBranchDTO> mappedValue = null;
                        resolvedValue = source.Branches;
                        mappedValue = (resolvedValue == null) ? 
                            new List<TreeBranchDTO>() : 
                            context.MapInternal<List<TreeBranch>, List<TreeBranchDTO>>(
                                resolvedValue,
                                (destination == null) ? (List<TreeBranchDTO>)null : 
                                    typeMapDestination.Branches,
                                (MemberMap)default(PropertyMap)/*NOTE: Provide the non-default value for the Constant!*/);
                        typeMapDestination.Branches = mappedValue;
                    }
                    catch (Exception ex)
                    {
                        throw TypeMapPlanBuilder.MemberMappingError(
                            ex,
                            default(PropertyMap)/*NOTE: Provide the non-default value for the Constant!*/);
                    }
                    try
                    {
                        List<Leaf> resolvedValue_1 = null;
                        List<LeafDTO> mappedValue_1 = null;
                        resolvedValue_1 = source.Leaves;
                        mappedValue_1 = (resolvedValue_1 == null) ? 
                            new List<LeafDTO>() : 
                            __f(() => {
                                List<LeafDTO> collectionDestination = null;
                                List<LeafDTO> passedDestination = null;
                                passedDestination = (destination == null) ? (List<LeafDTO>)null : 
                                    typeMapDestination.Leaves;
                                collectionDestination = passedDestination ?? new List<LeafDTO>();
                                collectionDestination.Clear();
                                List<Leaf>.Enumerator enumerator = default;
                                Leaf item = null;
                                enumerator = resolvedValue_1.GetEnumerator();
                                try
                                {
                                    while (true)
                                    {
                                        if (enumerator.MoveNext())
                                        {
                                            item = enumerator.Current;
                                            collectionDestination.Add(((Func<Leaf, LeafDTO, ResolutionContext, LeafDTO>)((
                                                Leaf source_1, 
                                                LeafDTO destination_1, 
                                                ResolutionContext context) => //LeafDTO
                                                (source_1 == null) ? 
                                                    (destination_1 == null) ? (LeafDTO)null : destination_1 : 
                                                    __f(() => {
                                                        LeafDTO typeMapDestination_1 = null;
                                                        typeMapDestination_1 = destination_1 ?? new LeafDTO();
                                                        try
                                                        {
                                                            typeMapDestination_1.Id = source_1.Id;
                                                        }
                                                        catch (Exception ex)
                                                        {
                                                            throw TypeMapPlanBuilder.MemberMappingError(
                                                                ex,
                                                                default(PropertyMap)/*NOTE: Provide the non-default value for the Constant!*/);
                                                        }
                                                        return typeMapDestination_1;
                                                    })))
                                            .Invoke(
                                                item,
                                                (LeafDTO)null,
                                                context));
                                        }
                                        else
                                        {
                                            goto LoopBreak;
                                        }
                                    }
                                    LoopBreak:;
                                }
                                finally
                                {
                                    enumerator.Dispose();
                                }
                                return collectionDestination;
                            });
                        typeMapDestination.Leaves = mappedValue_1;
                    }
                    catch (Exception ex)
                    {
                        throw TypeMapPlanBuilder.MemberMappingError(
                            ex,
                            default(PropertyMap)/*NOTE: Provide the non-default value for the Constant!*/);
                    }
                    try
                    {
                        List<Flower> resolvedValue_2 = null;
                        List<FlowerDTO> mappedValue_2 = null;
                        resolvedValue_2 = source.Flowers;
                        mappedValue_2 = (resolvedValue_2 == null) ? 
                            new List<FlowerDTO>() : 
                            __f(() => {
                                List<FlowerDTO> collectionDestination_1 = null;
                                List<FlowerDTO> passedDestination_1 = null;
                                passedDestination_1 = (destination == null) ? (List<FlowerDTO>)null : 
                                    typeMapDestination.Flowers;
                                collectionDestination_1 = passedDestination_1 ?? new List<FlowerDTO>();
                                collectionDestination_1.Clear();
                                List<Flower>.Enumerator enumerator_1 = default;
                                Flower item_1 = null;
                                enumerator_1 = resolvedValue_2.GetEnumerator();
                                try
                                {
                                    while (true)
                                    {
                                        if (enumerator_1.MoveNext())
                                        {
                                            item_1 = enumerator_1.Current;
                                            collectionDestination_1.Add(((Func<Flower, FlowerDTO, ResolutionContext, FlowerDTO>)((
                                                Flower source_2, 
                                                FlowerDTO destination_2, 
                                                ResolutionContext context) => //FlowerDTO
                                                (source_2 == null) ? 
                                                    (destination_2 == null) ? (FlowerDTO)null : destination_2 : 
                                                    __f(() => {
                                                        FlowerDTO typeMapDestination_2 = null;
                                                        typeMapDestination_2 = destination_2 ?? new FlowerDTO();
                                                        try
                                                        {
                                                            typeMapDestination_2.Id = source_2.Id;
                                                        }
                                                        catch (Exception ex)
                                                        {
                                                            throw TypeMapPlanBuilder.MemberMappingError(
                                                                ex,
                                                                default(PropertyMap)/*NOTE: Provide the non-default value for the Constant!*/);
                                                        }
                                                        return typeMapDestination_2;
                                                    })))
                                            .Invoke(
                                                item_1,
                                                (FlowerDTO)null,
                                                context));
                                        }
                                        else
                                        {
                                            goto LoopBreak_1;
                                        }
                                    }
                                    LoopBreak_1:;
                                }
                                finally
                                {
                                    enumerator_1.Dispose();
                                }
                                return collectionDestination_1;
                            });
                        typeMapDestination.Flowers = mappedValue_2;
                    }
                    catch (Exception ex)
                    {
                        throw TypeMapPlanBuilder.MemberMappingError(
                            ex,
                            default(PropertyMap)/*NOTE: Provide the non-default value for the Constant!*/);
                    }
                    try
                    {
                        List<Fruit> resolvedValue_3 = null;
                        List<FruitDTO> mappedValue_3 = null;
                        resolvedValue_3 = source.Fruits;
                        mappedValue_3 = (resolvedValue_3 == null) ? 
                            new List<FruitDTO>() : 
                            __f(() => {
                                List<FruitDTO> collectionDestination_2 = null;
                                List<FruitDTO> passedDestination_2 = null;
                                passedDestination_2 = (destination == null) ? (List<FruitDTO>)null : 
                                    typeMapDestination.Fruits;
                                collectionDestination_2 = passedDestination_2 ?? new List<FruitDTO>();
                                collectionDestination_2.Clear();
                                List<Fruit>.Enumerator enumerator_2 = default;
                                Fruit item_2 = null;
                                enumerator_2 = resolvedValue_3.GetEnumerator();
                                try
                                {
                                    while (true)
                                    {
                                        if (enumerator_2.MoveNext())
                                        {
                                            item_2 = enumerator_2.Current;
                                            collectionDestination_2.Add(((Func<Fruit, FruitDTO, ResolutionContext, FruitDTO>)((
                                                Fruit source_3, 
                                                FruitDTO destination_3, 
                                                ResolutionContext context) => //FruitDTO
                                                (source_3 == null) ? 
                                                    (destination_3 == null) ? (FruitDTO)null : destination_3 : 
                                                    __f(() => {
                                                        FruitDTO typeMapDestination_3 = null;
                                                        typeMapDestination_3 = destination_3 ?? new FruitDTO();
                                                        try
                                                        {
                                                            typeMapDestination_3.Id = source_3.Id;
                                                        }
                                                        catch (Exception ex)
                                                        {
                                                            throw TypeMapPlanBuilder.MemberMappingError(
                                                                ex,
                                                                default(PropertyMap)/*NOTE: Provide the non-default value for the Constant!*/);
                                                        }
                                                        return typeMapDestination_3;
                                                    })))
                                            .Invoke(
                                                item_2,
                                                (FruitDTO)null,
                                                context));
                                        }
                                        else
                                        {
                                            goto LoopBreak_2;
                                        }
                                    }
                                    LoopBreak_2:;
                                }
                                finally
                                {
                                    enumerator_2.Dispose();
                                }
                                return collectionDestination_2;
                            });
                        typeMapDestination.Fruits = mappedValue_3;
                    }
                    catch (Exception ex)
                    {
                        throw TypeMapPlanBuilder.MemberMappingError(
                            ex,
                            default(PropertyMap)/*NOTE: Provide the non-default value for the Constant!*/);
                    }
                    return typeMapDestination;
                });
        }));
```

## Poco还支持Invoke模式
>Invoke模式生成如下代码

```csharp
(Func<Tree, TreeDTO>)((Tree source) => //TreeDTO
{
    IConvertContext context = null;
    TreeDTO dest = null;
    context = ConvertContext.Create();
    if ((source != (Tree)null))
    {
        dest = new TreeDTO();
        List<TreeRoot> member0 = null;
        TreeBranch member1 = null;
        dest.Id = source.Id;
        member0 = source.Roots;
        if ((member0 != null))
        {
            // { The block result will be assigned to `dest.Roots`
            List<TreeRootDTO> dest_1 = null;
            dest_1 = new List<TreeRootDTO>(member0.Count);
            int index = default;
            int len = default;
            index = 0;
            len = member0.Count;
            while (true)
            {
                if ((index < len))
                {
                    TreeRoot sourceItem = null;
                    TreeRootDTO destItem = null;
                    sourceItem = member0[index];
                    destItem = (sourceItem == (TreeRoot)null) ? (TreeRootDTO)null : 
                        ((Func<IConvertContext, TreeRoot, TreeRootDTO>)((
                            IConvertContext context_1, 
                            TreeRoot source_1) => //TreeRootDTO
                        {
                            TreeRootDTO dest_2 = null;
                            if ((source_1 != (TreeRoot)null))
                            {
                                dest_2 = new TreeRootDTO();
                                context_1.SetCache<TreeRoot, TreeRootDTO>(
                                    source_1,
                                    dest_2);
                                List<TreeRoot> member0_1 = null;
                                dest_2.Id = source_1.Id;
                                member0_1 = source_1.Roots;
                                if ((member0_1 != null))
                                {
                                    // { The block result will be assigned to `dest_2.Roots`
                                    List<TreeRootDTO> dest_3 = null;
                                    dest_3 = new List<TreeRootDTO>(member0_1.Count);
                                    int index_1 = default;
                                    int len_1 = default;
                                    index_1 = 0;
                                    len_1 = member0_1.Count;
                                    while (true)
                                    {
                                        if ((index_1 < len_1))
                                        {
                                            TreeRoot sourceItem_1 = null;
                                            TreeRootDTO destItem_1 = null;
                                            sourceItem_1 = member0_1[index_1];
                                            destItem_1 = (sourceItem_1 == (TreeRoot)null) ? (TreeRootDTO)null : 
                                                context_1.Convert<TreeRoot, TreeRootDTO>(
                                                    (IContextConverter)default(ContextAchieved)/*NOTE: Provide the non-default value for the Constant!*/,
                                                    sourceItem_1);
                                            dest_3.Add(destItem_1);
                                            index_1++;
                                        }
                                        else
                                        {
                                            goto forLabel;
                                        }
                                    }
                                    forLabel:;
                                    dest_2.Roots = dest_3;
                                    // } end of block assignment;
                                }
                            }
                            return dest_2;
                        }))
                        .Invoke(
                            context,
                            sourceItem);
                    dest_1.Add(destItem);
                    index++;
                }
                else
                {
                    goto forLabel_1;
                }
            }
            forLabel_1:;
            dest.Roots = dest_1;
            // } end of block assignment;
        }
        member1 = source.Trunk;
        if ((member1 != null))
        {
            dest.Trunk = (member1 == (TreeBranch)null) ? (TreeBranchDTO)null : 
                ((Func<IConvertContext, TreeBranch, TreeBranchDTO>)((
                    IConvertContext context_2, 
                    TreeBranch source_2) => //TreeBranchDTO
                {
                    TreeBranchDTO dest_4 = null;
                    if ((source_2 != (TreeBranch)null))
                    {
                        dest_4 = new TreeBranchDTO();
                        context_2.SetCache<TreeBranch, TreeBranchDTO>(
                            source_2,
                            dest_4);
                        List<TreeBranch> member0_2 = null;
                        List<Leaf> member1_1 = null;
                        List<Flower> member2 = null;
                        List<Fruit> member3 = null;
                        dest_4.Id = source_2.Id;
                        member0_2 = source_2.Branches;
                        if ((member0_2 != null))
                        {
                            // { The block result will be assigned to `dest_4.Branches`
                            List<TreeBranchDTO> dest_5 = null;
                            dest_5 = new List<TreeBranchDTO>(member0_2.Count);
                            int index_2 = default;
                            int len_2 = default;
                            index_2 = 0;
                            len_2 = member0_2.Count;
                            while (true)
                            {
                                if ((index_2 < len_2))
                                {
                                    TreeBranch sourceItem_2 = null;
                                    TreeBranchDTO destItem_2 = null;
                                    sourceItem_2 = member0_2[index_2];
                                    destItem_2 = (sourceItem_2 == (TreeBranch)null) ? (TreeBranchDTO)null : 
                                        context_2.Convert<TreeBranch, TreeBranchDTO>(
                                            (IContextConverter)default(ContextAchieved)/*NOTE: Provide the non-default value for the Constant!*/,
                                            sourceItem_2);
                                    dest_5.Add(destItem_2);
                                    index_2++;
                                }
                                else
                                {
                                    goto forLabel_2;
                                }
                            }
                            forLabel_2:;
                            dest_4.Branches = dest_5;
                            // } end of block assignment;
                        }
                        member1_1 = source_2.Leaves;
                        if ((member1_1 != null))
                        {
                            // { The block result will be assigned to `dest_4.Leaves`
                            List<LeafDTO> dest_6 = null;
                            dest_6 = new List<LeafDTO>(member1_1.Count);
                            int index_3 = default;
                            int len_3 = default;
                            index_3 = 0;
                            len_3 = member1_1.Count;
                            while (true)
                            {
                                if ((index_3 < len_3))
                                {
                                    Leaf sourceItem_3 = null;
                                    LeafDTO destItem_3 = null;
                                    sourceItem_3 = member1_1[index_3];
                                    destItem_3 = ((Func<Leaf, LeafDTO>)((Leaf source_3) => //LeafDTO
                                    {
                                        LeafDTO dest_7 = null;
                                        if ((source_3 != (Leaf)null))
                                        {
                                            dest_7 = new LeafDTO();
                                            dest_7.Id = source_3.Id;
                                        }
                                        return dest_7;
                                    }))
                                    .Invoke(
                                        sourceItem_3);
                                    dest_6.Add(destItem_3);
                                    index_3++;
                                }
                                else
                                {
                                    goto forLabel_3;
                                }
                            }
                            forLabel_3:;
                            dest_4.Leaves = dest_6;
                            // } end of block assignment;
                        }
                        member2 = source_2.Flowers;
                        if ((member2 != null))
                        {
                            // { The block result will be assigned to `dest_4.Flowers`
                            List<FlowerDTO> dest_8 = null;
                            dest_8 = new List<FlowerDTO>(member2.Count);
                            int index_4 = default;
                            int len_4 = default;
                            index_4 = 0;
                            len_4 = member2.Count;
                            while (true)
                            {
                                if ((index_4 < len_4))
                                {
                                    Flower sourceItem_4 = null;
                                    FlowerDTO destItem_4 = null;
                                    sourceItem_4 = member2[index_4];
                                    destItem_4 = ((Func<Flower, FlowerDTO>)((Flower source_4) => //FlowerDTO
                                    {
                                        FlowerDTO dest_9 = null;
                                        if ((source_4 != (Flower)null))
                                        {
                                            dest_9 = new FlowerDTO();
                                            dest_9.Id = source_4.Id;
                                        }
                                        return dest_9;
                                    }))
                                    .Invoke(
                                        sourceItem_4);
                                    dest_8.Add(destItem_4);
                                    index_4++;
                                }
                                else
                                {
                                    goto forLabel_4;
                                }
                            }
                            forLabel_4:;
                            dest_4.Flowers = dest_8;
                            // } end of block assignment;
                        }
                        member3 = source_2.Fruits;
                        if ((member3 != null))
                        {
                            // { The block result will be assigned to `dest_4.Fruits`
                            List<FruitDTO> dest_10 = null;
                            dest_10 = new List<FruitDTO>(member3.Count);
                            int index_5 = default;
                            int len_5 = default;
                            index_5 = 0;
                            len_5 = member3.Count;
                            while (true)
                            {
                                if ((index_5 < len_5))
                                {
                                    Fruit sourceItem_5 = null;
                                    FruitDTO destItem_5 = null;
                                    sourceItem_5 = member3[index_5];
                                    destItem_5 = ((Func<Fruit, FruitDTO>)((Fruit source_5) => //FruitDTO
                                    {
                                        FruitDTO dest_11 = null;
                                        if ((source_5 != (Fruit)null))
                                        {
                                            dest_11 = new FruitDTO();
                                            dest_11.Id = source_5.Id;
                                        }
                                        return dest_11;
                                    }))
                                    .Invoke(
                                        sourceItem_5);
                                    dest_10.Add(destItem_5);
                                    index_5++;
                                }
                                else
                                {
                                    goto forLabel_5;
                                }
                            }
                            forLabel_5:;
                            dest_4.Fruits = dest_10;
                            // } end of block assignment;
                        }
                    }
                    return dest_4;
                }))
                .Invoke(
                    context,
                    member1);
        }
    }
    context.Dispose();
    return dest;
});
```

## Auto转化Tree2代码如下
```csharp
T __f<T>(System.Func<T> f) => f();
(Func<Tree2, TreeDTO2, ResolutionContext, TreeDTO2>)((
    Tree2 source, 
    TreeDTO2 destination, 
    ResolutionContext context) => //TreeDTO2
    (source == null) ? 
        (destination == null) ? (TreeDTO2)null : destination : 
        __f(() => {
            TreeDTO2 typeMapDestination = null;
            ResolutionContext.CheckContext(ref context);
            return ((TreeDTO2)context.GetDestination(
                source,
                typeof(TreeDTO2))) ?? 
                __f(() => {
                    typeMapDestination = destination ?? new TreeDTO2();
                    context.CacheDestination(
                        source,
                        typeof(TreeDTO2),
                        typeMapDestination);
                    typeMapDestination;
                    try
                    {
                        typeMapDestination.Id = source.Id;
                    }
                    catch (Exception ex)
                    {
                        throw TypeMapPlanBuilder.MemberMappingError(
                            ex,
                            default(PropertyMap)/*NOTE: Provide the non-default value for the Constant!*/);
                    }
                    try
                    {
                        List<TreeRoot2> resolvedValue = null;
                        List<TreeRootDTO2> mappedValue = null;
                        resolvedValue = source.Roots;
                        mappedValue = (resolvedValue == null) ? 
                            new List<TreeRootDTO2>() : 
                            context.MapInternal<List<TreeRoot2>, List<TreeRootDTO2>>(
                                resolvedValue,
                                (destination == null) ? (List<TreeRootDTO2>)null : 
                                    typeMapDestination.Roots,
                                (MemberMap)default(PropertyMap)/*NOTE: Provide the non-default value for the Constant!*/);
                        typeMapDestination.Roots = mappedValue;
                    }
                    catch (Exception ex)
                    {
                        throw TypeMapPlanBuilder.MemberMappingError(
                            ex,
                            default(PropertyMap)/*NOTE: Provide the non-default value for the Constant!*/);
                    }
                    try
                    {
                        TreeBranch2 resolvedValue_1 = null;
                        TreeBranchDTO2 mappedValue_1 = null;
                        resolvedValue_1 = source.Trunk;
                        mappedValue_1 = (resolvedValue_1 == null) ? (TreeBranchDTO2)null : 
                            context.MapInternal<TreeBranch2, TreeBranchDTO2>(
                                resolvedValue_1,
                                (destination == null) ? (TreeBranchDTO2)null : 
                                    typeMapDestination.Trunk,
                                (MemberMap)default(PropertyMap)/*NOTE: Provide the non-default value for the Constant!*/);
                        typeMapDestination.Trunk = mappedValue_1;
                    }
                    catch (Exception ex)
                    {
                        throw TypeMapPlanBuilder.MemberMappingError(
                            ex,
                            default(PropertyMap)/*NOTE: Provide the non-default value for the Constant!*/);
                    }
                    return typeMapDestination;
                });
        }));
```

## Auto转化Tree2代码如下
```csharp
T __f<T>(System.Func<T> f) => f();
(Func<List<TreeRoot2>, List<TreeRootDTO2>, ResolutionContext, List<TreeRootDTO2>>)((
    List<TreeRoot2> source, 
    List<TreeRootDTO2> mapperDestination, 
    ResolutionContext context) => //List<TreeRootDTO2>
    (source == null) ? 
        new List<TreeRootDTO2>() : 
        __f(() => {
            try
            {
                List<TreeRootDTO2> collectionDestination = null;
                List<TreeRootDTO2> passedDestination = null;
                ResolutionContext.CheckContext(ref context);
                passedDestination = mapperDestination;
                collectionDestination = passedDestination ?? new List<TreeRootDTO2>();
                collectionDestination.Clear();
                List<TreeRoot2>.Enumerator enumerator = default;
                TreeRoot2 item = null;
                enumerator = source.GetEnumerator();
                try
                {
                    while (true)
                    {
                        if (enumerator.MoveNext())
                        {
                            item = enumerator.Current;
                            collectionDestination.Add(((Func<TreeRoot2, TreeRootDTO2, ResolutionContext, TreeRootDTO2>)((
                                TreeRoot2 source_1, 
                                TreeRootDTO2 destination, 
                                ResolutionContext context) => //TreeRootDTO2
                                (source_1 == null) ? 
                                    (destination == null) ? (TreeRootDTO2)null : destination : 
                                    __f(() => {
                                        TreeRootDTO2 typeMapDestination = null;
                                        ResolutionContext.CheckContext(ref context);
                                        return ((TreeRootDTO2)context.GetDestination(
                                            source_1,
                                            typeof(TreeRootDTO2))) ?? 
                                            __f(() => {
                                                typeMapDestination = destination ?? new TreeRootDTO2();
                                                context.CacheDestination(
                                                    source_1,
                                                    typeof(TreeRootDTO2),
                                                    typeMapDestination);
                                                typeMapDestination;
                                                try
                                                {
                                                    typeMapDestination.Id = source_1.Id;
                                                }
                                                catch (Exception ex)
                                                {
                                                    throw TypeMapPlanBuilder.MemberMappingError(
                                                        ex,
                                                        default(PropertyMap)/*NOTE: Provide the non-default value for the Constant!*/);
                                                }
                                                try
                                                {
                                                    Tree2 resolvedValue = null;
                                                    TreeDTO2 mappedValue = null;
                                                    resolvedValue = source_1.Tree;
                                                    mappedValue = (resolvedValue == null) ? (TreeDTO2)null : 
                                                        context.MapInternal<Tree2, TreeDTO2>(
                                                            resolvedValue,
                                                            (destination == null) ? (TreeDTO2)null : 
                                                                typeMapDestination.Tree,
                                                            (MemberMap)default(PropertyMap)/*NOTE: Provide the non-default value for the Constant!*/);
                                                    typeMapDestination.Tree = mappedValue;
                                                }
                                                catch (Exception ex)
                                                {
                                                    throw TypeMapPlanBuilder.MemberMappingError(
                                                        ex,
                                                        default(PropertyMap)/*NOTE: Provide the non-default value for the Constant!*/);
                                                }
                                                try
                                                {
                                                    TreeRoot2 resolvedValue_1 = null;
                                                    TreeRootDTO2 mappedValue_1 = null;
                                                    resolvedValue_1 = source_1.Parent;
                                                    mappedValue_1 = (resolvedValue_1 == null) ? (TreeRootDTO2)null : 
                                                        context.MapInternal<TreeRoot2, TreeRootDTO2>(
                                                            resolvedValue_1,
                                                            (destination == null) ? (TreeRootDTO2)null : 
                                                                typeMapDestination.Parent,
                                                            (MemberMap)default(PropertyMap)/*NOTE: Provide the non-default value for the Constant!*/);
                                                    typeMapDestination.Parent = mappedValue_1;
                                                }
                                                catch (Exception ex)
                                                {
                                                    throw TypeMapPlanBuilder.MemberMappingError(
                                                        ex,
                                                        default(PropertyMap)/*NOTE: Provide the non-default value for the Constant!*/);
                                                }
                                                try
                                                {
                                                    List<TreeRoot2> resolvedValue_2 = null;
                                                    List<TreeRootDTO2> mappedValue_2 = null;
                                                    resolvedValue_2 = source_1.Roots;
                                                    mappedValue_2 = (resolvedValue_2 == null) ? 
                                                        new List<TreeRootDTO2>() : 
                                                        context.MapInternal<List<TreeRoot2>, List<TreeRootDTO2>>(
                                                            resolvedValue_2,
                                                            (destination == null) ? (List<TreeRootDTO2>)null : 
                                                                typeMapDestination.Roots,
                                                            (MemberMap)default(PropertyMap)/*NOTE: Provide the non-default value for the Constant!*/);
                                                    typeMapDestination.Roots = mappedValue_2;
                                                }
                                                catch (Exception ex)
                                                {
                                                    throw TypeMapPlanBuilder.MemberMappingError(
                                                        ex,
                                                        default(PropertyMap)/*NOTE: Provide the non-default value for the Constant!*/);
                                                }
                                                return typeMapDestination;
                                            });
                                    })))
                            .Invoke(
                                item,
                                (TreeRootDTO2)null,
                                context));
                        }
                        else
                        {
                            goto LoopBreak;
                        }
                    }
                    LoopBreak:;
                }
                finally
                {
                    enumerator.Dispose();
                }
                return collectionDestination;
            }
            catch (Exception ex)
            {
                throw MapperConfiguration.GetMappingError(
                    ex,
                    default(MapRequest)/*NOTE: Provide the non-default value for the Constant!*/);
            }
        }));
```

## Auto转化TreeBranch2代码如下
```csharp
T __f<T>(System.Func<T> f) => f();
(Func<TreeBranch2, TreeBranchDTO2, ResolutionContext, TreeBranchDTO2>)((
    TreeBranch2 source, 
    TreeBranchDTO2 destination, 
    ResolutionContext context) => //TreeBranchDTO2
    (source == null) ? 
        (destination == null) ? (TreeBranchDTO2)null : destination : 
        __f(() => {
            TreeBranchDTO2 typeMapDestination = null;
            ResolutionContext.CheckContext(ref context);
            return ((TreeBranchDTO2)context.GetDestination(
                source,
                typeof(TreeBranchDTO2))) ?? 
                __f(() => {
                    typeMapDestination = destination ?? new TreeBranchDTO2();
                    context.CacheDestination(
                        source,
                        typeof(TreeBranchDTO2),
                        typeMapDestination);
                    typeMapDestination;
                    try
                    {
                        typeMapDestination.Id = source.Id;
                    }
                    catch (Exception ex)
                    {
                        throw TypeMapPlanBuilder.MemberMappingError(
                            ex,
                            default(PropertyMap)/*NOTE: Provide the non-default value for the Constant!*/);
                    }
                    try
                    {
                        Tree2 resolvedValue = null;
                        TreeDTO2 mappedValue = null;
                        resolvedValue = source.Tree;
                        mappedValue = (resolvedValue == null) ? (TreeDTO2)null : 
                            context.MapInternal<Tree2, TreeDTO2>(
                                resolvedValue,
                                (destination == null) ? (TreeDTO2)null : 
                                    typeMapDestination.Tree,
                                (MemberMap)default(PropertyMap)/*NOTE: Provide the non-default value for the Constant!*/);
                        typeMapDestination.Tree = mappedValue;
                    }
                    catch (Exception ex)
                    {
                        throw TypeMapPlanBuilder.MemberMappingError(
                            ex,
                            default(PropertyMap)/*NOTE: Provide the non-default value for the Constant!*/);
                    }
                    try
                    {
                        TreeBranch2 resolvedValue_1 = null;
                        TreeBranchDTO2 mappedValue_1 = null;
                        resolvedValue_1 = source.Parent;
                        mappedValue_1 = (resolvedValue_1 == null) ? (TreeBranchDTO2)null : 
                            context.MapInternal<TreeBranch2, TreeBranchDTO2>(
                                resolvedValue_1,
                                (destination == null) ? (TreeBranchDTO2)null : 
                                    typeMapDestination.Parent,
                                (MemberMap)default(PropertyMap)/*NOTE: Provide the non-default value for the Constant!*/);
                        typeMapDestination.Parent = mappedValue_1;
                    }
                    catch (Exception ex)
                    {
                        throw TypeMapPlanBuilder.MemberMappingError(
                            ex,
                            default(PropertyMap)/*NOTE: Provide the non-default value for the Constant!*/);
                    }
                    try
                    {
                        TreeBranch2[] resolvedValue_2 = null;
                        TreeBranchDTO2[] mappedValue_2 = null;
                        resolvedValue_2 = source.Branches;
                        mappedValue_2 = (resolvedValue_2 == null) ? 
                            Array.Empty<TreeBranchDTO2>() : 
                            context.MapInternal<TreeBranch2[], TreeBranchDTO2[]>(
                                resolvedValue_2,
                                (destination == null) ? (TreeBranchDTO2[])null : 
                                    typeMapDestination.Branches,
                                (MemberMap)default(PropertyMap)/*NOTE: Provide the non-default value for the Constant!*/);
                        typeMapDestination.Branches = mappedValue_2;
                    }
                    catch (Exception ex)
                    {
                        throw TypeMapPlanBuilder.MemberMappingError(
                            ex,
                            default(PropertyMap)/*NOTE: Provide the non-default value for the Constant!*/);
                    }
                    try
                    {
                        TreeLeaf2[] resolvedValue_3 = null;
                        TreeLeafDTO2[] mappedValue_3 = null;
                        resolvedValue_3 = source.Leaves;
                        mappedValue_3 = (resolvedValue_3 == null) ? 
                            Array.Empty<TreeLeafDTO2>() : 
                            context.MapInternal<TreeLeaf2[], TreeLeafDTO2[]>(
                                resolvedValue_3,
                                (destination == null) ? (TreeLeafDTO2[])null : 
                                    typeMapDestination.Leaves,
                                (MemberMap)default(PropertyMap)/*NOTE: Provide the non-default value for the Constant!*/);
                        typeMapDestination.Leaves = mappedValue_3;
                    }
                    catch (Exception ex)
                    {
                        throw TypeMapPlanBuilder.MemberMappingError(
                            ex,
                            default(PropertyMap)/*NOTE: Provide the non-default value for the Constant!*/);
                    }
                    try
                    {
                        List<TreeFlower2> resolvedValue_4 = null;
                        List<TreeFlowerDTO2> mappedValue_4 = null;
                        resolvedValue_4 = source.Flowers;
                        mappedValue_4 = (resolvedValue_4 == null) ? 
                            new List<TreeFlowerDTO2>() : 
                            context.MapInternal<List<TreeFlower2>, List<TreeFlowerDTO2>>(
                                resolvedValue_4,
                                (destination == null) ? (List<TreeFlowerDTO2>)null : 
                                    typeMapDestination.Flowers,
                                (MemberMap)default(PropertyMap)/*NOTE: Provide the non-default value for the Constant!*/);
                        typeMapDestination.Flowers = mappedValue_4;
                    }
                    catch (Exception ex)
                    {
                        throw TypeMapPlanBuilder.MemberMappingError(
                            ex,
                            default(PropertyMap)/*NOTE: Provide the non-default value for the Constant!*/);
                    }
                    try
                    {
                        List<TreeFruit2> resolvedValue_5 = null;
                        List<TreeFruitDTO2> mappedValue_5 = null;
                        resolvedValue_5 = source.Fruits;
                        mappedValue_5 = (resolvedValue_5 == null) ? 
                            new List<TreeFruitDTO2>() : 
                            context.MapInternal<List<TreeFruit2>, List<TreeFruitDTO2>>(
                                resolvedValue_5,
                                (destination == null) ? (List<TreeFruitDTO2>)null : 
                                    typeMapDestination.Fruits,
                                (MemberMap)default(PropertyMap)/*NOTE: Provide the non-default value for the Constant!*/);
                        typeMapDestination.Fruits = mappedValue_5;
                    }
                    catch (Exception ex)
                    {
                        throw TypeMapPlanBuilder.MemberMappingError(
                            ex,
                            default(PropertyMap)/*NOTE: Provide the non-default value for the Constant!*/);
                    }
                    return typeMapDestination;
                });
        }));
```