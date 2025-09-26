using PocoEmit.CollectionsUnitTests.Supports;

namespace PocoEmit.CollectionsUnitTests.Copies.ToCollection;

public class FromArrayTests : CollectionTestBase
{
    [Fact]
    public void Copy_User2DTO()
    {
        User[] source = [new User { Id = 1, Name = "Jxj" }, new User { Id = 2, Name = "张三" }];
        ICollection<UserDTO> result = [];
        _mapper.Copy(source, result);
        Assert.Equal(source.Length, result.Count);
        Equal(source[0], result.First());
    }
    [Fact]
    public void Copy_UserArray2DTO()
    {
        User[] sourceItems = [new User { Id = 1, Name = "Jxj" }, new User { Id = 2, Name = "张三" }];
        var source = new UserArray { Name = "VIP", Users = sourceItems };
        var result = new UserDTOICollection();
        _mapper.Copy(source, result);
        Assert.Equal(source.Name, result.Name);
        var resultItems = result.Users;
        Assert.Equal(sourceItems.Length, resultItems.Count);
        var sourceItem = sourceItems[0];
        var resultItem = resultItems.First();
        Assert.Equal(sourceItem.Id, resultItem.Id);
        Assert.Equal(sourceItem.Name, resultItem.Name);
    }
    [Fact]
    public void ConfigureMap()
    {
        _mapper.ConfigureMap<AutoUserDTO, User>();
        AutoUserDTO[] source = [new AutoUserDTO { UserId = "222", UserName = "Jxj" }, new AutoUserDTO { UserId = "333", UserName = "李四" }];
        ICollection<User> result = [];
        var copier = _mapper.GetCopier<AutoUserDTO[], ICollection<User>>();
        copier.Copy(source, result);
        Assert.Equal(source.Length, result.Count);
        Equal(source[0], result.First());
    }
}
