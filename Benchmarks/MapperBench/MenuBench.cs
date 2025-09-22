using AutoMapper;
using AutoMapper.Internal;
using BenchmarkDotNet.Attributes;
using MapperBench.Supports;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;
using PocoEmit;

namespace MapperBench;

[MemoryDiagnoser, SimpleJob(launchCount: 2, warmupCount: 10, iterationCount: 10, invocationCount: 10000000)]
public class MenuBench
{
    #region 配置
    private static Menu _node = Menu.GetMenu();
    private AutoMapper.IMapper _auto;
    private Func<Menu, MenuDTO, ResolutionContext, MenuDTO> _autoFunc;
    private ResolutionContext _resolutionContext;
    private PocoEmit.IMapper _poco;
    private Func<Menu, MenuDTO> _pocoFunc;
    #endregion

    [Benchmark]
    public MenuDTO Auto()
    {
        return _auto.Map<Menu, MenuDTO>(_node);
    }
    [Benchmark]
    public MenuDTO AutoFunc()
    {
        return _autoFunc(_node, default(MenuDTO), _resolutionContext);
    }
    [Benchmark(Baseline = true)]
    public MenuDTO Poco()
    {
        return _poco.Convert<Menu, MenuDTO>(_node);
    }
    [Benchmark]
    public MenuDTO PocoFunc()
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
            var mapRequest = new MapRequest(new TypePair(typeof(Menu), typeof(MenuDTO)));
            _autoFunc = configuration.GetExecutionPlan<Menu, MenuDTO>(mapRequest);
        }
        {
            var field = typeof(AutoMapper.Mapper).GetField("_defaultContext", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            _resolutionContext = field.GetValue(_auto) as ResolutionContext;
        }
        _poco = PocoEmit.Mapper.Create();
        _poco.UseCollection();
        _pocoFunc = _poco.GetConvertFunc<Menu, MenuDTO>();
    }
    private static MapperConfiguration ConfigureAutoMapper()
    {
        return new MapperConfiguration(CreateMap, LoggerFactory.Create(_ => { }));
    }
    private static void CreateMap(IMapperConfigurationExpression cfg)
    {
        cfg.CreateMap<Menu, MenuDTO>();
    }
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public MenuDTO BuildAuto()
    {
        LambdaExpression expression = _auto.ConfigurationProvider.BuildExecutionPlan(typeof(Menu), typeof(MenuDTO));
        //expression.Body.GetMembers()
        string code = FastExpressionCompiler.ToCSharpPrinter.ToCSharpString(expression);
        Console.WriteLine(code);
        LambdaExpression expression2 = _auto.ConfigurationProvider.BuildExecutionPlan(typeof(List<Menu>), typeof(List<MenuDTO>));
        string code2 = FastExpressionCompiler.ToCSharpPrinter.ToCSharpString(expression2);
        Console.WriteLine(code2);
        return Auto();
    }
    public MenuDTO BuildPoco()
    {
        LambdaExpression expression = _poco.BuildConverter<Menu, MenuDTO>();
        string code = FastExpressionCompiler.ToCSharpPrinter.ToCSharpString(expression);
        Console.WriteLine(code);
        LambdaExpression expression2 = _poco.BuildConverter<List<Menu>, List<MenuDTO>>();
        string code2 = FastExpressionCompiler.ToCSharpPrinter.ToCSharpString(expression2);
        Console.WriteLine(code2);
        return Poco();
    }
}
