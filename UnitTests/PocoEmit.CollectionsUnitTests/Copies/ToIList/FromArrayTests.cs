using PocoEmit.CollectionsUnitTests.Supports;

namespace PocoEmit.CollectionsUnitTests.Copies.ToIList;

public class FromArrayTests : CollectionTestBase
{
    [Fact]
    public void Copy_User2DTO()
    {
        User[] source = [new User { Id = 1, Name = "Jxj" }, new User { Id = 2, Name = "张三" }];
        IList<UserDTO> result = [];
        _mapper.Copy(source, result);
        Assert.Equal(source.Length, result.Count);
        Equal(source[0], result[0]);
    }
    [Fact]
    public void Copy_UserArray2DTO()
    {
        User[] sourceItems = [new User { Id = 1, Name = "Jxj" }, new User { Id = 2, Name = "张三" }];
        var source = new UserArray { Name = "VIP", Users = sourceItems };
        var result = new UserDTOIList();
        _mapper.Copy(source, result);
        Assert.Equal(source.Name, result.Name);
        var resultItems = result.Users;
        Assert.Equal(sourceItems.Length, resultItems.Count);
        var sourceItem = sourceItems[0];
        var resultItem = resultItems[0];
        Assert.Equal(sourceItem.Id, resultItem.Id);
        Assert.Equal(sourceItem.Name, resultItem.Name);
    }
    [Fact]
    public void ConfigureMap()
    {
        IMapper mapper = Mapper.Create();
        // Emit默认不支持字符串转int,需要扩展
        // mapper.UseSystemConvert();
        mapper.ConfigureMap<AutoUserDTO, User>();
        AutoUserDTO[] source = [new AutoUserDTO { UserId = "222", UserName = "Jxj" }, new AutoUserDTO { UserId = "333", UserName = "李四" }];
        IList<User> result = [];
        var copier = mapper.GetCopier<AutoUserDTO[], IList<User>>();
        copier.Copy(source, result);
        Assert.Equal(source.Length, result.Count);
        Equal(source[0], result[0]);
    }
}
