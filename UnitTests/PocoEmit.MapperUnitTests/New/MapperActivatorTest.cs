using PocoEmit.MapperUnitTests.Supports;

namespace PocoEmit.MapperUnitTests.New;

public class MapperActivatorTest
{
    [Fact]
    public void UseActivator()
    {
        const string name = "UseActivator";
        IMapper mapper = Mapper.Create();
        mapper.ConfigureMap<User, UserCustomDTO>()
            .UseActivator(() => new UserCustomDTO(name))
            .Source
            .MapTo(nameof(User.Id), nameof(UserCustomDTO.UId));
        var expression = mapper.BuildConverter<User, UserCustomDTO>();
        var code = FastExpressionCompiler.ToCSharpPrinter.ToCSharpString(expression);
        Assert.NotNull(code);

        var source = new User { Id = 111, Name = name };
        var converter = mapper.GetConverter<User, UserCustomDTO>();
        var result = converter.Convert(source);
        Assert.NotNull(result);       

        
        Assert.Equal(source.Id, result.UId);
        Assert.Equal(source.Name, result.UName);
    }
    [Fact]
    public void UseActivator2()
    {
        IMapper mapper = Mapper.Create();
        mapper.ConfigureMap<User, UserCustomDTO>()
            .UseActivator(u => new UserCustomDTO(u.Name))
            .Source
            .MapTo(nameof(User.Id), nameof(UserCustomDTO.UId));
        var expression = mapper.BuildConverter<User, UserCustomDTO>();
        var code = FastExpressionCompiler.ToCSharpPrinter.ToCSharpString(expression);
        Assert.NotNull(code);

        var source = new User { Id = 222, Name = "Jxj2" };
        var converter = mapper.GetConverter<User, UserCustomDTO>();
        var result = converter.Convert(source);
        Assert.NotNull(result);


        Assert.Equal(source.Id, result.UId);
        Assert.Equal(source.Name, result.UName);
    }
    [Fact]
    public void UseActivator3()
    {
        const int userId = 222;
        IMapper mapper = Mapper.Create();
        mapper.ConfigureMap<User, UserCustomDTO>()
            .UseActivator(u => new UserCustomDTO(u.Name) { UId = userId })
            .Source
            .MapTo(nameof(User.Id), nameof(UserCustomDTO.UId));
        var expression = mapper.BuildConverter<User, UserCustomDTO>();
        var code = FastExpressionCompiler.ToCSharpPrinter.ToCSharpString(expression);
        Assert.NotNull(code);

        var source = new User { Id = userId, Name = "Jxj2" };
        var converter = mapper.GetConverter<User, UserCustomDTO>();
        var result = converter.Convert(source);
        Assert.NotNull(result);


        Assert.Equal(source.Id, result.UId);
        Assert.Equal(source.Name, result.UName);
    }
}
