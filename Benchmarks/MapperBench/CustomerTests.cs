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
}
