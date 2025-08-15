using PocoEmit.Collections;

namespace PocoEmit.CollectionsUnitTests.Counters;

public class CountTests
{
    private readonly CollectionContainer _container = CollectionContainer.Instance;
    [Fact]
    public void Array()
    {
        int[] collection = [1, 2, 3];
        Count(collection, collection.Length);
    }
    [Fact]
    public void List()
    {
        List<int> collection = [1, 2, 3];
        Count(collection, collection.Count);
    }
    [Fact]
    public void IList()
    {
        IList<int> collection = [1, 2, 3];
        Count(collection, collection.Count);
    }
    [Fact]
    public void HashSet()
    {
        HashSet<int> collection = [1, 2, 3];
        Count(collection, collection.Count);
    }
    [Fact]
    public void ISet()
    {
        ISet<int> collection = new HashSet<int>([1, 2, 3]);
        Count(collection, collection.Count);
    }
    [Fact]
    public void Dictionary()
    {
        Dictionary<string, int> collection = new() { { "a", 1 }, { "b", 2 }, { "c", 3 } };
        Count(collection, collection.Count);
    }
    [Fact]
    public void IDictionary()
    {
        IDictionary<string, int> collection = new Dictionary<string, int>() { { "a", 1 }, { "b", 2 }, { "c", 3 } };
        Count(collection, collection.Count);
    }

    private void Count<TCollection>(TCollection collection, int expected)
    {
        var count = _container.Count(collection);
        Assert.Equal(expected, count);
    }
}
