using PocoEmit.Collections;
using System.Collections.Concurrent;

namespace PocoEmit.CollectionsUnitTests.Saves;

public class SaveActionTests
{
    private readonly CollectionContainer _container = CollectionContainer.Instance;
    [Fact]
    public void List()
    {
        List<int> collection = [1, 2, 3];
        SaveAction(collection, collection.Count);
    }
    [Fact]
    public void IList()
    {
        IList<int> collection = [1, 2, 3];
        SaveAction(collection, collection.Count);
    }
    [Fact]
    public void ICollection()
    {
        ICollection<int> collection = [1, 2, 3];
        SaveAction(collection, collection.Count);
    }
    [Fact]
    public void IProducerConsumerCollection()
    {
        IProducerConsumerCollection<int> collection = new ConcurrentBag<int>([1, 2, 3]);
        SaveAction(collection, collection.Count);
    }
    [Fact]
    public void HashSet()
    {
        HashSet<int> collection = [1, 2, 3];
        SaveAction(collection, collection.Count);
    }
    [Fact]
    public void ISet()
    {
        ISet<int> collection = new HashSet<int>([1, 2, 3]);
        SaveAction(collection, collection.Count);
    }
    [Fact]
    public void Queue()
    {
        Queue<int> collection = new([1, 2, 3]);
        SaveAction(collection, collection.Count);
    }
    [Fact]
    public void Stack()
    {
        Stack<int> collection = new([1, 2, 3]);
        SaveAction(collection, collection.Count);
    }
    [Fact]
    public void BlockingCollection()
    {
        BlockingCollection<int> collection = [1, 2, 3];
        SaveAction(collection, collection.Count);
    }
    [Fact]
    public void ConcurrentQueue()
    {
        ConcurrentQueue<int> collection = new([1, 2, 3]);
        SaveAction(collection, collection.Count);
    }
    [Fact]
    public void ConcurrentStack()
    {
        ConcurrentStack<int> collection = new([1, 2, 3]);
        SaveAction(collection, collection.Count);
    }
    [Fact]
    public void ConcurrentBag()
    {
        ConcurrentBag<int> collection = [1, 2, 3];
        SaveAction(collection, collection.Count);
    }
    private void SaveAction<TCollection>(TCollection collection, int count0)
    {
        var saveAction = _container.GetSaveAction<TCollection, int>();
        saveAction(collection, 4);
        var count = _container.Count(collection);
        Assert.Equal(count0 + 1, count);
    }
}
