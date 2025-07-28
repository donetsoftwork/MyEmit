using PocoEmit.MapperUnitTests.Supports;

namespace PocoEmit.MapperUnitTests.New;

public class MapperGetCopyActionTests : MapperConvertTestBase
{
    [Fact]
    public void GetCopyAction_User2DTO()
    {
        // Act
        var copyAction = _mapper.GetCopyAction<User, UserDTO>();
        // Assert
        Assert.NotNull(copyAction);
        var source = new User { Id = 1, Name = "Jxj" };
        var dest = new UserDTO();
        copyAction(source, dest);
        Assert.Equal(source.Id, dest.Id);
        Assert.Equal(source.Name, dest.Name);
    }
    [Fact]
    public void GetCopyAction_DTO2User()
    {
        // Act
        var copyAction = _mapper.GetCopyAction<UserDTO, User>();
        // Assert
        Assert.NotNull(copyAction);
        var source = new UserDTO { Id = 3, Name = "张三" };
        var dest = new User();
        copyAction(source, dest);
        Assert.Equal(source.Id, dest.Id);
        Assert.Equal(source.Name, dest.Name);
    }
}
