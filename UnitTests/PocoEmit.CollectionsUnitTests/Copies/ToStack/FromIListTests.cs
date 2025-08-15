using PocoEmit.CollectionsUnitTests.Supports;

namespace PocoEmit.CollectionsUnitTests.Copies.ToStack;

public class FromIListTests : CollectionTestBase
{
    [Fact]
    public void Copy_User2DTO()
    {
        IList<User> source = [new User { Id = 1, Name = "Jxj" }, new User { Id = 2, Name = "张三" }];
        Stack<UserDTO> result = [];
        _mapper.Copy(source, result);
        Assert.Equal(source.Count, result.Count);
        Equal(source[0], result.Last());
    }
    [Fact]
    public void Copy_UserArray2DTO()
    {
        IList<User> sourceItems = [new User { Id = 1, Name = "Jxj" }, new User { Id = 2, Name = "张三" }];
        var source = new UserIList { Name = "VIP", Users = sourceItems };
        var result = new UserDTOStack();
        _mapper.Copy(source, result);
        Assert.Equal(source.Name, result.Name);
        var resultItems = result.Users;
        Assert.Equal(sourceItems.Count, resultItems.Count);
        var sourceItem = sourceItems[0];
        var resultItem = resultItems.Last();
        Assert.Equal(sourceItem.Id, resultItem.Id);
        Assert.Equal(sourceItem.Name, resultItem.Name);
    }
    [Fact]
    public void ConfigureMap()
    {
        IMapper mapper = Mapper.Create();
        // Emit默认不支持字符串转int,需要扩展
        mapper.UseSystemConvert();
        mapper.ConfigureMap<AutoUserDTO, User>();
        IList<AutoUserDTO> source = [new AutoUserDTO { UserId = "222", UserName = "Jxj" }, new AutoUserDTO { UserId = "333", UserName = "李四" }];
        Stack<User> result = [];
        var copier = mapper.GetCopier<IList<AutoUserDTO>, Stack<User>>();
        copier.Copy(source, result);
        Assert.Equal(source.Count, result.Count);
        Equal(source[0], result.Last());
    }
}
