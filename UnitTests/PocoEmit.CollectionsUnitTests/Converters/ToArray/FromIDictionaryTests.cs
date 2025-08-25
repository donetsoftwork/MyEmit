using PocoEmit.CollectionsUnitTests.Supports;

namespace PocoEmit.CollectionsUnitTests.Converters.ToArray;

public class FromIDictionaryTests : CollectionTestBase
{
    [Fact]
    public void Convert_DictionaryUser2DTO()
    {
        IDictionary<int, User> source = new Dictionary<int, User>() { { 1, new User { Id = 1, Name = "Jxj" } } };
        var result = _mapper.Convert<IDictionary<int, User>, UserDTO[]>(source);
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
        IDictionary<int, User> sourceItems = new Dictionary<int, User>() { { 1, new User { Id = 1, Name = "Jxj" } } };
        var source = new UserIDictionary { Name = "VIP", Users = sourceItems };
        var result = _mapper.Convert<UserIDictionary, UserDTOArray>(source);
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
        IMapper mapper = Mapper.Create();
        // Emit默认不支持字符串转int,需要扩展
        // mapper.UseSystemConvert();
        mapper.ConfigureMap<AutoUserDTO, User>();
        IDictionary<string, AutoUserDTO> source = new Dictionary<string, AutoUserDTO>() { { "222", new AutoUserDTO { UserId = "222", UserName = "Jxj" } } };
        var converter = mapper.GetConverter<IDictionary<string, AutoUserDTO>, User[]>();
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
