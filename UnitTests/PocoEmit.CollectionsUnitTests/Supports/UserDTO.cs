namespace PocoEmit.CollectionsUnitTests.Supports;

public class User
{
    public int Id { get; set; }
    public string Name { get; set; }
}


public class UserDTO
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
