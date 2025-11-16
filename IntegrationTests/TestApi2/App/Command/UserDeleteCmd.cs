namespace TestApi2.App.Command;

public class UserDeleteCmd
{
    public long Id { get; set; }
    public UserDeleteResult Execute()
    {

        return new();
    }
}
