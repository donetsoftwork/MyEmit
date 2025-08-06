using PocoEmit.MapperUnitTests.Supports;

namespace PocoEmit.MapperUnitTests.New;

public class MapperActivatorTest
{
    [Fact]
    public void UseActivator()
    {
        IMapper mapper = Mapper.Create();
        mapper.ConfigureMap<User, UserCustomDTO>()
            .UseActivator(u => new UserCustomDTO(u.Name))
            .Source
            .MapTo(nameof(User.Id), nameof(UserCustomDTO.UId));
        var source = new User { Id = 222, Name = "Jxj2" };
        var converter = mapper.GetConverter<User, UserCustomDTO>();
        var result = converter.Convert(source);
        Assert.NotNull(result);
        Assert.Equal(source.Id, result.UId);
        Assert.Equal(source.Name, result.UName);
    }
}
