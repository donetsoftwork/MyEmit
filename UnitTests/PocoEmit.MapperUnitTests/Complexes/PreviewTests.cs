using PocoEmit.Complexes;
using PocoEmit.Configuration;
using PocoEmit.MapperUnitTests.Supports;

namespace PocoEmit.MapperUnitTests.Complexes;

public class PreviewTests
{
    [Fact]
    public void UserPreview()
    {
        IMapper mapper = Mapper.Create();
        var converter = mapper.GetEmitConverter<User, UserDTO>();
        var context = new BuildContext((IMapperOptions)mapper);
        var list = context.Visit(converter).ToArray();
        Assert.Single(list);
    }
    [Fact]
    public void UserPreview2()
    {
        IMapper mapper = Mapper.Create();
        var converter = mapper.GetEmitConverter<User?, UserDTO>();
        var context = new BuildContext((IMapperOptions)mapper);
        var list = context.Visit(converter).ToArray();
        Assert.Single(list);
    }
    [Fact]
    public void VisitUser()
    {
        IMapper mapper = Mapper.Create();
        var context = new BuildContext((IMapperOptions)mapper);
        _ = context.Visit<User, UserDTO>()
            .ToArray();
        Assert.Single(context.Collections);
    }
    [Fact]
    public void Visitint()
    {
        IMapper mapper = Mapper.Create();
        var context = new BuildContext((IMapperOptions)mapper);
        var list = context.Visit<int, string>()
            .ToArray();
        Assert.Empty(list);
    }
    [Fact]
    public void VisitNode()
    {
        IMapper mapper = Mapper.Create();
        var context = new BuildContext((IMapperOptions)mapper);
        var list = context.Visit<Node, NodeDTO>()
            .ToArray();
        Assert.Single(context.Collections);
    }
}
