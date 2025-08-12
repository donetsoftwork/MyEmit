using PocoEmit.CollectionsUnitTests.Supports;

namespace PocoEmit.CollectionsUnitTests.Converters.ToArray;

public class FromIListTests : CollectionTestBase
{
    [Fact]
    public void Convert_IListUser2DTO()
    {
        IList<User> source = [new User { Id = 1, Name = "Jxj" }, new User { Id = 2, Name = "张三" }];
        var result = _mapper.Convert<IList<User>, UserDTO[]>(source);
        Assert.NotNull(result);
        Assert.Equal(source.Count, result.Length);
        Equal(source[0], result[0]);
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
        Equal(sourceItems[0], resultItems[0]);
    }
    [Fact]
    public void IListConfigureMap()
    {
        IMapper mapper = Mapper.Create();
        // Emit默认不支持字符串转int,需要扩展
        mapper.UseSystemConvert();
        mapper.ConfigureMap<AutoUserDTO, User>();
        IList<AutoUserDTO> source = [new AutoUserDTO { UserId = "222", UserName = "Jxj" }, new AutoUserDTO { UserId = "333", UserName = "李四" }];
        var converter = mapper.GetConverter<IList<AutoUserDTO>, User[]>();
        var result = converter.Convert(source);
        Assert.NotNull(result);
        Assert.Equal(source.Count, result.Length);
        Equal(source[0], result[0]);
    }
}
