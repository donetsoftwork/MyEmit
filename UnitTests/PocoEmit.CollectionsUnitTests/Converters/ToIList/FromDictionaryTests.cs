using PocoEmit.CollectionsUnitTests.Supports;

namespace PocoEmit.CollectionsUnitTests.Converters.ToIList;

public class FromDictionaryTests : CollectionTestBase
{
    [Fact]
    public void Convert_DictionaryUser2DTO()
    {
        Dictionary<int, User> source = new() { { 1, new User { Id = 1, Name = "Jxj" } } };
        var result = _mapper.Convert<Dictionary<int, User>, List<UserDTO>>(source);
        Assert.NotNull(result);
        Assert.Equal(source.Count, result.Count);
        Equal(source.Values.First(), result[0]);
    }
    [Fact]
    public void Convert_DictionaryUserArray2DTO()
    {
        Dictionary<int, User> sourceItems = new() { { 1, new User { Id = 1, Name = "Jxj" } } };
        var source = new UserDictionary { Name = "VIP", Users = sourceItems };
        var result = _mapper.Convert<UserDictionary, UserDTOList>(source);
        Assert.NotNull(result);
        Assert.Equal(source.Name, result.Name);
        Equal(sourceItems.Values.First(), result.Users[0]);
    }
    [Fact]
    public void ListConfigureMap()
    {
        _mapper.ConfigureMap<AutoUserDTO, User>();
        Dictionary<string, AutoUserDTO> source = new() { { "222", new AutoUserDTO { UserId = "222", UserName = "Jxj" } } };
        var converter = _mapper.GetConverter<Dictionary<string, AutoUserDTO>, List<User>>();
        var result = converter.Convert(source);
        Equal(source.Values.First(), result[0]);
    }
}
