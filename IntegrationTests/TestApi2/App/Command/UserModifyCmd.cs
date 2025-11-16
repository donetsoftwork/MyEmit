namespace TestApi2.App.Command;

public class UserModifyCmd(long id, IDictionary<string, object?> data)
{
    public long Id { get; } = id;
    /// <summary>
    /// 变化的数据
    /// </summary>
    public IDictionary<string, object?> Data { get; } = data;

    public UserModifyResult Execute()
    {

        return new();
    }
}
