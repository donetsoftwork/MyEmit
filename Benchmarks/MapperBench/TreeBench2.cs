using AutoMapper;
using AutoMapper.Internal;
using BenchmarkDotNet.Attributes;
using MapperBench.Supports;
using Microsoft.Extensions.Logging;
using PocoEmit;
using PocoEmit.Configuration;
using System.Linq.Expressions;


namespace MapperBench;

[MemoryDiagnoser, SimpleJob(launchCount: 2, warmupCount: 10, iterationCount: 10, invocationCount: 500000)]
public class TreeBench2
{
    private AutoMapper.IMapper _auto;
    private PocoEmit.IMapper _poco;
    private PocoEmit.IMapper _invoke;
    private Func<Tree2, TreeDTO2> _pocoFunc;
    private Func<Tree2, TreeDTO2> _invokeFunc;
    private Func<Tree2, TreeDTO2, ResolutionContext, TreeDTO2> _autoFunc;
    private ResolutionContext _resolutionContext;
    private static Tree2 _tree = GetTree();

    [Benchmark]
    public TreeDTO2 Auto2()
    {
        return _auto.Map<Tree2, TreeDTO2>(_tree);
    }
    [Benchmark]
    public TreeDTO2 AutoFunc2()
    {
        return _autoFunc(_tree, default(TreeDTO2), _resolutionContext);
    }
    [Benchmark(Baseline = true)]
    public TreeDTO2 Poco2()
    {
        return _poco.Convert<Tree2, TreeDTO2>(_tree);
    }
    [Benchmark]
    public TreeDTO2 PocoFunc2()
    {
        return _pocoFunc(_tree);
    }

    [Benchmark]
    public TreeDTO2 Invoke2()
    {
        return _invoke.Convert<Tree2, TreeDTO2>(_tree);
    }
    [Benchmark]
    public TreeDTO2 InvokeFunc2()
    {
        return _invokeFunc(_tree);
    }

    public TreeDTO2 BuildAuto()
    {
        LambdaExpression expression = _auto.ConfigurationProvider.BuildExecutionPlan(typeof(Tree2), typeof(TreeDTO2));
        string code = FastExpressionCompiler.ToCSharpPrinter.ToCSharpString(expression);
        Console.WriteLine(code);
        LambdaExpression expressionRoot = _auto.ConfigurationProvider.BuildExecutionPlan(typeof(List<TreeRoot2>), typeof(List<TreeRootDTO2>));
        string codeRoot = FastExpressionCompiler.ToCSharpPrinter.ToCSharpString(expressionRoot);
        Console.WriteLine(codeRoot);
        LambdaExpression expressionBranch = _auto.ConfigurationProvider.BuildExecutionPlan(typeof(TreeBranch2), typeof(TreeBranchDTO2));
        string codeBranch = FastExpressionCompiler.ToCSharpPrinter.ToCSharpString(expressionBranch);
        Console.WriteLine(codeBranch);
        return Auto2();
    }
    public TreeDTO2 BuildPoco()
    {
        LambdaExpression expression = _poco.BuildConverter<Tree2, TreeDTO2>();
        string code = FastExpressionCompiler.ToCSharpPrinter.ToCSharpString(expression);
        Console.WriteLine(code);
        //for (int i = 0; i < 100; i++)
        //{
        //    Poco2();
        //}
        return Poco2();
    }

    public TreeDTO2 BuildInvoke()
    {
        LambdaExpression expression = _invoke.BuildConverter<Tree2, TreeDTO2>();
        string code = FastExpressionCompiler.ToCSharpPrinter.ToCSharpString(expression);
        Console.WriteLine(code);
        return Invoke2();
    }

