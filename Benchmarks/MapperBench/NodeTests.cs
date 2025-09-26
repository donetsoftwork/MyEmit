using AutoMapper;
using AutoMapper.Internal;
using BenchmarkDotNet.Attributes;
using MapperBench.Supports;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;
using PocoEmit;
using PocoEmit.Configuration;

namespace MapperBench;

[MemoryDiagnoser, SimpleJob(launchCount: 2, warmupCount: 10, iterationCount: 10, invocationCount: 5000000)]
public class NodeBench
{
    #region 配置
    private static Node _node = Node.GetNode();
    private AutoMapper.IMapper _auto;
    private Func<Node, NodeDTO, ResolutionContext, NodeDTO> _autoFunc;
    private ResolutionContext _resolutionContext;
    private PocoEmit.IMapper _poco;
    private Func<Node, NodeDTO> _pocoFunc;
    #endregion

    [Benchmark]
    public NodeDTO Auto()
    {
        return _auto.Map<Node, NodeDTO>(_node);
    }
    [Benchmark]
    public NodeDTO AutoFunc()
    {
        return _autoFunc(_node, default(NodeDTO), _resolutionContext);
    }
    [Benchmark(Baseline = true)]
    public NodeDTO Poco()
    {
        return _poco.Convert<Node, NodeDTO>(_node);
    }
    [Benchmark]
    public NodeDTO PocoFunc()
    {
        return _pocoFunc(_node);
    }

    [GlobalSetup]
    public void Setup()
    {
        _auto = ConfigureAutoMapper()
            .CreateMapper();
        {
            var configuration = _auto.ConfigurationProvider.Internal();
            var mapRequest = new MapRequest(new TypePair(typeof(Node), typeof(NodeDTO)));
            _autoFunc = configuration.GetExecutionPlan<Node, NodeDTO>(mapRequest);
        }
        {
            var field = typeof(AutoMapper.Mapper).GetField("_defaultContext", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            _resolutionContext = field.GetValue(_auto) as ResolutionContext;
        }
        _poco = PocoEmit.Mapper.Create(new MapperOptions { Cached = ComplexCached.Circle });
        _pocoFunc = _poco.GetConvertFunc<Node, NodeDTO>();
    }
    private static MapperConfiguration ConfigureAutoMapper()
    {
        return new MapperConfiguration(CreateMap, LoggerFactory.Create(_ => { }));
    }
    private static void CreateMap(IMapperConfigurationExpression cfg)
    {
        cfg.CreateMap<Node, NodeDTO>();
    }
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public NodeDTO BuildAuto()
    {
        LambdaExpression expression = _auto.ConfigurationProvider.BuildExecutionPlan(typeof(Node), typeof(NodeDTO));
        string code = FastExpressionCompiler.ToCSharpPrinter.ToCSharpString(expression);
        Console.WriteLine(code);
        return Auto();
    }
    public NodeDTO BuildPoco()
    {
        LambdaExpression expression = _poco.BuildConverter<Node, NodeDTO>();
        string code = FastExpressionCompiler.ToCSharpPrinter.ToCSharpString(expression);
        Console.WriteLine(code);
        return Poco();
    }
}
