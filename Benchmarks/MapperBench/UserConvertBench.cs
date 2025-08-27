using AutoMapper;
using AutoMapper.Execution;
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
        //UserDTO dto = PocoEmit.Mapper.Default.Convert<User, UserDTO>(new User());
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
    private static void ConfigureAutoMapper(IServiceCollection services)
    {
        services.AddLogging();
        services.AddAutoMapper(cfg => cfg.CreateMap<User, UserDTO>());
    }

    //T __f<T>(System.Func<T> f) => f();
    //static CustomerDTO _autoMap(Customer source, CustomerDTO destination, ResolutionContext context)
    //{
    //    return (source == null) ?
    //        (destination == null) ? (CustomerDTO)null : destination :
    //        __f(() => {
    //            CustomerDTO typeMapDestination = null;
    //            typeMapDestination = destination ?? new CustomerDTO();
    //            try
    //            {
    //                typeMapDestination.Id = source.Id;
    //            }
    //            catch (Exception ex)
    //            {
    //                throw TypeMapPlanBuilder.MemberMappingError(
    //                    ex,
    //                    default(PropertyMap)/*NOTE: Provide the non-default value for the Constant!*/);
    //            }
    //            try
    //            {
    //                typeMapDestination.Name = source.Name;
    //            }
    //            catch (Exception ex)
    //            {
    //                throw TypeMapPlanBuilder.MemberMappingError(
    //                    ex,
    //                    default(PropertyMap)/*NOTE: Provide the non-default value for the Constant!*/);
    //            }
    //            try
    //            {
    //                Address resolvedValue = null;
    //                Address mappedValue = null;
    //                resolvedValue = source.Address;
    //                mappedValue = (resolvedValue == null) ? (Address)null :
    //                    ((Func<Address, Address, ResolutionContext, Address>)((
    //                        Address source_1,
    //                        Address destination_1,
    //                        ResolutionContext context) => //Address
    //                        (source_1 == null) ?
    //                            (destination_1 == null) ? (Address)null : destination_1 :
    //                            __f(() => {
    //                                Address typeMapDestination_1 = null;
    //                                typeMapDestination_1 = destination_1 ?? new Address();
    //                                try
    //                                {
    //                                    typeMapDestination_1.Id = source_1.Id;
    //                                }
    //                                catch (Exception ex)
    //                                {
    //                                    throw TypeMapPlanBuilder.MemberMappingError(
    //                                        ex,
    //                                        default(PropertyMap)/*NOTE: Provide the non-default value for the Constant!*/);
    //                                }
    //                                try
    //                                {
    //                                    typeMapDestination_1.Street = source_1.Street;
    //                                }
    //                                catch (Exception ex)
    //                                {
    //                                    throw TypeMapPlanBuilder.MemberMappingError(
    //                                        ex,
    //                                        default(PropertyMap)/*NOTE: Provide the non-default value for the Constant!*/);
    //                                }
    //                                try
    //                                {
    //                                    typeMapDestination_1.City = source_1.City;
    //                                }
    //                                catch (Exception ex)
    //                                {
    //                                    throw TypeMapPlanBuilder.MemberMappingError(
    //                                        ex,
    //                                        default(PropertyMap)/*NOTE: Provide the non-default value for the Constant!*/);
    //                                }
    //                                try
    //                                {
    //                                    typeMapDestination_1.Country = source_1.Country;
    //                                }
    //                                catch (Exception ex)
    //                                {
    //                                    throw TypeMapPlanBuilder.MemberMappingError(
    //                                        ex,
    //                                        default(PropertyMap)/*NOTE: Provide the non-default value for the Constant!*/);
    //                                }
    //                                return typeMapDestination_1;
    //                            })))
    //                    .Invoke(
    //                        resolvedValue,
    //                        (destination == null) ? (Address)null :
    //                            typeMapDestination.Address,
    //                        context);
    //                typeMapDestination.Address = mappedValue;
    //            }
    //            catch (Exception ex)
    //            {
    //                throw TypeMapPlanBuilder.MemberMappingError(
    //                    ex,
    //                    default(PropertyMap)/*NOTE: Provide the non-default value for the Constant!*/);
    //            }
    //            try
    //            {
    //                Address resolvedValue_1 = null;
    //                AddressDTO mappedValue_1 = null;
    //                resolvedValue_1 = source.HomeAddress;
    //                mappedValue_1 = (resolvedValue_1 == null) ? (AddressDTO)null :
    //                    ((Func<Address, AddressDTO, ResolutionContext, AddressDTO>)((
    //                        Address source_2,
    //                        AddressDTO destination_2,
    //                        ResolutionContext context) => //AddressDTO
    //                        (source_2 == null) ?
    //                            (destination_2 == null) ? (AddressDTO)null : destination_2 :
    //                            __f(() => {
    //                                AddressDTO typeMapDestination_2 = null;
    //                                typeMapDestination_2 = destination_2 ?? new AddressDTO();
    //                                try
    //                                {
    //                                    typeMapDestination_2.Id = source_2.Id;
    //                                }
    //                                catch (Exception ex)
    //                                {
    //                                    throw TypeMapPlanBuilder.MemberMappingError(
    //                                        ex,
    //                                        default(PropertyMap)/*NOTE: Provide the non-default value for the Constant!*/);
    //                                }
    //                                try
    //                                {
    //                                    typeMapDestination_2.City = source_2.City;
    //                                }
    //                                catch (Exception ex)
    //                                {
    //                                    throw TypeMapPlanBuilder.MemberMappingError(
    //                                        ex,
    //                                        default(PropertyMap)/*NOTE: Provide the non-default value for the Constant!*/);
    //                                }
    //                                try
    //                                {
    //                                    typeMapDestination_2.Country = source_2.Country;
    //                                }
    //                                catch (Exception ex)
    //                                {
    //                                    throw TypeMapPlanBuilder.MemberMappingError(
    //                                        ex,
    //                                        default(PropertyMap)/*NOTE: Provide the non-default value for the Constant!*/);
    //                                }
    //                                return typeMapDestination_2;
    //                            })))
    //                    .Invoke(
    //                        resolvedValue_1,
    //                        (destination == null) ? (AddressDTO)null :
    //                            typeMapDestination.HomeAddress,
    //                        context);
    //                typeMapDestination.HomeAddress = mappedValue_1;
    //            }
    //            catch (Exception ex)
    //            {
    //                throw TypeMapPlanBuilder.MemberMappingError(
    //                    ex,
    //                    default(PropertyMap)/*NOTE: Provide the non-default value for the Constant!*/);
    //            }
    //            try
    //            {
    //                Address[] resolvedValue_2 = null;
    //                AddressDTO[] mappedValue_2 = null;
    //                resolvedValue_2 = source.Addresses;
    //                mappedValue_2 = (resolvedValue_2 == null) ?
    //                    Array.Empty<AddressDTO>() :
    //                    __f(() => {
    //                        AddressDTO[] destinationArray = null;
    //                        int destinationArrayIndex = default;
    //                        destinationArray = new AddressDTO[resolvedValue_2.Length];
    //                        destinationArrayIndex = default(int);
    //                        int sourceArrayIndex = default;
    //                        Address sourceItem = null;
    //                        sourceArrayIndex = default(int);
    //                        while (true)
    //                        {
    //                            if ((sourceArrayIndex < resolvedValue_2.Length))
    //                            {
    //                                sourceItem = resolvedValue_2[sourceArrayIndex];
    //                                destinationArray[destinationArrayIndex++] = ((Func<Address, AddressDTO, ResolutionContext, AddressDTO>)((
    //                                    Address source_2,
    //                                    AddressDTO destination_2,
    //                                    ResolutionContext context) => //AddressDTO
    //                                    (source_2 == null) ?
    //                                        (destination_2 == null) ? (AddressDTO)null : destination_2 :
    //                                        __f(() => {
    //                                            AddressDTO typeMapDestination_2 = null;
    //                                            typeMapDestination_2 = destination_2 ?? new AddressDTO();
    //                                            try
    //                                            {
    //                                                typeMapDestination_2.Id = source_2.Id;
    //                                            }
    //                                            catch (Exception ex)
    //                                            {
    //                                                throw TypeMapPlanBuilder.MemberMappingError(
    //                                                    ex,
    //                                                    default(PropertyMap)/*NOTE: Provide the non-default value for the Constant!*/);
    //                                            }
    //                                            try
    //                                            {
    //                                                typeMapDestination_2.City = source_2.City;
    //                                            }
    //                                            catch (Exception ex)
    //                                            {
    //                                                throw TypeMapPlanBuilder.MemberMappingError(
    //                                                    ex,
    //                                                    default(PropertyMap)/*NOTE: Provide the non-default value for the Constant!*/);
    //                                            }
    //                                            try
    //                                            {
    //                                                typeMapDestination_2.Country = source_2.Country;
    //                                            }
    //                                            catch (Exception ex)
    //                                            {
    //                                                throw TypeMapPlanBuilder.MemberMappingError(
    //                                                    ex,
    //                                                    default(PropertyMap)/*NOTE: Provide the non-default value for the Constant!*/);
    //                                            }
    //                                            return typeMapDestination_2;
    //                                        })))
    //                                .Invoke(
    //                                    sourceItem,
    //                                    (AddressDTO)null,
    //                                    context);
    //                                sourceArrayIndex++;
    //                            }
    //                            else
    //                            {
    //                                goto LoopBreak;
    //                            }
    //                        }
    //                    LoopBreak:;
    //                        return destinationArray;
    //                    });
    //                typeMapDestination.Addresses = mappedValue_2;
    //            }
    //            catch (Exception ex)
    //            {
    //                throw TypeMapPlanBuilder.MemberMappingError(
    //                    ex,
    //                    default(PropertyMap)/*NOTE: Provide the non-default value for the Constant!*/);
    //            }
    //            try
    //            {
    //                List<Address> resolvedValue_3 = null;
    //                List<AddressDTO> mappedValue_3 = null;
    //                resolvedValue_3 = source.WorkAddresses;
    //                mappedValue_3 = (resolvedValue_3 == null) ?
    //                    new List<AddressDTO>() :
    //                    __f(() => {
    //                        List<AddressDTO> collectionDestination = null;
    //                        List<AddressDTO> passedDestination = null;
    //                        passedDestination = (destination == null) ? (List<AddressDTO>)null :
    //                            typeMapDestination.WorkAddresses;
    //                        collectionDestination = passedDestination ?? new List<AddressDTO>();
    //                        collectionDestination.Clear();
    //                        List<Address>.Enumerator enumerator = default;
    //                        Address item = null;
    //                        enumerator = resolvedValue_3.GetEnumerator();
    //                        try
    //                        {
    //                            while (true)
    //                            {
    //                                if (enumerator.MoveNext())
    //                                {
    //                                    item = enumerator.Current;
    //                                    collectionDestination.Add(((Func<Address, AddressDTO, ResolutionContext, AddressDTO>)((
    //                                        Address source_2,
    //                                        AddressDTO destination_2,
    //                                        ResolutionContext context) => //AddressDTO
    //                                        (source_2 == null) ?
    //                                            (destination_2 == null) ? (AddressDTO)null : destination_2 :
    //                                            __f(() => {
    //                                                AddressDTO typeMapDestination_2 = null;
    //                                                typeMapDestination_2 = destination_2 ?? new AddressDTO();
    //                                                try
    //                                                {
    //                                                    typeMapDestination_2.Id = source_2.Id;
    //                                                }
    //                                                catch (Exception ex)
    //                                                {
    //                                                    throw TypeMapPlanBuilder.MemberMappingError(
    //                                                        ex,
    //                                                        default(PropertyMap)/*NOTE: Provide the non-default value for the Constant!*/);
    //                                                }
    //                                                try
    //                                                {
    //                                                    typeMapDestination_2.City = source_2.City;
    //                                                }
    //                                                catch (Exception ex)
    //                                                {
    //                                                    throw TypeMapPlanBuilder.MemberMappingError(
    //                                                        ex,
    //                                                        default(PropertyMap)/*NOTE: Provide the non-default value for the Constant!*/);
    //                                                }
    //                                                try
    //                                                {
    //                                                    typeMapDestination_2.Country = source_2.Country;
    //                                                }
    //                                                catch (Exception ex)
    //                                                {
    //                                                    throw TypeMapPlanBuilder.MemberMappingError(
    //                                                        ex,
    //                                                        default(PropertyMap)/*NOTE: Provide the non-default value for the Constant!*/);
    //                                                }
    //                                                return typeMapDestination_2;
    //                                            })))
    //                                    .Invoke(
    //                                        item,
    //                                        (AddressDTO)null,
    //                                        context));
    //                                }
    //                                else
    //                                {
    //                                    goto LoopBreak_1;
    //                                }
    //                            }
    //                        LoopBreak_1:;
    //                        }
    //                        finally
    //                        {
    //                            enumerator.Dispose();
    //                        }
    //                        return collectionDestination;
    //                    });
    //                typeMapDestination.WorkAddresses = mappedValue_3;
    //            }
    //            catch (Exception ex)
    //            {
    //                throw TypeMapPlanBuilder.MemberMappingError(
    //                    ex,
    //                    default(PropertyMap)/*NOTE: Provide the non-default value for the Constant!*/);
    //            }
    //            return typeMapDestination;
    //        });
    //}
}
