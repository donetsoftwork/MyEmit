using PocoEmit.CollectionsUnitTests.Supports;

namespace PocoEmit.CollectionsUnitTests.Converters.ToISet;

public class FromArrayTests : CollectionTestBase
{
    [Fact]
    public void Convert_User2DTO()
    {
        User[] source = [new User { Id = 1, Name = "Jxj" }, new User { Id = 2, Name = "张三" }];
        var result = _mapper.Convert<User[], ISet<UserDTO>>(source);
        Assert.NotNull(result);
        Assert.Equal(source.Length, result.Count);
        Equal(source[0], result.First());
    }
    [Fact]
    public void Convert_UserArray2DTO()
    {
        User[] sourceItems = [new User { Id = 1, Name = "Jxj" }, new User { Id = 2, Name = "张三" }];
        var source = new UserArray { Name = "VIP", Users = sourceItems };
        var result = _mapper.Convert<UserArray, UserDTOISet>(source);
        Assert.NotNull(result);
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
        var converter = _mapper.GetConverter<AutoUserDTO[], ISet<User>>();
        var result = converter.Convert(source);
        Assert.NotNull(result);
        Assert.Equal(source.Length, result.Count);
        Equal(source[0], result.First());
    }
}
