using PocoEmit.CollectionsUnitTests.Supports;

namespace PocoEmit.CollectionsUnitTests.Converters.ToIDictionary;

public class FromIListTests : CollectionTestBase
{
    [Fact]
    public void Convert_ListUser2DTO()
    {
        IList<User> source = [new User { Id = 1, Name = "Jxj" }, new User { Id = 2, Name = "张三" }];
        var result = _mapper.Convert<IList<User>, IDictionary<int, UserDTO>>(source);
        Assert.NotNull(result);
        Assert.Equal(source.Count, result.Count);
        Equal(source[0], result.Values.First());
    }
    [Fact]
    public void Convert_ListUserArray2DTO()
    {
        IList<User> sourceItems = [new User { Id = 1, Name = "Jxj" }, new User { Id = 2, Name = "张三" }];
        var source = new UserIList { Name = "VIP", Users = sourceItems };
        var result = _mapper.Convert<UserIList, UserDTOIDictionary>(source);
        Assert.NotNull(result);
        Assert.Equal(source.Name, result.Name);
        var resultItems = result.Users;
        Assert.Equal(sourceItems.Count, resultItems.Count);
        Equal(sourceItems[0], resultItems["0"]);
    }
    [Fact]
    public void ListConfigureMap()
    {
        _mapper.ConfigureMap<AutoUserDTO, User>();
        IList<AutoUserDTO> source = [new AutoUserDTO { UserId = "222", UserName = "Jxj" }, new AutoUserDTO { UserId = "333", UserName = "李四" }];
        var converter = _mapper.GetConverter<IList<AutoUserDTO>, IDictionary<int, User>>();
        var result = converter.Convert(source);
        Assert.NotNull(result);
        Assert.Equal(source.Count, result.Count);
        Equal(source[0], result.Values.First());
    }
}
