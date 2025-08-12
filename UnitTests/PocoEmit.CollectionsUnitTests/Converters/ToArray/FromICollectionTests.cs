using PocoEmit.CollectionsUnitTests.Supports;

namespace PocoEmit.CollectionsUnitTests.Converters.ToArray;

public class FromICollectionTests : CollectionTestBase
{
    [Fact]
    public void Convert_ICollectionUser2DTO()
    {
        ICollection<User> source = [new User { Id = 1, Name = "Jxj" }, new User { Id = 2, Name = "张三" }];
        var result = _mapper.Convert<ICollection<User>, UserDTO[]>(source);
        Assert.NotNull(result);
        Assert.Equal(source.Count, result.Length);
        Equal(source.First(), result[0]);
    }
    [Fact]
    public void Convert_ICollectionUserArray2DTO()
    {
        ICollection<User> sourceItems = [new User { Id = 1, Name = "Jxj" }, new User { Id = 2, Name = "张三" }];
        var source = new UserICollection { Name = "VIP", Users = sourceItems };
        var result = _mapper.Convert<UserICollection, UserDTOArray>(source);
        Assert.NotNull(result);
        Assert.Equal(source.Name, result.Name);
        var resultItems = result.Users;
        Assert.Equal(sourceItems.Count, resultItems.Length);
        Equal(sourceItems.First(), resultItems[0]);
    }
    [Fact]
    public void ICollectionConfigureMap()
    {
        IMapper mapper = Mapper.Create();
        // Emit默认不支持字符串转int,需要扩展
        mapper.UseSystemConvert();
        mapper.ConfigureMap<AutoUserDTO, User>();
        ICollection<AutoUserDTO> source = [new AutoUserDTO { UserId = "222", UserName = "Jxj" }, new AutoUserDTO { UserId = "333", UserName = "李四" }];
        var converter = mapper.GetConverter<ICollection<AutoUserDTO>, User[]>();
        var result = converter.Convert(source);
        Assert.NotNull(result);
        Assert.Equal(source.Count, result.Length);
        Equal(source.First(), result[0]);
    }
}
