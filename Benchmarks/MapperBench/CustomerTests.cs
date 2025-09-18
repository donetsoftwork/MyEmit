using FastExpressionCompiler;
using MapperBench.Supports;
using PocoEmit;
using System.Linq.Expressions;
using System.Reflection;

namespace MapperBench;

public class CustomerTests
{
    private static PocoEmit.IMapper _poco = ConfigurePocoMapper();

    public static string BuildPoco()
    {
        Expression<Func<Customer, CustomerDTO>> expression = _poco.BuildConverter<Customer, CustomerDTO>();
        var code = FastExpressionCompiler.ToCSharpPrinter.ToCSharpString(expression);
        Console.WriteLine(code);
        return code;
    }

    public static CustomerDTO ByFast()
    {
        Expression<Func<Customer, CustomerDTO>> expression = _poco.BuildConverter<Customer, CustomerDTO>();
        var func = FastExpressionCompiler.ExpressionCompiler.CompileFast<Func<Customer, CustomerDTO>>(expression);
        return func(CustomerConvertBench.Customer);
    }

    public static CustomerDTO BySys()
    {
        Expression<Func<Customer, CustomerDTO>> expression = _poco.BuildConverter<Customer, CustomerDTO>();
        var func = expression.Compile();
        var methodInfo = func.GetMethodInfo();
        var debugInfo = func.TryGetDebugInfo();
        return func(CustomerConvertBench.Customer);
    }

    private static PocoEmit.IMapper ConfigurePocoMapper()
    {
        var mapper = PocoEmit.Mapper.Create();
        mapper.UseCollection();
        mapper.ConfigureMap<Customer, CustomerDTO>()
            .UseCheckAction((s, t) => CustomerConvertBench.ConvertAddressCity(s, t));
        return mapper;
    }
    public static readonly Func<Customer, CustomerDTO> CustomerToDTO = ((Customer source) => //CustomerDTO
        ((Func<Customer, CustomerDTO>)((Customer source_1) => //CustomerDTO
        {
            CustomerDTO dest = null;
            if ((source_1 != (Customer)null))
            {
                dest = new CustomerDTO();
                Address member0 = null;
                Address member1 = null;
                Address[] member2 = null;
                List<Address> member3 = null;
                dest.Id = source_1.Id;
                dest.Name = source_1.Name;
                member0 = source_1.Address;
                if ((member0 != null))
                {
                    dest.Address = member0;
                }
                member1 = source_1.HomeAddress;
                if ((member1 != null))
                {
                    dest.HomeAddress = ((Func<Address, AddressDTO>)((Address source_2) => //AddressDTO
                    {
                        AddressDTO dest_1 = null;
                        if ((source_2 != (Address)null))
                        {
                            dest_1 = new AddressDTO();
                            dest_1.Id = source_2.Id;
                            dest_1.City = source_2.City;
                            dest_1.Country = source_2.Country;
                        }
                        return dest_1;
                    }))
                    .Invoke(
                        member1);
                }
                member2 = source_1.Addresses;
                if ((member2 != null))
                {
                    // { The block result will be assigned to `dest.Addresses`
                    int count = default;
                    AddressDTO[] dest_2 = null;
                    int index = default;
                    Address sourceItem = null;
                    count = member2.Length;
                    dest_2 = new AddressDTO[count];
                    while (true)
                    {
                        if ((index < count))
                        {
                            sourceItem = member2[index];
                            dest_2[index] = ((Func<Address, AddressDTO>)((Address source_2) => //AddressDTO
                            {
                                AddressDTO dest_1 = null;
                                if ((source_2 != (Address)null))
                                {
                                    dest_1 = new AddressDTO();
                                    dest_1.Id = source_2.Id;
                                    dest_1.City = source_2.City;
                                    dest_1.Country = source_2.Country;
                                }
                                return dest_1;
                            }))
                                .Invoke(
                                    sourceItem);
                            index++;
                        }
                        else
                        {
                            goto forLabel;
                        }
                    }
                forLabel:;
                    dest.Addresses = dest_2;
                    // } end of block assignment;
                }
                member3 = source_1.WorkAddresses;
                if ((member3 != null))
                {
                    // { The block result will be assigned to `dest.WorkAddresses`
                    List<AddressDTO> dest_3 = null;
                    dest_3 = new List<AddressDTO>(member3.Count);
                    int index_1 = default;
                    int len = default;
                    len = member3.Count;
                    while (true)
                    {
                        if ((index_1 < len))
                        {
                            Address sourceItem_1 = null;
                            AddressDTO destItem = null;
                            sourceItem_1 = member3[index_1];
                            destItem = ((Func<Address, AddressDTO>)((Address source_2) => //AddressDTO
                            {
                                AddressDTO dest_1 = null;
                                if ((source_2 != (Address)null))
                                {
                                    dest_1 = new AddressDTO();
                                    dest_1.Id = source_2.Id;
                                    dest_1.City = source_2.City;
                                    dest_1.Country = source_2.Country;
                                }
                                return dest_1;
                            }))
                                .Invoke(
                                    sourceItem_1);
                            dest_3.Add(destItem);
                            index_1++;
                        }
                        else
                        {
                            goto forLabel_1;
                        }
                    }
                forLabel_1:;
                    dest.WorkAddresses = dest_3;
                    // } end of block assignment;
                }
                CustomerConvertBench.ConvertAddressCity(
                        source_1,
                        dest);
            }
            return dest;
        }))
        .Invoke(
            source));
}
