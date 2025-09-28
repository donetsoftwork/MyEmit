using PocoEmit.Dictionaries;
using PocoEmit.DictionariesUnitTests.Supports;

namespace PocoEmit.DictionariesUnitTests;

public class DictionaryConvertTests : CollectionTestBase
{
    [Fact]
    public void CreateDictionaryConvertFunc()
    {
        var func = _mapper.CreateDictionaryConvertFunc<Student, Dictionary<string, string>>();
        Assert.NotNull(func);
        Student source = new()
        {
            User = new() { Id = 2, Name = "Jxj" },
            Role = "班长",
            Skill = new() { { "足球", "很好" }, { "篮球", "优秀" } }
        };
        var result = func(source);
        Assert.NotNull(result);
        Assert.Equal(4, result.Count);
    }

    [Fact]
    public void CreateDictionaryConvertFunc2()
    {
        var func = _mapper.CreateDictionaryConvertFunc<Student, Dictionary<string, int>>();
        Assert.NotNull(func);
        Student source = new()
        {
            User = new() { Id = 2, Name = "Jxj" },
            Age = 17,
            Score = new() { { "语文", 95 }, { "数学", 96 } }
        };
        var result = func(source);
        Assert.NotNull(result);
        Assert.Equal(4, result.Count);
    }
    [Fact]
    public void CreateDictionaryConvertFunc3()
    {
        var func = _mapper.CreateDictionaryConvertFunc<Student, int, Dictionary<string, string>>();
        Assert.NotNull(func);
        Student source = new()
        {
            User = new() { Id = 2, Name = "Jxj" },
            Age = 17,
            Score = new() { { "语文", 95 }, { "数学", 96 } }
        };
        var result = func(source);
        Assert.NotNull(result);
        Assert.Equal(4, result.Count);
    }
    [Fact]
    public void CreateDictionaryConvertFunc4()
    {
        var func = _mapper.CreateDictionaryConvertFunc<Student, int, IDictionary<string, string>>();
        Assert.NotNull(func);
        Student source = new()
        {
            User = new() { Id = 2, Name = "Jxj" },
            Age = 17,
            Score = new() { { "语文", 95 }, { "数学", 96 } }
        };
        var result = func(source);
        Assert.NotNull(result);
        Assert.Equal(4, result.Count);
    }

    [Fact]
    public void CreateDictionaryConvertFunc5()
    {
        var func = _mapper.CreateDictionaryConvertFunc<Node, string, IDictionary<string, int>>();
        Assert.NotNull(func);
        Node node1 = new() { Id = new(1), Name = "node1", SortOrder = 1 };
        Node node2 = new() { Id = new(2), Name = "node2", SortOrder = 2, Parent = node1 };
        var result = func(node2);
        Assert.NotNull(result);
    }


    [Fact]
    public void CreateDictionaryConvertFunc6()
    {
        var func = _mapper.CreateDictionaryConvertFunc<Node, NodeId, IDictionary<string, int>>();
        Assert.NotNull(func);
        Node node1 = new() { Id = new(1), Name = "node1", SortOrder = 1 };
        Node node2 = new() { Id = new(2), Name = "node2", SortOrder = 2, Parent = node1 };
        var result = func(node2);
        Assert.NotNull(result);
    }
}
