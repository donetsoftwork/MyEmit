using AutoMapper;
using AutoMapper.Internal;
using BenchmarkDotNet.Attributes;
using MapperBench.Supports;
using Microsoft.Extensions.Logging;
using PocoEmit;
using PocoEmit.Configuration;
using System.Linq.Expressions;


namespace MapperBench;

[MemoryDiagnoser, SimpleJob(launchCount: 2, warmupCount: 10, iterationCount: 10, invocationCount: 5000000)]
public class TreeBench
{
    private AutoMapper.IMapper _auto;
    private PocoEmit.IMapper _poco;
    private PocoEmit.IMapper _invoke;
    private Func<Tree, TreeDTO> _pocoFunc;
    private Func<Tree, TreeDTO> _invokeFunc;
    private Func<Tree, TreeDTO, ResolutionContext, TreeDTO> _autoFunc;
    private ResolutionContext _resolutionContext;
    private static Tree _tree = GetTree();

    [Benchmark]
    public TreeDTO Auto()
    {
        return _auto.Map<Tree, TreeDTO>(_tree);
    }
    [Benchmark]
    public TreeDTO AutoFunc()
    {
        return _autoFunc(_tree, default(TreeDTO), _resolutionContext);
    }
    [Benchmark(Baseline = true)]
    public TreeDTO Poco()
    {
        return _poco.Convert<Tree, TreeDTO>(_tree);
    }
    [Benchmark]
    public TreeDTO PocoFunc()
    {
        return _pocoFunc(_tree);
    }

    [Benchmark]
    public TreeDTO Invoke()
    {
        return _invoke.Convert<Tree, TreeDTO>(_tree);
    }
    [Benchmark]
    public TreeDTO InvokeFunc()
    {
        return _invokeFunc(_tree);
    }

    public TreeDTO BuildAuto()
    {
        LambdaExpression expression = _auto.ConfigurationProvider.BuildExecutionPlan(typeof(Tree), typeof(TreeDTO));
        string code = FastExpressionCompiler.ToCSharpPrinter.ToCSharpString(expression);
        Console.WriteLine(code);
        LambdaExpression expressionRoot = _auto.ConfigurationProvider.BuildExecutionPlan(typeof(List<TreeRoot>), typeof(List<TreeRootDTO>));
        string codeRoot = FastExpressionCompiler.ToCSharpPrinter.ToCSharpString(expressionRoot);
        Console.WriteLine(codeRoot);
        LambdaExpression expressionBranch = _auto.ConfigurationProvider.BuildExecutionPlan(typeof(TreeBranch), typeof(TreeBranchDTO));
        string codeBranch = FastExpressionCompiler.ToCSharpPrinter.ToCSharpString(expressionBranch);
        Console.WriteLine(codeBranch);
        return Auto();
    }
    public TreeDTO BuildPoco()
    {
        LambdaExpression expression = _poco.BuildConverter<Tree, TreeDTO>();
        string code = FastExpressionCompiler.ToCSharpPrinter.ToCSharpString(expression);
        Console.WriteLine(code);
        return Poco();
    }

    public TreeDTO BuildInvoke()
    {
        LambdaExpression expression = _invoke.BuildConverter<Tree, TreeDTO>();
        string code = FastExpressionCompiler.ToCSharpPrinter.ToCSharpString(expression);
        Console.WriteLine(code);
        return Invoke();
    }

    [GlobalSetup]
    public void Setup()
    {
        _auto = ConfigureAutoMapper()
            .CreateMapper();
        {
            var configuration = _auto.ConfigurationProvider.Internal();
            var mapRequest = new MapRequest(new TypePair(typeof(Tree), typeof(TreeDTO)));
            _autoFunc = configuration.GetExecutionPlan<Tree, TreeDTO>(mapRequest);
        }
        {
            var field = typeof(AutoMapper.Mapper).GetField("_defaultContext", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            _resolutionContext = field.GetValue(_auto) as ResolutionContext;
        }

        _poco = ConfigurePocoMapper(new MapperOptions());
        _invoke = ConfigurePocoMapper(new MapperOptions() { LambdaInvoke = true });
        _pocoFunc = _poco.GetConvertFunc<Tree, TreeDTO>();
        _invokeFunc = _invoke.GetConvertFunc<Tree, TreeDTO>();
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
        cfg.CreateMap<Leaf, LeafDTO>();
        cfg.CreateMap<Flower, FlowerDTO>();
        cfg.CreateMap<Fruit, FruitDTO>();
        cfg.CreateMap<TreeBranch, TreeBranchDTO>();
        cfg.CreateMap<TreeRoot, TreeRootDTO>();
        cfg.CreateMap<Tree, TreeDTO>();
    }
    private static Tree GetTree()
    {
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
        return tree;
    }
}
