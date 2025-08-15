using PocoEmit.Collections;

namespace PocoEmit.CollectionsUnitTests.Counters;

public class CountFuncTests
{
    private readonly CollectionContainer _container = CollectionContainer.Instance;
    [Fact]
    public void Array()
    {
        int[] collection = [1, 2, 3];
        CountFunc(collection, collection.Length);
    }
    [Fact]
    public void List()
    {
        List<int> collection = [1, 2, 3];
        CountFunc(collection, collection.Count);
    }
    [Fact]
    public void IList()
    {
        IList<int> collection = [1, 2, 3];
        CountFunc(collection, collection.Count);
    }
    [Fact]
    public void HashSet()
    {
        HashSet<int> collection = [1, 2, 3];
        CountFunc(collection, collection.Count);
    }
    [Fact]
    public void ISet()
    {
        ISet<int> collection = new HashSet<int>([1, 2, 3]);
        CountFunc(collection, collection.Count);
    }
    [Fact]
    public void Dictionary()
    {
        Dictionary<string, int> collection = new() { { "a", 1 }, { "b", 2 }, { "c", 3 } };
        CountFunc(collection, collection.Count);
    }
    [Fact]
    public void IDictionary()
    {
        IDictionary<string, int> collection = new Dictionary<string, int>() { { "a", 1 }, { "b", 2 }, { "c", 3 } };
        CountFunc(collection, collection.Count);
    }

    private void CountFunc<TCollection>(TCollection collection, int expected)
    {
        var countFunc = _container.GetCountFunc<TCollection>();
        var count = countFunc(collection);
        Assert.Equal(expected, count);
    }
}
