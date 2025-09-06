using AutoMapper;
using MapperBench.Supports;
using Microsoft.Extensions.DependencyInjection;
using System.Linq.Expressions;

namespace MapperBench;

public class NodeTests
{
    #region 配置
    private ServiceCollection _services = new();
    private AutoMapper.IMapper _auto;

    public NodeTests()
    {
        ConfigureAutoMapper(_services);
        var serviceProvider = _services.BuildServiceProvider();
        _auto = serviceProvider.GetRequiredService<AutoMapper.IMapper>();
    }

    private static void ConfigureAutoMapper(ServiceCollection services)
    {
        services.AddLogging();
        services.AddAutoMapper(CreateMap);
    }
    private static void CreateMap(IMapperConfigurationExpression cfg)
    {
        cfg.CreateMap<Node, NodeDTO>();
    }
    /// <summary>
    /// 
    /// </summary>
    public IMapper Auto
        => _auto;
    #endregion
    public NodeDTO AutoMap()
    {
        Node node1 = new() { Id = 1, Name = "node1", SortOrder = 1 };
        Node node2 = new() { Id = 2, Name = "node2", SortOrder = 2, Parent = node1 };
        var dto = _auto.Map<Node, NodeDTO>(node2);
        return dto;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public string BuildAuto()
    {
        LambdaExpression expression = _auto.ConfigurationProvider.BuildExecutionPlan(typeof(Node), typeof(NodeDTO));
        string code = FastExpressionCompiler.ToCSharpPrinter.ToCSharpString(expression);
        Console.WriteLine(code);
        return code;
    }
}
