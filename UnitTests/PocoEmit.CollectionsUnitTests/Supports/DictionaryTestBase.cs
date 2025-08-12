namespace PocoEmit.CollectionsUnitTests.Supports;

public class DictionaryTestBase : CollectionTestBase
{
    protected void Equal(IDictionary<int, User> source, IDictionary<int, UserDTO> dest)
    {
        Assert.NotNull(dest);
        Assert.Equal(source.Count, dest.Count);
        foreach (var item in source)
        {
            var key = item.Key;
            Equal(item.Value, dest[key]);
        }
    }
    protected void Equal(IDictionary<int, User> source, IDictionary<string, UserDTO> dest)
    {
        Assert.NotNull(dest);
        Assert.Equal(source.Count, dest.Count);
        foreach (var item in source)
        {
            var key = item.Key;
            Equal(item.Value, dest[key.ToString()]);
        }
    }
    protected void Equal(IDictionary<string, AutoUserDTO> source, IDictionary<int, User> dest)
    {
        Assert.NotNull(dest);
        Assert.Equal(source.Count, dest.Count);
        foreach (var item in source)
        {
            var key = item.Key;
            Equal(item.Value, dest[int.Parse(key)]);
        }
    }
}
