namespace PocoEmit.ServiceProviderUnitTests.Supports;

public class User
{
    public int Id { get; set; }
    public string Name { get; set; }
}
public record UserDTO
{
    public int Id { get; set; }
    public string Name { get; set; }
}

public class AutoUserDTO
{
    public string UserId { get; set; }
    public string UserName { get; set; }
}

public class UserCustomDTO(string userName)
{
    public int? UId { get; set; }

    public string UName { get; } = userName;
}

public class UserCustomCopier
{
    public static UserCustomDTO ActivatoryByUser(User user)
        => new(user.Name);
    public static void Copy(User user, UserCustomDTO dto)
    {
        dto.UId = user.Id;
    }
    public static void Copy(UserCustomDTO dto, User user)
    {
        var userId = dto.UId;
        if (userId is not null)
            user.Id = userId.Value;
        user.Name = dto.UName;
    }
}
