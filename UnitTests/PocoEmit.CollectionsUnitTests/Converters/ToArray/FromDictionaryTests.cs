using PocoEmit.CollectionsUnitTests.Supports;

namespace PocoEmit.CollectionsUnitTests.Converters.ToArray;

public class FromDictionaryTests : CollectionTestBase
{
    [Fact]
    public void Convert_DictionaryUser2DTO()
    {
        Dictionary<int, User> source = new() { { 1, new User { Id = 1, Name = "Jxj" } } };
        var result = _mapper.Convert<Dictionary<int, User>, UserDTO[]>(source);
        Assert.NotNull(result);
        Assert.Equal(source.Count, result.Length);
        var sourceItem = source.Values.First();
        var resultItem = result[0];
        Assert.Equal(sourceItem.Id, resultItem.Id);
        Assert.Equal(sourceItem.Name, resultItem.Name);
    }
    [Fact]
    public void Convert_DictionaryUserArray2DTO()
    {
        Dictionary<int, User> sourceItems = new() { { 1, new User { Id = 1, Name = "Jxj" } } };
        var source = new UserDictionary { Name = "VIP", Users = sourceItems };
        var result = _mapper.Convert<UserDictionary, UserDTOArray>(source);
        Assert.NotNull(result);
        Assert.Equal(source.Name, result.Name);
        var resultItems = result.Users;
        Assert.Equal(sourceItems.Count, resultItems.Length);
        var sourceItem = sourceItems.Values.First();
        var resultItem = resultItems[0];
        Assert.Equal(sourceItem.Id, resultItem.Id);
        Assert.Equal(sourceItem.Name, resultItem.Name);
    }
    [Fact]
    public void ListConfigureMap()
    {
        _mapper.ConfigureMap<AutoUserDTO, User>();
        Dictionary<string, AutoUserDTO> source = new() { { "222", new AutoUserDTO { UserId = "222", UserName = "Jxj" } } };
        var converter = _mapper.GetConverter<Dictionary<string, AutoUserDTO>, User[]>();
        var result = converter.Convert(source);
        Assert.NotNull(result);
        Assert.Equal(source.Count, result.Length);
        var sourceItem = source.Values.First();
        var resultItem = result[0];
        // ConfigureMap默认把类名作为前缀
        Assert.Equal(sourceItem.UserId, resultItem.Id.ToString());
        Assert.Equal(sourceItem.UserName, resultItem.Name);
    }
}
