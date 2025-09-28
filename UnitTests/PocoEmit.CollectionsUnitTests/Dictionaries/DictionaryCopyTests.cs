using PocoEmit.CollectionsUnitTests.Supports;
using PocoEmit.Dictionaries;

namespace PocoEmit.CollectionsUnitTests.Dictionaries;

public class DictionaryCopyTests : CollectionTestBase
{
    [Fact]
    public void DictionaryCopy()
    {
        User user = new() { Id = 3, Name = "张三" };
        var dic = new Dictionary<string, object>();
        _mapper.DictionaryCopy(user, dic);
        Assert.NotNull(dic);
        Assert.Equal(user.Id, dic[nameof(user.Id)]);
        Assert.Equal(user.Name, dic[nameof(user.Name)]);
    }
    [Fact]
    public void GetDictionaryCopyAction()
    {
        var action = _mapper.GetDictionaryCopyAction<User>();
        User user = new() { Id = 3, Name = "张三" };
        var dic = new Dictionary<string, object>();
        action(user, dic);
        Assert.NotNull(dic);
        Assert.Equal(user.Id, dic[nameof(user.Id)]);
        Assert.Equal(user.Name, dic[nameof(user.Name)]);
    }    
}
