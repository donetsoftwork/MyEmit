using BenchmarkDotNet.Attributes;
using MapperBench.Supports;
using Microsoft.Extensions.DependencyInjection;
using PocoEmit;
using PocoEmit.Dictionaries;

namespace MapperBench;

[MemoryDiagnoser, SimpleJob(launchCount: 2, warmupCount: 10, iterationCount: 10, invocationCount: 20000000)]
//[MemoryDiagnoser, SimpleJob(launchCount: 2, warmupCount: 10, iterationCount: 10, invocationCount: 100)]
public class ToDictionaryBench
{
    private ServiceCollection _services = new();
    private AutoMapper.IMapper _auto;
    private PocoEmit.IMapper _poco;
    private PocoEmit.IMapper _collection;
    private Func<User, IDictionary<string, object>> _toDictionaryFunc;
    private Func<User, IDictionary<string, object>> _collectionFunc;
    private User _user = new() { Id = 111, Name = "Jxj" };

    //[Benchmark]
    //public IDictionary<string, object> Auto()
    //{
    //    return _auto.Map<User, Dictionary<string, object>>(_user);
    //}
    [Benchmark(Baseline = true)]
    public IDictionary<string, object> ToDictionary()
    {
        return _poco.ToDictionary(_user);
    }
    [Benchmark]
    public IDictionary<string, object> ToDictionaryFunc()
    {
        return _toDictionaryFunc(_user);
    }
    [Benchmark]
    public IDictionary<string, object> Collection()
    {
        return _collection.Convert<User, IDictionary<string, object>>(_user);
    }
    [Benchmark]
    public IDictionary<string, object> CollectionFunc()
    {
        return _collectionFunc(_user);
    }


    [GlobalSetup]
    public void Setup()
    {
        //ConfigureAutoMapper(_services);
        //var serviceProvider = _services.BuildServiceProvider();
        //_auto = serviceProvider.GetRequiredService<AutoMapper.IMapper>();
        _poco = ConfigurePocoMapper();
        _collection = ConfigurCollection();
        _toDictionaryFunc = _poco.GetToDictionaryFunc<User>();
        _collectionFunc = _collection.GetConvertFunc<User, IDictionary<string, object>>();
    }

    private static PocoEmit.IMapper ConfigurePocoMapper()
    {
        var mapper = PocoEmit.Mapper.Create();        
        return mapper;
    }
    private static PocoEmit.IMapper ConfigurCollection()
    {
        var mapper = ConfigurePocoMapper();
        mapper.UseCollection();
        return mapper;
    }
    private static void ConfigureAutoMapper(IServiceCollection services)
    {
        services.AddLogging();
        services.AddAutoMapper(cfg => cfg.CreateMap<User, Dictionary<string, object>>()
            .ForMember(dest => dest["Id"], opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest["Name"], opt => opt.MapFrom(src => src.Name)));
    }
}
