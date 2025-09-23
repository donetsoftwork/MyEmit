using AutoMapper;
using AutoMapper.Internal;
using BenchmarkDotNet.Attributes;
using MapperBench.Supports;
using Microsoft.Extensions.Logging;
using PocoEmit;
using PocoEmit.Dictionaries;

namespace MapperBench;

[MemoryDiagnoser, SimpleJob(launchCount: 2, warmupCount: 10, iterationCount: 10, invocationCount: 20000000)]
public class UserConvertBench
{
    private AutoMapper.IMapper _auto;
    private Func<User, UserDTO, ResolutionContext, UserDTO> _autoFunc;
    private ResolutionContext _resolutionContext;
    private PocoEmit.IMapper _poco;
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
        var expression = _poco.BuildDictionaryConverter<User, UserDTO>();
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
    public UserDTO Poco()
    {
        return _poco.Convert<User, UserDTO>(_user);
    }
    [Benchmark]
    public UserDTO Converter()
    {
        return _converter.Convert(_user);
    }

    [Benchmark]
    public UserDTO PocoFunc()
    {
        return _convertFunc(_user);
    }

    [GlobalSetup]
    public void Setup()
    {
        _auto = ConfigureAutoMapper()
            .CreateMapper();
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
        _converter = _poco.GetConverter<User, UserDTO>();
        _convertFunc = _poco.GetConvertFunc<User, UserDTO>();
        //UserDTO dto = PocoEmit.Mapper.Default.Convert<User, UserDTO>(new User());
    }
    private static PocoEmit.IMapper ConfigurePocoMapper()
    {
        var mapper = PocoEmit.Mapper.Create();
        mapper.UseCollection();
        return mapper;
    }
    private static MapperConfiguration ConfigureAutoMapper()
    {
        return new MapperConfiguration(CreateMap, LoggerFactory.Create(_ => { }));
    }

    private static void CreateMap(IMapperConfigurationExpression cfg)
    {
        cfg.CreateMap<User, UserDTO>();
    }    
}
