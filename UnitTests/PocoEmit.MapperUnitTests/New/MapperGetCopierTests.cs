using PocoEmit.MapperUnitTests.Supports;

namespace PocoEmit.MapperUnitTests.New;

public class MapperGetCopierTests : MapperConvertTestBase
{
    [Fact]
    public void GetCopier_User2DTO()
    {
        // Act
        var copier = _mapper.GetCopier<User, UserDTO>();
        // Assert
        Assert.NotNull(copier);
        var source = new User { Id = 1, Name = "Jxj" };
        var dest = new UserDTO();
        copier.Copy(source, dest);
        Assert.Equal(source.Id, dest.Id);
        Assert.Equal(source.Name, dest.Name);
    }
    [Fact]
    public void GetCopier_DTO2User()
    {
        // Act
        var copier = _mapper.GetCopier<UserDTO, User>();
        // Assert
        Assert.NotNull(copier);
        var source = new UserDTO { Id = 3, Name = "张三" };
        var dest = new User();
        copier.Copy(source, dest);
        Assert.Equal(source.Id, dest.Id);
        Assert.Equal(source.Name, dest.Name);
    }
}
