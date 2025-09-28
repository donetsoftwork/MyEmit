using PocoEmit.CollectionsUnitTests.Supports;
using PocoEmit.Dictionaries;

namespace PocoEmit.CollectionsUnitTests.Dictionaries;

public class DictionaryConvertTests : CollectionTestBase
{
    [Fact]
    public void ToDictionary()
    {
        User user = new() { Id = 3, Name = "张三" };
        var dic = _mapper.ToDictionary(user);
        Assert.NotNull(dic);
        Assert.Equal(user.Id, dic[nameof(user.Id)]);
        Assert.Equal(user.Name, dic[nameof(user.Name)]);
    }
    [Fact]
    public void GetToDictionaryFunc()
    {
        var func = _mapper.GetToDictionaryFunc<User>();
        User user = new() { Id = 3, Name = "张三" };
        var dic = func(user);
        Assert.NotNull(dic);
        Assert.Equal(user.Id, dic[nameof(user.Id)]);
        Assert.Equal(user.Name, dic[nameof(user.Name)]);
    }
    [Fact]
    public void FromDictionary()
    {
        Dictionary<string, object> dic = new() { { nameof(User.Id), "3" }, { nameof(User.Name), "张三" } };
        var user = _mapper.FromDictionary<User>(dic);
        Assert.NotNull(dic);
        Assert.Equal(dic[nameof(user.Id)], user.Id.ToString());
        Assert.Equal(dic[nameof(user.Name)], user.Name);
    }
    [Fact]
    public void GetFromDictionaryFunc()
    {
        var func = _mapper.GetFromDictionaryFunc<User>();
        Dictionary<string, object> dic = new() { { nameof(User.Id), "3" }, { nameof(User.Name), "张三" } };
        var user = func(dic);
        Assert.NotNull(dic);
        Assert.Equal(dic[nameof(user.Id)], user.Id.ToString());
        Assert.Equal(dic[nameof(user.Name)], user.Name);
    }    
}
