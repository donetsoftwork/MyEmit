namespace TestApi2.App.Command;

public class UserCreateCmd
{
    public string Name { get; set; }
    public int? Age { get; set; }

    public UserCreateResult Execute()
    {

        return new();
    }
}
