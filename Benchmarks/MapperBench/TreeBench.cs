using AutoMapper;
using AutoMapper.Internal;
using BenchmarkDotNet.Attributes;
using MapperBench.Supports;
using Microsoft.Extensions.DependencyInjection;
using PocoEmit;
using PocoEmit.Configuration;
using System.Linq.Expressions;


namespace MapperBench;

[MemoryDiagnoser, SimpleJob(launchCount: 2, warmupCount: 10, iterationCount: 10, invocationCount: 20000000)]
public class TreeBench
{
    private ServiceCollection _services = new();
    private AutoMapper.IMapper _auto;
    private PocoEmit.IMapper _poco;
    private PocoEmit.IMapper _visit;
    private Func<TreeBranch, TreeBranchDTO> _pocoFunc;
    private Func<TreeBranch, TreeBranchDTO> _visitFunc;
    private Func<TreeBranch, TreeBranchDTO, ResolutionContext, TreeBranchDTO> _autoFunc;
    private ResolutionContext _resolutionContext;
    private static TreeBranch _treeBranch = GetTreeBranch();

    [Benchmark]
    public TreeBranchDTO Auto()
    {
        return _auto.Map<TreeBranch, TreeBranchDTO>(_treeBranch);
    }
    [Benchmark]
    public TreeBranchDTO AutoFunc()
    {
        return _autoFunc(_treeBranch, default(TreeBranchDTO), _resolutionContext);
    }
    [Benchmark(Baseline = true)]
    public TreeBranchDTO Poco()
    {
        return _poco.Convert<TreeBranch, TreeBranchDTO>(_treeBranch);
    }
    [Benchmark]
    public TreeBranchDTO PocoFunc()
    {
        return _pocoFunc(_treeBranch);
    }

    //[Benchmark]
    //public TreeBranchDTO Visit()
    //{
    //    return _visit.Convert<TreeBranch, TreeBranchDTO>(_treeBranch);
    //}
    //[Benchmark]
    //public TreeBranchDTO VisitFunc()
    //{
    //    return _visitFunc(_treeBranch);
    //}

    public TreeBranchDTO BuildAuto()
    {
        LambdaExpression expression = _auto.ConfigurationProvider.BuildExecutionPlan(typeof(TreeBranch), typeof(TreeBranchDTO));
        string code = FastExpressionCompiler.ToCSharpPrinter.ToCSharpString(expression);
        Console.WriteLine(code);
        return Auto();
    }
    public TreeBranchDTO BuildPoco()
    {
        LambdaExpression expression = _poco.BuildConverter<TreeBranch, TreeBranchDTO>();
        string code = FastExpressionCompiler.ToCSharpPrinter.ToCSharpString(expression);
        Console.WriteLine(code);
        return Poco();
    }

    [GlobalSetup]
    public void Setup()
    {
        ConfigureAutoMapper(_services);
        var serviceProvider = _services.BuildServiceProvider();
        _auto = serviceProvider.GetRequiredService<AutoMapper.IMapper>();
        {
            var configuration = _auto.ConfigurationProvider.Internal();
            var mapRequest = new MapRequest(new TypePair(typeof(TreeBranch), typeof(TreeBranchDTO)));
            _autoFunc = configuration.GetExecutionPlan<TreeBranch, TreeBranchDTO>(mapRequest);
        }
        {
            var field = typeof(AutoMapper.Mapper).GetField("_defaultContext", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            _resolutionContext = field.GetValue(_auto) as ResolutionContext;
        }

        _poco = ConfigurePocoMapper(new MapperOptions());
        _visit = ConfigurePocoMapper(new MapperOptions() { LambdaInvoke = false });
        _pocoFunc = _poco.GetConvertFunc<TreeBranch, TreeBranchDTO>();
        //_visitFunc = _visit.GetConvertFunc<TreeBranch, TreeBranchDTO>();
    }
    private static PocoEmit.IMapper ConfigurePocoMapper(MapperOptions options)
    {
        var mapper = PocoEmit.Mapper.Create(options);
        mapper.UseCollection();
        return mapper;
    }
    private static void ConfigureAutoMapper(ServiceCollection services)
    {
        services.AddLogging();
        services.AddAutoMapper(CreateMap);
    }
    private static void CreateMap(IMapperConfigurationExpression cfg)
    {
        cfg.CreateMap<TreeLeaf, TreeLeafDTO>();
        cfg.CreateMap<TreeBranch, TreeBranchDTO>();
    }
    private static TreeBranch GetTreeBranch()
    {
        var tree = new TreeBranch { Id = 0 };
        var branch1 = new TreeBranch { Id = 1 };
        var branch2 = new TreeBranch { Id = 2 };
        tree.Branches = [branch1, branch2];
        var leaf11 = new TreeLeaf { Id = 11 };
        var leaf12 = new TreeLeaf { Id = 12 };
        branch1.Leaves = [leaf11, leaf12];
        var leaf21 = new TreeLeaf { Id = 21 };
        var leaf22 = new TreeLeaf { Id = 22 };
        branch2.Leaves = [leaf21, leaf22];
        return tree;
    }
}
