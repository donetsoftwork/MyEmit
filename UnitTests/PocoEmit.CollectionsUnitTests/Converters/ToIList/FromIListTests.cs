using PocoEmit.CollectionsUnitTests.Supports;

namespace PocoEmit.CollectionsUnitTests.Converters.ToIList;

public class FromIListTests : CollectionTestBase
{
    [Fact]
    public void Convert_IListUser2DTO()
    {
        IList<User> source = [new User { Id = 1, Name = "Jxj" }, new User { Id = 2, Name = "张三" }];
        var result = _mapper.Convert<IList<User>, UserDTO[]>(source);
        Assert.NotNull(result);
        Assert.Equal(source.Count, result.Length);
        var sourceItem = source[0];
        var resultItem = result[0];
        Assert.Equal(sourceItem.Id, resultItem.Id);
        Assert.Equal(sourceItem.Name, resultItem.Name);
    }
    [Fact]
    public void GetConvertFunc_IListUser2DTO()
    {
        IList<User> source = [new User { Id = 1, Name = "Jxj" }, new User { Id = 2, Name = "张三" }];
        var converter = _mapper.GetConvertFunc<IList<User>, UserDTO[]>();
        UserDTO[] result = converter(source);
        Assert.NotNull(result);
        Assert.Equal(source.Count, result.Length);
        var sourceItem = source[0];
        var resultItem = result[0];
        Assert.Equal(sourceItem.Id, resultItem.Id);
        Assert.Equal(sourceItem.Name, resultItem.Name);
    }
    [Fact]
    public void Convert_IListUserArray2DTO()
    {
        IList<User> sourceItems = [new User { Id = 1, Name = "Jxj" }, new User { Id = 2, Name = "张三" }];
        var source = new UserIList { Name = "VIP", Users = sourceItems };
        var result = _mapper.Convert<UserIList, UserDTOArray>(source);
        Assert.NotNull(result);
        Assert.Equal(source.Name, result.Name);
        var resultItems = result.Users;
        Assert.Equal(sourceItems.Count, resultItems.Length);
        var sourceItem = sourceItems[0];
        var resultItem = resultItems[0];
        Assert.Equal(sourceItem.Id, resultItem.Id);
        Assert.Equal(sourceItem.Name, resultItem.Name);
    }
    [Fact]
    public void IListConfigureMap()
    {
        _mapper.ConfigureMap<AutoUserDTO, User>();
        IList<AutoUserDTO> source = [new AutoUserDTO { UserId = "222", UserName = "Jxj" }, new AutoUserDTO { UserId = "333", UserName = "李四" }];
        var converter = _mapper.GetConverter<IList<AutoUserDTO>, User[]>();
        var result = converter.Convert(source);
        Assert.NotNull(result);
        Assert.Equal(source.Count, result.Length);
        var sourceItem = source[0];
        var resultItem = result[0];
        // ConfigureMap默认把类名作为前缀
        Assert.Equal(sourceItem.UserId, resultItem.Id.ToString());
        Assert.Equal(sourceItem.UserName, resultItem.Name);
    }
}
