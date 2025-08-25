using PocoEmit.MapperUnitTests.Supports;

namespace PocoEmit.MapperUnitTests.Members;

public class DelegateTests
{
    [Fact]
    public void CheckAction()
    {
        Action<User, UserDTO> checkAction = CheckId;
        checkAction += CheckName;
        var user = new User { Id = 111, Name = "Jxj" };
        var dto = Mapper.Default.Convert<User, UserDTO>(user);
        Equal(user, dto);
        var dto2 = new UserDTO();
        checkAction(user, dto2);
        Equal(user, dto2);;
    }

    [Fact]
    public void CheckDelegate()
    {
        Delegate checkDelegate = CheckId;
        checkDelegate = Delegate.Combine(checkDelegate, CheckName);
        Assert.NotNull(checkDelegate);
        Action<User, UserDTO> checkDelegate2 = (checkDelegate as Action<User, UserDTO>)!;
        Assert.NotNull(checkDelegate2);
        var user = new User { Id = 111, Name = "Jxj" };
        var dto = new UserDTO();
        checkDelegate2(user, dto);
        Equal(user, dto);
    }

    private static void Equal(User user, UserDTO dto)
    {
        Assert.Equal(user.Id, dto.Id);
        Assert.Equal(user.Name, dto.Name);
    }

    private static void CheckId(User user, UserDTO dto)
    {
        dto.Id = user.Id;
    }
    private static void CheckName(User user, UserDTO dto)
    {
        dto.Name = user.Name;
    }
}
