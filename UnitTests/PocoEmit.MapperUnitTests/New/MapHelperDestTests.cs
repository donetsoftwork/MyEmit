using PocoEmit.MapperUnitTests.Supports;
using FastExpressionCompiler;

namespace PocoEmit.MapperUnitTests.New;

public class MapHelperDestTests : MapHelperBaseTests
{
    [Fact]
    public void Ignore()
    {
        IMapper mapper = Mapper.Create();
        mapper.ConfigureMap<User, UserDTO>()
            .Dest
            .Ignore(nameof(UserDTO.Name));
        var source = new User { Id = 111, Name = "Jxj" };
        var converter = mapper.GetConverter<User, UserDTO>();
        var result = converter.Convert(source);
        Assert.Equal(source.Id, result.Id);
        Assert.NotEqual(source.Name, result.Name);
    }
    [Fact]
    public void Ignore2()
    {
        IMapper mapper = Mapper.Create();
        mapper.ConfigureMap<User, UserDTO>()
            .Dest
            .ForMember(nameof(UserDTO.Name)).Ignore();
        var source = new User { Id = 111, Name = "Jxj" };
        var converter = mapper.GetConverter<User, UserDTO>();
        var result = converter.Convert(source);
        Assert.NotNull(result);
        Assert.Equal(source.Id, result.Id);
        Assert.NotEqual(source.Name, result.Name);
    }
    [Fact]
    public void MapFrom()
    {
        IMapper mapper = Mapper.Create();
        mapper.ConfigureMap<UserCustomDTO, User>()
            .Dest
            .MapFrom(nameof(User.Id), nameof(UserCustomDTO.UId))
            .MapFrom(nameof(User.Name), nameof(UserCustomDTO.UName));
        var source = new UserCustomDTO("Jxj2") { UId = 222 };
        var converter = mapper.GetConverter<UserCustomDTO, User>();
        var result = converter.Convert(source);
        Assert.NotNull(result);
        Assert.Equal(source.UId, result.Id);
        Assert.Equal(source.UName, result.Name);
    }
    [Fact]
    public void MapFrom2()
    {
        IMapper mapper = Mapper.Create();
        mapper.ConfigureMap<UserCustomDTO, User>()
            .Dest
            .ForMember(nameof(User.Id)).MapFrom(nameof(UserCustomDTO.UId))
            .ForMember(nameof(User.Name)).MapFrom(nameof(UserCustomDTO.UName));
        var source = new UserCustomDTO("Jxj2") { UId = 222 };
        var converter = mapper.GetConverter<UserCustomDTO, User>();
        var result = converter.Convert(source);
        Assert.NotNull(result);
        Assert.Equal(source.UId, result.Id);
        Assert.Equal(source.UName, result.Name);
    }
    [Fact]
    public void Prefix()
    {
        IMapper mapper = Mapper.Create();
        mapper.ConfigureMap<User, UserCustomDTO>()
            .Dest
            .AddPrefix("U");
        var source = new User { Id = 222, Name = "Jxj2" };
        var expression = mapper.BuildConverter<User, UserCustomDTO>();
        var code = ToCSharpPrinter.ToCSharpString(expression);
        //var compiler = FastExpressionCompiler.ExpressionCompiler.TryCompile<Func<User, UserCustomDTO>>(expression);
        //Console.WriteLine(expression.ToString());
        var converter = expression.Compile();
        var result = converter(source);
        Assert.NotNull(result);
        Assert.Equal(source.Id, result.UId);
        Assert.Equal(source.Name, result.UName);
    }
}