using AutoMapper;
using AutoMapper.Internal;
using BenchmarkDotNet.Attributes;
using MapperBench.Supports;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;
using PocoEmit;
using PocoEmit.Configuration;
using PocoEmit.Resolves;

namespace MapperBench;

[MemoryDiagnoser, SimpleJob(launchCount: 2, warmupCount: 10, iterationCount: 10, invocationCount: 10000000)]
public class SoldierTeamBench
{
    #region 配置
    private static SoldierTeam _team = SoldierTeam.GetTeam();
    private AutoMapper.IMapper _auto;
    private Func<SoldierTeam, SoldierTeamDTO, ResolutionContext, SoldierTeamDTO> _autoFunc;
    private ResolutionContext _resolutionContext;
    private PocoEmit.IMapper _poco;
    private Func<SoldierTeam, SoldierTeamDTO> _pocoFunc;
    private PocoEmit.IMapper _cache;
    private Func<SoldierTeam, SoldierTeamDTO> _cacheFunc;
    private Func<IConvertContext, SoldierTeam, SoldierTeamDTO> _pocoContextFunc;
    #endregion

    [Benchmark]
    public SoldierTeamDTO Auto()
    {
        return _auto.Map<SoldierTeam, SoldierTeamDTO>(_team);
    }
    [Benchmark]
    public SoldierTeamDTO AutoFunc()
    {
        return _autoFunc(_team, default(SoldierTeamDTO), _resolutionContext);
    }
    //[Benchmark(Baseline = true)]
    //public SoldierTeamDTO Poco()
    //{
    //    return _poco.Convert<SoldierTeam, SoldierTeamDTO>(_team);
    //}
    //[Benchmark]
    //public SoldierTeamDTO PocoFunc()
    //{
    //    return _pocoFunc(_team);
    //}
    [Benchmark(Baseline = true)]
    public SoldierTeamDTO Poco()
    {
        return _cache.Convert<SoldierTeam, SoldierTeamDTO>(_team);
    }
    [Benchmark]
    public SoldierTeamDTO PocoFunc()
    {
        return _cacheFunc(_team);
    }
    [Benchmark]
    public SoldierTeamDTO PocoContextFunc()
    {
        using var context = SingleContext<Soldier, SoldierDTO>.Pool.Get();
        return _pocoContextFunc(context, _team);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public SoldierTeamDTO BuildAuto()
    {
        LambdaExpression expression = _auto.ConfigurationProvider.BuildExecutionPlan(typeof(SoldierTeam), typeof(SoldierTeamDTO));
        //expression.Body.GetMembers()
        string code = FastExpressionCompiler.ToCSharpPrinter.ToCSharpString(expression);
        Console.WriteLine(code);
        LambdaExpression expression2 = _auto.ConfigurationProvider.BuildExecutionPlan(typeof(List<Soldier>), typeof(List<SoldierDTO>));
        string code2 = FastExpressionCompiler.ToCSharpPrinter.ToCSharpString(expression2);
        Console.WriteLine(code2);
        var auto = Auto();
        var dtoList = auto.Members.Concat([auto.Leader, auto.Courier]).Distinct().ToArray();
        Console.WriteLine($"DTO Count:{dtoList.Length}");
        return auto;
    }
    //public SoldierTeamDTO BuildPoco()
    //{
    //    LambdaExpression expression = _poco.BuildConverter<SoldierTeam, SoldierTeamDTO>();
    //    string code = FastExpressionCompiler.ToCSharpPrinter.ToCSharpString(expression);
    //    Console.WriteLine(code);
    //    LambdaExpression expression2 = _poco.BuildConverter<List<Soldier>, List<SoldierDTO>>();
    //    string code2 = FastExpressionCompiler.ToCSharpPrinter.ToCSharpString(expression2);
    //    Console.WriteLine(code2);
    //    var poco = Poco();
    //    var dtoList = poco.Members.Concat([poco.Leader, poco.Courier]).Distinct().ToArray();
    //    Console.WriteLine($"DTO Count:{dtoList.Length}");
    //    return poco;
    //}
    public SoldierTeamDTO BuildCache()
    {
        LambdaExpression expression = _cache.BuildConverter<SoldierTeam, SoldierTeamDTO>();
        string code = FastExpressionCompiler.ToCSharpPrinter.ToCSharpString(expression);
        Console.WriteLine(code);
        //LambdaExpression expression2 = _cache.BuildConverter<List<Soldier>, List<SoldierDTO>>();
        //string code2 = FastExpressionCompiler.ToCSharpPrinter.ToCSharpString(expression2);
        //Console.WriteLine(code2);
        var list = _team.Members.Concat([_team.Leader, _team.Courier]).Distinct().ToArray();
        var cache = Poco();
        var dtoList = cache.Members.Concat([cache.Leader, cache.Courier]).Distinct().ToArray();
        Console.WriteLine($"DTO Count:{dtoList.Length}");
        return cache;
    }

    public SoldierTeamDTO BuildContextFunc()
    {
        var converter =  _poco.GetEmitContextConverter<SoldierTeam, SoldierTeamDTO>();
        LambdaExpression expression = converter.Create();
        string code = FastExpressionCompiler.ToCSharpPrinter.ToCSharpString(expression);
        Console.WriteLine(code);
        //LambdaExpression expression2 = _cache.BuildConverter<List<Soldier>, List<SoldierDTO>>();
        //string code2 = FastExpressionCompiler.ToCSharpPrinter.ToCSharpString(expression2);
        //Console.WriteLine(code2);
        var list = _team.Members.Concat([_team.Leader, _team.Courier]).Distinct().ToArray();
        var context = PocoContextFunc();
        var dtoList = context.Members.Concat([context.Leader, context.Courier]).Distinct().ToArray();
        Console.WriteLine($"Team Count:{list.Length},DTO Count:{dtoList.Length}");
        return context;
    }

    [GlobalSetup]
    public void Setup()
    {
        _auto = ConfigureAutoMapper()
            .CreateMapper();
        {
            var configuration = _auto.ConfigurationProvider.Internal();
            var mapRequest = new MapRequest(new TypePair(typeof(SoldierTeam), typeof(SoldierTeamDTO)));
            _autoFunc = configuration.GetExecutionPlan<SoldierTeam, SoldierTeamDTO>(mapRequest);
        }
        {
            var field = typeof(AutoMapper.Mapper).GetField("_defaultContext", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            _resolutionContext = field.GetValue(_auto) as ResolutionContext;
        }
        _poco = PocoEmit.Mapper.Create();
        _poco.UseCollection();
        _pocoFunc = _poco.GetConvertFunc<SoldierTeam, SoldierTeamDTO>();
        _cache = PocoEmit.Mapper.Create(new MapperOptions { Cached = ComplexCached.Always })
            .UseCollection();
        _cacheFunc = _cache.GetConvertFunc<SoldierTeam, SoldierTeamDTO>();
        _pocoContextFunc = _poco.GetContextConvertFunc<SoldierTeam, SoldierTeamDTO>();
    }
    private static MapperConfiguration ConfigureAutoMapper()
    {
        return new MapperConfiguration(CreateMap, LoggerFactory.Create(_ => { }));
    }
    private static void CreateMap(IMapperConfigurationExpression cfg)
    {
        cfg.CreateMap<Soldier, SoldierDTO>().PreserveReferences() ;
        cfg.CreateMap<SoldierTeam, SoldierTeamDTO>().PreserveReferences();
    }
}
