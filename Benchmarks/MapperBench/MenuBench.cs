using AutoMapper;
using AutoMapper.Internal;
using BenchmarkDotNet.Attributes;
using MapperBench.Supports;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;
using PocoEmit;
using PocoEmit.Configuration;

namespace MapperBench;

[MemoryDiagnoser, SimpleJob(launchCount: 2, warmupCount: 10, iterationCount: 10, invocationCount: 10000000)]
public class MenuBench
{
    #region 配置
    private static Menu _menu = Menu.GetMenu();
    private static List<Menu0> _menus = Menu0.GetMenus();
    private AutoMapper.IMapper _auto;
    private AutoMapper.IMapper _auto0;
    private Func<Menu, MenuDTO, ResolutionContext, MenuDTO> _autoFunc;
    private ResolutionContext _resolutionContext;
    private PocoEmit.IMapper _poco;
    private Func<Menu, MenuDTO> _pocoFunc;
    private PocoEmit.IMapper _cache;
    private Func<Menu, MenuDTO> _cacheFunc;
    #endregion

    [Benchmark]
    public MenuDTO Auto()
    {
        return _auto.Map<Menu, MenuDTO>(_menu);
    }
    [Benchmark]
    public List<Menu0DTO> Auto0()
    {
        return _auto0.Map<List<Menu0>, List<Menu0DTO>>(_menus);
    }
    [Benchmark]
    public MenuDTO AutoFunc()
    {
        return _autoFunc(_menu, default(MenuDTO), _resolutionContext);
    }
    [Benchmark(Baseline = true)]
    public MenuDTO Poco()
    {
        return _poco.Convert<Menu, MenuDTO>(_menu);
    }
    [Benchmark]
    public List<Menu0DTO> Poco0()
    {
        return _poco.Convert<List<Menu0>, List<Menu0DTO>>(_menus);
    }
    [Benchmark]
    public MenuDTO PocoFunc()
    {
        return _pocoFunc(_menu);
    }
    [Benchmark]
    public MenuDTO PocoCache()
    {
        return _cache.Convert<Menu, MenuDTO>(_menu);
    }
    [Benchmark]
    public MenuDTO PocoCacheFunc()
    {
        return _cacheFunc(_menu);
    }

    [GlobalSetup]
    public void Setup()
    {
        _auto0 = ConfigureAutoMapper0()
            .CreateMapper();
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
        _poco = PocoEmit.Mapper.Create()
            .UseCollection();
        _pocoFunc = _poco.GetConvertFunc<Menu, MenuDTO>();
        _cache = PocoEmit.Mapper.Create(new MapperOptions { Cached = ComplexCached.Circle })
            .UseCollection();
        _cacheFunc = _cache.GetConvertFunc<Menu, MenuDTO>();
    }
    private static MapperConfiguration ConfigureAutoMapper0()
    {
        return new MapperConfiguration(CreateMap0, LoggerFactory.Create(_ => { }));
    }
    private static MapperConfiguration ConfigureAutoMapper()
    {
        return new MapperConfiguration(CreateMap, LoggerFactory.Create(_ => { }));
    }
    private static void CreateMap0(IMapperConfigurationExpression cfg)
    {
        cfg.CreateMap<Menu0, Menu0DTO>();
    }
    private static void CreateMap(IMapperConfigurationExpression cfg)
    {
        cfg.CreateMap<Menu, MenuDTO>();
    }
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
    public List<Menu0DTO> BuildAuto0()
    {
        LambdaExpression expression = _auto0.ConfigurationProvider.BuildExecutionPlan(typeof(List<Menu0>), typeof(List<Menu0DTO>));
        string code = FastExpressionCompiler.ToCSharpPrinter.ToCSharpString(expression);
        Console.WriteLine(code);
        return Auto0();
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
    public MenuDTO BuildPoco0()
    {
        LambdaExpression expression = _poco.BuildConverter<List<Menu0>, List<Menu0DTO>>();
        string code = FastExpressionCompiler.ToCSharpPrinter.ToCSharpString(expression);
        Console.WriteLine(code);
        return Poco();
    }
    public MenuDTO BuildCache()
    {
        LambdaExpression expression = _cache.BuildConverter<Menu, MenuDTO>();
        string code = FastExpressionCompiler.ToCSharpPrinter.ToCSharpString(expression);
        Console.WriteLine(code);
        LambdaExpression expression2 = _cache.BuildConverter<List<Menu>, List<MenuDTO>>();
        string code2 = FastExpressionCompiler.ToCSharpPrinter.ToCSharpString(expression2);
        Console.WriteLine(code2);
        return PocoCache();
    }
}
