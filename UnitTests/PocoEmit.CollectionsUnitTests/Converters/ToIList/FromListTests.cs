using PocoEmit.CollectionsUnitTests.Supports;

namespace PocoEmit.CollectionsUnitTests.Converters.ToIList;

public class FromListTests : CollectionTestBase
{
    [Fact]
    public void Convert_ListUser2DTO()
    {
        List<User> source = [new User { Id = 1, Name = "Jxj" }, new User { Id = 2, Name = "张三" }];
        var result = _mapper.Convert<List<User>, List<UserDTO>>(source);
        Assert.NotNull(result);
        Assert.Equal(source.Count, result.Count);
        var sourceItem = source[0];
        var resultItem = result[0];
        Assert.Equal(sourceItem.Id, resultItem.Id);
        Assert.Equal(sourceItem.Name, resultItem.Name);
    }
    [Fact]
    public void Convert_ListUserArray2DTO()
    {
        List<User> sourceItems = [new User { Id = 1, Name = "Jxj" }, new User { Id = 2, Name = "张三" }];
        var source = new UserList { Name = "VIP", Users = sourceItems };
        var result = _mapper.Convert<UserList, UserDTOList>(source);
        Assert.NotNull(result);
        Assert.Equal(source.Name, result.Name);
        var resultItems = result.Users;
        Assert.Equal(sourceItems.Count, resultItems.Count);
        var sourceItem = sourceItems[0];
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
        List<AutoUserDTO> source = [new AutoUserDTO { UserId = "222", UserName = "Jxj" }, new AutoUserDTO { UserId = "333", UserName = "李四" }];
        var converter = mapper.GetConverter<List<AutoUserDTO>, List<User>>();
        var result = converter.Convert(source);
        Assert.NotNull(result);
        Assert.Equal(source.Count, result.Count);
        var sourceItem = source[0];
        var resultItem = result[0];
        // ConfigureMap默认把类名作为前缀
        Assert.Equal(sourceItem.UserId, resultItem.Id.ToString());
        Assert.Equal(sourceItem.UserName, resultItem.Name);
    }
}
