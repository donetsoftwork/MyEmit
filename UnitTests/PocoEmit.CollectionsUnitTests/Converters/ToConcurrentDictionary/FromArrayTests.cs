using PocoEmit.CollectionsUnitTests.Supports;
using System.Collections.Concurrent;

namespace PocoEmit.CollectionsUnitTests.Converters.ToConcurrentDictionary;

public class FromArrayTests : CollectionTestBase
{
    [Fact]
    public void Convert_User2DTO()
    {
        User[] source = [new User { Id = 1, Name = "Jxj" }, new User { Id = 2, Name = "张三" }];
        var result = _mapper.Convert<User[], ConcurrentDictionary<int, UserDTO>>(source);
        Assert.NotNull(result);
        Assert.Equal(source.Length, result.Count);
        Equal(source[0], result.Values.First());
    }
    [Fact]
    public void Convert_UserArray2DTO()
    {
        User[] sourceItems = [new User { Id = 1, Name = "Jxj" }, new User { Id = 2, Name = "张三" }];
        var source = new UserArray { Name = "VIP", Users = sourceItems };
        var result = _mapper.Convert<UserArray, UserDTODictionary>(source);
        Assert.NotNull(result);
        Assert.Equal(source.Name, result.Name);
        var resultItems = result.Users;
        Assert.Equal(sourceItems.Length, resultItems.Count);
        var sourceItem = sourceItems[0];
        var resultItem = resultItems.Values.First();
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
        var converter = mapper.GetConverter<AutoUserDTO[], ConcurrentDictionary<int, User>>();
        var result = converter.Convert(source);
        Assert.NotNull(result);
        Assert.Equal(source.Length, result.Count);
        Equal(source[0], result.Values.First());
    }
}
