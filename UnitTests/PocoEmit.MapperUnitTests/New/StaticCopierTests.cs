using PocoEmit.MapperUnitTests.Supports;

namespace PocoEmit.MapperUnitTests.New;

public class StaticCopierTests
{
    private readonly IMapper _mapper = Mapper.Create();

    public StaticCopierTests()
    {
        _mapper.UseStaticCopier<UserCustomCopier>();
        _mapper.UseActivator<User, UserCustomDTO>(u => UserCustomCopier.ActivatoryByUser(u));
    }
    [Fact]
    public void ToDTO()
    {
        var expression = _mapper.BuildConverter<User, UserCustomDTO>();
        var code = FastExpressionCompiler.ToCSharpPrinter.ToCSharpString(expression);
        Assert.NotNull(code);
        var source = new User { Id = 111, Name = "Jxj" };
        var converter = _mapper.GetConverter<User, UserCustomDTO>();
        var result = converter.Convert(source);
        Assert.NotNull(result);
        Assert.Equal(source.Id, result.UId);
        Assert.Equal(source.Name, result.UName);
    }
    [Fact]
    public void ToUser()
    {
        var expression = _mapper.BuildConverter<UserCustomDTO, User>();
        var code = FastExpressionCompiler.ToCSharpPrinter.ToCSharpString(expression);
        Assert.NotNull(code);

        var source = new UserCustomDTO("Jxj2") { UId = 222 };
        var converter = _mapper.GetConverter<UserCustomDTO, User>();
        var result = converter.Convert(source);
        Assert.NotNull(result);
        Assert.Equal(source.UId, result.Id);
        Assert.Equal(source.UName, result.Name);
    }

}