    [GlobalSetup]
    public void Setup()
    {
        _auto = ConfigureAutoMapper()
            .CreateMapper();
        {
            var configuration = _auto.ConfigurationProvider.Internal();
            var mapRequest = new MapRequest(new TypePair(typeof(Tree2), typeof(TreeDTO2)));
            _autoFunc = configuration.GetExecutionPlan<Tree2, TreeDTO2>(mapRequest);
        }
        {
            var field = typeof(AutoMapper.Mapper).GetField("_defaultContext", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            _resolutionContext = field.GetValue(_auto) as ResolutionContext;
        }

        _poco = ConfigurePocoMapper(new MapperOptions());
        _invoke = ConfigurePocoMapper(new MapperOptions() { LambdaInvoke = true });
        _pocoFunc = _poco.GetConvertFunc<Tree2, TreeDTO2>();
        _invokeFunc = _invoke.GetConvertFunc<Tree2, TreeDTO2>();
    }
    private static PocoEmit.IMapper ConfigurePocoMapper(MapperOptions options)
    {
        var mapper = PocoEmit.Mapper.Create(options);
        mapper.UseCollection();
        return mapper;
    }
    private static MapperConfiguration ConfigureAutoMapper()
    {
        return new MapperConfiguration(CreateMap, LoggerFactory.Create(_ => { }));
    }
    private static void CreateMap(IMapperConfigurationExpression cfg)
    {
        cfg.CreateMap<TreeLeaf2, TreeLeafDTO2>();
        cfg.CreateMap<TreeFlower2, TreeFlowerDTO2>();
        cfg.CreateMap<TreeFruit2, TreeFruitDTO2>();
        cfg.CreateMap<TreeBranch2, TreeBranchDTO2>();
        cfg.CreateMap<TreeRoot2, TreeRootDTO2>();
        cfg.CreateMap<Tree2, TreeDTO2>();
    }
    private static Tree2 GetTree()
    {
        var tree = new Tree2 { Id = 1 };
        #region root1
        var root1 = new TreeRoot2 { Id = 1, Tree = tree };
        var root11 = new TreeRoot2 { Id = 11, Tree = tree, Parent = root1 };
        var root12 = new TreeRoot2 { Id = 12, Tree = tree, Parent = root1 };
        root1.Roots = [root11, root12];
        #endregion
        #region root2
        var root2 = new TreeRoot2 { Id = 2, Tree = tree };
        var root21 = new TreeRoot2 { Id = 21, Tree = tree, Parent = root2 };
        var root22 = new TreeRoot2 { Id = 22, Tree = tree, Parent = root2 };
        root2.Roots = [root21, root22];
        #endregion
        #region trunk
        var trunk = new TreeBranch2 { Id = 11, Tree = tree };
        #region branch1
        var branch1 = new TreeBranch2 { Id = 12, Tree = tree, Parent = trunk };
        var leaf11 = new TreeLeaf2 { Id = 111, Branch = branch1 };
        var leaf12 = new TreeLeaf2 { Id = 112, Branch = branch1 };
        var flower11 = new TreeFlower2 { Id = 111, Branch = branch1 };
        var fruit11 = new TreeFruit2 { Id = 111, Branch = branch1 };
        branch1.Leaves = [leaf11, leaf12];
        branch1.Flowers = [flower11];
        branch1.Fruits = [fruit11];
        #endregion
        #region branch2
        var branch2 = new TreeBranch2 { Id = 13, Tree = tree, Parent = trunk };
        var leaf21 = new TreeLeaf2 { Id = 121, Branch = branch2 };
        var leaf22 = new TreeLeaf2 { Id = 122, Branch = branch2 };
        var flower21 = new TreeFlower2 { Id = 121, Branch = branch2 };
        var fruit21 = new TreeFruit2 { Id = 121, Branch = branch2 };
        branch2.Leaves = [leaf21, leaf22];
        branch2.Flowers = [flower21];
        branch2.Fruits = [fruit21];
        #endregion
        trunk.Branches = [branch1, branch2];
        #endregion

        tree.Roots = [root1, root2];
        tree.Trunk = trunk;
        return tree;
    }
}
