using PocoEmit.CollectionsUnitTests.Supports;
using System.Collections.Concurrent;

namespace PocoEmit.CollectionsUnitTests.Copies.ToBlockingCollection;

public class FromListTests : CollectionTestBase
{
    [Fact]
    public void Copy_User2DTO()
    {
        List<User> source = [new User { Id = 1, Name = "Jxj" }, new User { Id = 2, Name = "张三" }];
        BlockingCollection<UserDTO> result = [];
        _mapper.Copy(source, result);
        Assert.Equal(source.Count, result.Count);
        Equal(source[0], result.First());
    }
    [Fact]
    public void Copy_UserArray2DTO()
    {
        List<User> sourceItems = [new User { Id = 1, Name = "Jxj" }, new User { Id = 2, Name = "张三" }];
        var source = new UserList { Name = "VIP", Users = sourceItems };
        var result = new UserDTOBlockingCollection();
        var copyAction = _mapper.GetCopyAction<UserList, UserDTOBlockingCollection>();
        copyAction(source, result);
        Assert.Equal(source.Name, result.Name);
        var resultItems = result.Users;
        Assert.Equal(sourceItems.Count, resultItems.Count);
        var sourceItem = sourceItems[0];
        var resultItem = resultItems.First();
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
        List<AutoUserDTO> source = [new AutoUserDTO { UserId = "222", UserName = "Jxj" }, new AutoUserDTO { UserId = "333", UserName = "李四" }];
        BlockingCollection<User> result = [];
        var copier = mapper.GetCopier<List<AutoUserDTO>, BlockingCollection<User>>();
        copier.Copy(source, result);
        Assert.Equal(source.Count, result.Count);
        Equal(source[0], result.First());
    }
}
