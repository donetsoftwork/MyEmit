namespace PocoEmitUnitTests.Supports;


public class UserConverter
{
    public static User ToUser(UserDTO dto)
    {
        return new User(dto.UserId, dto.UserName);
    }
    public static UserDTO ToDTO(User user)
    {
        return new UserDTO(user.Id, user.Name);
    }
}


public record User(int Id, string Name);
public record UserDTO(int UserId, string UserName);

