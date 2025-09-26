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
        context.Visit(converter);
        Assert.Single(context.Collections);
    }
    [Fact]
    public void UserPreview2()
    {
        IMapper mapper = Mapper.Create();
        var converter = mapper.GetEmitConverter<User?, UserDTO>();
        var context = new BuildContext((IMapperOptions)mapper);
        context.Visit(converter);
        Assert.Single(context.Collections);
    }
    [Fact]
    public void VisitUser()
    {
        IMapper mapper = Mapper.Create();
        var context = new BuildContext((IMapperOptions)mapper);
        context.Visit<User, UserDTO>();
        Assert.Single(context.Collections);
    }
    [Fact]
    public void Visitint()
    {
        IMapper mapper = Mapper.Create();
        var context = new BuildContext((IMapperOptions)mapper);
        context.Visit<int, string>();
        Assert.Empty(context.Collections);
    }
    [Fact]
    public void VisitNode()
    {
        IMapper mapper = Mapper.Create();
        var context = new BuildContext((IMapperOptions)mapper);
        context.Visit<Node, NodeDTO>();
        Assert.Single(context.Collections);
    }
}
