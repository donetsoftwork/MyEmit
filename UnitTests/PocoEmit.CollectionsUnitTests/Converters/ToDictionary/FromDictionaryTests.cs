using PocoEmit.CollectionsUnitTests.Supports;

namespace PocoEmit.CollectionsUnitTests.Converters.ToDictionary;

public class FromDictionaryTests : DictionaryTestBase
{
    [Fact]
    public void Convert_DictionaryUser2DTO()
    {
        Dictionary<int, User> source = new() { { 1, new User { Id = 1, Name = "Jxj" } } };
        var result = _mapper.Convert<Dictionary<int, User>, Dictionary<int, UserDTO>>(source);
        Equal(source, result);
    }
    [Fact]
    public void Convert_DictionaryUserArray2DTO()
    {
        Dictionary<int, User> sourceItems = new() { { 1, new User { Id = 1, Name = "Jxj" } } };
        var source = new UserDictionary { Name = "VIP", Users = sourceItems };
        var result = _mapper.Convert<UserDictionary, UserDTODictionary>(source);
        Assert.NotNull(result);
        Assert.Equal(source.Name, result.Name);
        Equal(sourceItems, result.Users);        
    }
    [Fact]
    public void ListConfigureMap()
    {
        IMapper mapper = Mapper.Create();
        // Emit默认不支持字符串转int,需要扩展
        // mapper.UseSystemConvert();
        mapper.ConfigureMap<AutoUserDTO, User>();
        Dictionary<string, AutoUserDTO> source = new() { { "222", new AutoUserDTO { UserId = "222", UserName = "Jxj" } } };
        var converter = mapper.GetConverter<Dictionary<string, AutoUserDTO>, Dictionary< int,User>>();
        var result = converter.Convert(source);
        Equal(source, result);
    }
}
