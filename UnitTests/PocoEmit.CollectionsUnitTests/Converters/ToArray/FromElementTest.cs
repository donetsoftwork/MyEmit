using PocoEmit.CollectionsUnitTests.Supports;

namespace PocoEmit.CollectionsUnitTests.Converters.ToArray;

public class FromElementTest : CollectionTestBase
{
    [Fact]
    public void Convert_User()
    {
        var source = new User { Id = 1, Name = "Jxj" };
        var result = _mapper.Convert<User, User[]>(source);
        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Equal(source, result[0]);
    }

    [Fact]
    public void Convert_int()
    {
        var source = 123;
        var result = _mapper.Convert<int, int[]>(source);
        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Equal(source, result[0]);
    }
    [Fact]
    public void Convert_object()
    {
        var source = 123;
        var result = _mapper.Convert<int, object[]>(source);
        Assert.NotNull(result);
        Assert.Single(result);
    }
}
