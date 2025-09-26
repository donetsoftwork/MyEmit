using PocoEmit.CollectionsUnitTests.Supports;

namespace PocoEmit.CollectionsUnitTests.Converters.ToList;

public class FromElementTest : CollectionTestBase
{
    [Fact]
    public void Convert_ListUser()
    {
        var source = new User { Id = 1, Name = "Jxj" };
        var result = _mapper.Convert<User, List<User>>(source);
        Assert.NotNull(result);
        Assert.Single(result);
    }
    [Fact]
    public void Convert_IListUser()
    {
        var source = new User { Id = 1, Name = "Jxj" };
        var result = _mapper.Convert<User, IList<User>>(source);
        Assert.NotNull(result);
        Assert.Single(result);
    }
    [Fact]
    public void Convert_IEnumerableUser()
    {
        var source = new User { Id = 1, Name = "Jxj" };
        var result = _mapper.Convert<User, IEnumerable<User>>(source);
        Assert.NotNull(result);
        Assert.Single(result);
    }
    [Fact]
    public void Convert_Liststring()
    {
        var source = "张三";
        var result = _mapper.Convert<string, List<string>>(source);
        Assert.NotNull(result);
        Assert.Single(result);
    }
    [Fact]
    public void Convert_IListstring()
    {
        var source = "张三";
        var result = _mapper.Convert<string, IList<string>>(source);
        Assert.NotNull(result);
        Assert.Single(result);
    }
    [Fact]
    public void Convert_IEnumerablestring()
    {
        var source = "张三";
        var result = _mapper.Convert<string, IEnumerable<string>>(source);
        Assert.NotNull(result);
        Assert.Single(result);
    }
    [Fact]
    public void Convert_Listobject()
    {
        var source = "张三";
        var result = _mapper.Convert<string, List<object>>(source);
        Assert.NotNull(result);
        Assert.Single(result);
    }
    [Fact]
    public void Convert_IListobject()
    {
        var source = "张三";
        var result = _mapper.Convert<string, IList<object>>(source);
        Assert.NotNull(result);
        Assert.Single(result);
    }
    [Fact]
    public void Convert_IEnumerableobject()
    {
        var source = "张三";
        var result = _mapper.Convert<string, IEnumerable<object>>(source);
        Assert.NotNull(result);
        Assert.Single(result);
    }
}
