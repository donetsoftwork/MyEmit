using PocoEmit.CollectionsUnitTests.Supports;

namespace PocoEmit.CollectionsUnitTests.Converters.ToArray;

public class FromIEnumerableTests : CollectionTestBase
{
    [Fact]
    public void Convert_IEnumerableUser2DTO()
    {
        IEnumerable<User> source = [new User { Id = 1, Name = "Jxj" }, new User { Id = 2, Name = "张三" }];
        var result = _mapper.Convert<IEnumerable<User>, UserDTO[]>(source);
        Assert.NotNull(result);
        var sourceItem = source.First();
        var resultItem = result[0];
        Assert.Equal(sourceItem.Id, resultItem.Id);
        Assert.Equal(sourceItem.Name, resultItem.Name);
    }
    [Fact]
    public void Convert_IEnumerableUserArray2DTO()
    {
        IEnumerable<User> sourceItems = [new User { Id = 1, Name = "Jxj" }, new User { Id = 2, Name = "张三" }];
        var source = new UserIEnumerable { Name = "VIP", Users = sourceItems };
        var result = _mapper.Convert<UserIEnumerable, UserDTOArray>(source);
        Assert.NotNull(result);
        Assert.Equal(source.Name, result.Name);
        var resultItems = result.Users;
        var sourceItem = sourceItems.First();
        var resultItem = resultItems[0];
        Assert.Equal(sourceItem.Id, resultItem.Id);
        Assert.Equal(sourceItem.Name, resultItem.Name);
    }
    [Fact]
    public void IEnumerableConfigureMap()
    {
        IMapper mapper = Mapper.Create();
        // Emit默认不支持字符串转int,需要扩展
        mapper.UseSystemConvert();
        mapper.ConfigureMap<AutoUserDTO, User>();
        IEnumerable<AutoUserDTO> source = [new AutoUserDTO { UserId = "222", UserName = "Jxj" }, new AutoUserDTO { UserId = "333", UserName = "李四" }];
        var converter = mapper.GetConverter<IEnumerable<AutoUserDTO>, User[]>();
        var result = converter.Convert(source);
        Assert.NotNull(result);
        var sourceItem = source.First();
        var resultItem = result[0];
        // ConfigureMap默认把类名作为前缀
        Assert.Equal(sourceItem.UserId, resultItem.Id.ToString());
        Assert.Equal(sourceItem.UserName, resultItem.Name);
    }
}
