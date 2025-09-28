using PocoEmit.Dictionaries;
using PocoEmit.DictionariesUnitTests.Supports;

namespace PocoEmit.DictionariesUnitTests;

public class DictionaryCopyTests : CollectionTestBase
{
    [Fact]
    public void CreatetDictionaryCopyAction()
    {
        var action = _mapper.CreatetDictionaryCopyAction<Student, Dictionary<string, string>>();
        Assert.NotNull(action);
        Student source = new()
        {
            User = new() { Id = 2, Name = "Jxj" },
            Role = "班长",
            Skill = new() { { "足球", "很好" }, { "篮球", "优秀" } }
        };
        var result = new Dictionary<string, string>();
        action(source, result);
        Assert.NotNull(result);
        Assert.Equal(4, result.Count);
        Assert.True(result.ContainsKey("UserName"));
    }

    [Fact]
    public void CreatetDictionaryCopyAction2()
    {
        var action = _mapper.CreatetDictionaryCopyAction<Student, Dictionary<string, int>>();
        Assert.NotNull(action);
        Student source = new()
        {
            User = new() { Id = 2, Name = "Jxj" },
            Age = 17,
            Score = new() { { "语文", 95 }, { "数学", 96 } }
        };
        var dic = new Dictionary<string, int>();
        action(source, dic);
        Assert.NotNull(dic);
        Assert.Equal(4, dic.Count);
        Assert.True(dic.ContainsKey("UserId"));
    }
    [Fact]
    public void CreatetDictionaryCopyAction3()
    {
        var action = _mapper.CreatetDictionaryCopyAction<Student, int, Dictionary<string, string>>();
        Assert.NotNull(action);
        Student source = new()
        {
            User = new() { Id = 2, Name = "Jxj" },
            Age = 17,
            Score = new() { { "语文", 95 }, { "数学", 96 } }
        };
        var dic = new Dictionary<string, string>();
        action(source, dic);
        Assert.NotNull(dic);
        Assert.Equal(4, dic.Count);
        Assert.True(dic.ContainsKey("UserId"));
    }
    [Fact]
    public void CreatetDictionaryCopyAction4()
    {
        var action = _mapper.CreatetDictionaryCopyAction<Student, int, IDictionary<string, string>>();
        Assert.NotNull(action);
        Student source = new()
        {
            User = new() { Id = 2, Name = "Jxj" },
            Age = 17,
            Score = new() { { "语文", 95 }, { "数学", 96 } }
        };
        var dic = new Dictionary<string, string>();
        action(source, dic);
        Assert.NotNull(dic);
        Assert.Equal(4, dic.Count);
        Assert.True(dic.ContainsKey("UserId"));
    }

    [Fact]
    public void CreatetDictionaryCopyAction5()
    {
        var expression = _mapper.BuildDictionaryCopier<Node, NodeId, IDictionary<string, int>>();
        var code = FastExpressionCompiler.ToCSharpPrinter.ToCSharpString(expression);
        Assert.NotNull(code);
        var action = _mapper.CreatetDictionaryCopyAction<Node, NodeId, IDictionary<string, int>>();
        Assert.NotNull(action);
        Node node1 = new() { Id = new(1), Name = "node1", SortOrder = 1 };
        Node node2 = new() { Id = new(2), Name = "node2", SortOrder = 2, Parent = node1 };
        var dic = new Dictionary<string, int>();
        action(node2, dic);
        Assert.NotNull(dic);
    }
}
