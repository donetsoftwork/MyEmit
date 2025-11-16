namespace TestApi2.App.Query;

public class UserDetailQuery
{
    public int Id { get; set; }
    public UserDetailResult Get()
    {

        return new();
    }
}
