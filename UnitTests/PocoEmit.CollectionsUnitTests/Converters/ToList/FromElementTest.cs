using PocoEmit.CollectionsUnitTests.Supports;

namespace PocoEmit.CollectionsUnitTests.Converters.ToList;

public class FromElementTest : CollectionTestBase
{
    [Fact]
    public void Convert_User()
    {
        var source = new User { Id = 1, Name = "Jxj" };
        var result = _mapper.Convert<User, List<User>>(source);
        Assert.NotNull(result);
        Assert.Single(result);
    }
    [Fact]
    public void Convert_string()
    {
        var source = "张三";
        var result = _mapper.Convert<string, List<string>>(source);
        Assert.NotNull(result);
        Assert.Single(result);
    }
    [Fact]
    public void Convert_object()
    {
        var source = "张三";
        var result = _mapper.Convert<string, List<object>>(source);
        Assert.NotNull(result);
        Assert.Single(result);
    }
}
