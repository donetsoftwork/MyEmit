using AutoMapper;
using AutoMapper.Internal;
using BenchmarkDotNet.Attributes;
using MapperBench.Supports;
using Microsoft.Extensions.DependencyInjection;
using PocoEmit;

namespace MapperBench;

[MemoryDiagnoser, SimpleJob(launchCount: 2, warmupCount: 10, iterationCount: 10, invocationCount: 20000000)]
public class UserConvertBench
{
    private ServiceCollection _services = new();
    private AutoMapper.IMapper _auto;
    private Func<User, UserDTO, ResolutionContext, UserDTO> _autoFunc;
    private ResolutionContext _resolutionContext;
    private PocoEmit.IMapper _poco;
    private PocoEmit.IMapper _frozen;
    private PocoEmit.IPocoConverter<User, UserDTO> _converter;
    private Func<User, UserDTO> _convertFunc;
    private User _user = new() {  Id =111, Name = "Jxj" };
    /// <summary>
    /// 
    /// </summary>
    public User User
        => _user;

    [Benchmark]
    public UserDTO Auto()
    {
        return _auto.Map<User, UserDTO>(_user);
    }
    [Benchmark]
    public UserDTO AutoFunc()
    {
        return _autoFunc.Invoke(_user, default(UserDTO), _resolutionContext);
    }
    public string BuildPoco()
    {
        var expression = _poco.BuildConverter<User, UserDTO>();
        var code = FastExpressionCompiler.ToCSharpPrinter.ToCSharpString(expression);
        Console.WriteLine(code);
        return code;
    }
    public string BuildAuto()
    {
        var expression = _auto.ConfigurationProvider.BuildExecutionPlan(typeof(User), typeof(UserDTO));
        var code = FastExpressionCompiler.ToCSharpPrinter.ToCSharpString(expression);
        Console.WriteLine(code);
        return code;
    }
    [Benchmark(Baseline = true)]
    public UserDTO Convert()
    {
        return _poco.Convert<User, UserDTO>(_user);
    }
    [Benchmark]
    public UserDTO Frozen()
    {
        return _frozen.Convert<User, UserDTO>(_user);
    }

    [Benchmark]
    public UserDTO Converter()
    {
        return _converter.Convert(_user);
    }

    [Benchmark]
    public UserDTO ConvertFunc()
    {
        return _convertFunc(_user);
    }

    [GlobalSetup]
    public void Setup()
    {
        ConfigureAutoMapper(_services);
        var serviceProvider = _services.BuildServiceProvider();
        _auto = serviceProvider.GetRequiredService<AutoMapper.IMapper>();
        {
            var configuration = _auto.ConfigurationProvider.Internal();
            var mapRequest = new MapRequest(new TypePair(typeof(User), typeof(UserDTO)));
            _autoFunc = configuration.GetExecutionPlan<User, UserDTO>(mapRequest);
        }
        {
            var field = typeof(AutoMapper.Mapper).GetField("_defaultContext", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            _resolutionContext = field.GetValue(_auto) as ResolutionContext;
        }
        _poco = ConfigurePocoMapper();
        _frozen = ConfigurePocoFrozen();
        _converter = _poco.GetConverter<User, UserDTO>();
        _convertFunc = _poco.GetConvertFunc<User, UserDTO>();
    }
    private static PocoEmit.IMapper ConfigurePocoMapper()
    {
        var mapper = PocoEmit.Mapper.Create();
        mapper.UseCollection();
        return mapper;
    }
    private static PocoEmit.IMapper ConfigurePocoFrozen()
    {
        var mapper = ConfigurePocoMapper();
        // 预加载,缓存
        mapper.GetConverter<User, UserDTO>();
        if (mapper is PocoEmit.Mapper configuration)
            configuration.ToFrozen();
        return mapper;
    }
    private static void ConfigureAutoMapper(ServiceCollection services)
    {
        services.AddLogging();
        services.AddAutoMapper(CreateMap);
    }
    private static void CreateMap(IMapperConfigurationExpression cfg)
    {
        cfg.CreateMap<User, UserDTO>();
    }
}
