using AutoMapper;
using AutoMapper.Internal;
using BenchmarkDotNet.Attributes;
using MapperBench.Supports;
using Microsoft.Extensions.DependencyInjection;
using PocoEmit;
using System.Linq.Expressions;

namespace MapperBench;

[MemoryDiagnoser, SimpleJob(launchCount: 2, warmupCount: 10, iterationCount: 10, invocationCount: 20000000)]
public class CustomerConvertBench
{
    private ServiceCollection _services = new();
    private AutoMapper.IMapper _auto;
    private PocoEmit.IMapper _poco;
    private PocoEmit.IMapper _frozen;
    private PocoEmit.IPocoConverter<Customer, CustomerDTO> _converter;
    private Func<Customer, CustomerDTO> _convertFunc;
    private Customer _customer = GetCustomer();
    private Func<Customer, CustomerDTO, ResolutionContext, CustomerDTO> _autoFunc;
    private ResolutionContext _resolutionContext;
    public static readonly Func<User, UserDTO> UserDTOConvert = PocoEmit.Mapper.Default.GetConvertFunc<User, UserDTO>();
    /// <summary>
    /// 
    /// </summary>
    public Customer Customer
        => _customer;

    [Benchmark]
    public CustomerDTO Auto()
    {
        
        return _auto.Map<Customer, CustomerDTO>(_customer);
    }
    public void Test()
    {
        //var user2 = _auto.Map<User, UserDTO2>(new User { Id = 1, Name = "a" });
        //var text = _auto.Map<int, string>(1);
        //var num = _auto.Map<string, int>("1");
        ConsoleColor color = ConsoleColor.Red;
        LambdaExpression autoExpression = _auto.ConfigurationProvider.BuildExecutionPlan(typeof(ConsoleColor), typeof(MyColor));
        var autoCode = FastExpressionCompiler.ToCSharpPrinter.ToCSharpString(autoExpression);
        Console.WriteLine(autoCode);
        var autoColor = _auto.Map<ConsoleColor, MyColor>(color);
        Console.WriteLine(autoColor.ToString());
        LambdaExpression pocoExpression = _poco.BuildConverter<ConsoleColor, MyColor>();
        var pocoCode = FastExpressionCompiler.ToCSharpPrinter.ToCSharpString(pocoExpression);
        Console.WriteLine(pocoCode);
        var pocoColor = _poco.Convert<ConsoleColor, MyColor>(color);
        Console.WriteLine(pocoColor.ToString());
    }
    [Benchmark]
    public CustomerDTO AutoFunc()
    {
        return _autoFunc(_customer, default(CustomerDTO), _resolutionContext);
    }
    public string BuildPoco()
    {
        Expression<Func<Customer, CustomerDTO>> expression = _poco.BuildConverter<Customer, CustomerDTO>();
        var code = FastExpressionCompiler.ToCSharpPrinter.ToCSharpString(expression);
        Console.WriteLine(code);
        return code;
    }
    public string BuildAuto()
    {
        LambdaExpression expression = _auto.ConfigurationProvider.BuildExecutionPlan(typeof(Customer), typeof(CustomerDTO));        
        string code = FastExpressionCompiler.ToCSharpPrinter.ToCSharpString(expression);
        Console.WriteLine(code);
        return code;
    }
    [Benchmark(Baseline = true)]
    public CustomerDTO Convert()
    {
        return _poco.Convert<Customer, CustomerDTO>(_customer);
    }
    [Benchmark]
    public CustomerDTO Frozen()
    {
        return _frozen.Convert<Customer, CustomerDTO>(_customer);
    }

    [Benchmark]
    public CustomerDTO Converter()
    {
        return _converter.Convert(_customer);
    }

    [Benchmark]
    public CustomerDTO ConvertFunc()
    {
        return _convertFunc(_customer);
    }


    [GlobalSetup]
    public void Setup()
    {
        ConfigureAutoMapper(_services);
        var serviceProvider = _services.BuildServiceProvider();
        _auto = serviceProvider.GetRequiredService<AutoMapper.IMapper>();     
        {
            var configuration = _auto.ConfigurationProvider.Internal();
            var mapRequest = new MapRequest(new TypePair(typeof(Customer), typeof(CustomerDTO)));
            _autoFunc = configuration.GetExecutionPlan<Customer, CustomerDTO>(mapRequest);
        }
        {
            var field = typeof(AutoMapper.Mapper).GetField("_defaultContext", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            _resolutionContext = field.GetValue(_auto) as ResolutionContext;
        }

        _poco = ConfigurePocoMapper();
        _frozen = ConfigurePocoFrozen();
        _converter = _poco.GetConverter<Customer, CustomerDTO>();
        _convertFunc = _poco.GetConvertFunc<Customer, CustomerDTO>();
    }
    private static PocoEmit.IMapper ConfigurePocoMapper()
    {
        var mapper = PocoEmit.Mapper.Create();
        mapper.UseCollection();
        mapper.ConfigureMap<Customer, CustomerDTO>()
            .UseCheckAction(ConvertAddressCity);
        return mapper;
    }
    private static void ConvertAddressCity(Customer customer, CustomerDTO dto)
    {
        dto.AddressCity = customer.Address.City;
    }
    private static PocoEmit.IMapper ConfigurePocoFrozen()
    {
        var mapper = ConfigurePocoMapper();
        // 预加载,缓存
        mapper.GetConverter<Customer, CustomerDTO>();
        if(mapper is PocoEmit.Mapper configuration)
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
        cfg.CreateMap<Address, Address>();
        cfg.CreateMap<Address, AddressDTO>();
        cfg.CreateMap<Customer, CustomerDTO>();
    }
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    private static Customer GetCustomer()
    {
        return new Customer()
        {
            Address = new Address() { City = "istanbul", Country = "turkey", Id = 1, Street = "istiklal cad." },
            HomeAddress = new Address() { City = "istanbul", Country = "turkey", Id = 2, Street = "istiklal cad." },
            Id = 1,
            Name = "Eduardo Najera",
            Credit = 234.7m,
            WorkAddresses =
                [
                    new Address() {City = "istanbul", Country = "turkey", Id = 5, Street = "istiklal cad."},
                    new Address() {City = "izmir", Country = "turkey", Id = 6, Street = "konak"}
                ],
            Addresses =
                [
                    new Address() {City = "istanbul", Country = "turkey", Id = 3, Street = "istiklal cad."},
                    new Address() {City = "izmir", Country = "turkey", Id = 4, Street = "konak"}
                ]
        };
    }
}
