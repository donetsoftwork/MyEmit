using PocoEmit.CollectionsUnitTests.Supports;

namespace PocoEmit.CollectionsUnitTests.Converters.ToIEnumerable;

public class FromArrayTests : CollectionTestBase
{
    [Fact]
    public void Convert_User2DTO()
    {
        User[] source = [new User { Id = 1, Name = "Jxj" }, new User { Id = 2, Name = "张三" }];
        var result = _mapper.Convert<User[], IEnumerable<UserDTO>>(source);
        Assert.NotNull(result);
        Equal(source[0], result.First());
    }
    [Fact]
    public void Convert_UserArray2DTO()
    {
        var expression = _mapper.BuildConverter<UserArray, UserDTOIEnumerable>();
        var code = FastExpressionCompiler.ToCSharpPrinter.ToCSharpString(expression);
        Assert.NotNull(code);
        User[] sourceItems = [new User { Id = 1, Name = "Jxj" }, new User { Id = 2, Name = "张三" }];
        var source = new UserArray { Name = "VIP", Users = sourceItems };
        var result = _mapper.Convert<UserArray, UserDTOIEnumerable>(source);
        Assert.NotNull(result);
        Assert.Equal(source.Name, result.Name);
        var resultItems = result.Users;
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
        AutoUserDTO[] source = [new AutoUserDTO { UserId = "222", UserName = "Jxj" }, new AutoUserDTO { UserId = "333", UserName = "李四" }];
        var converter = mapper.GetConverter<AutoUserDTO[], IEnumerable<User>>();
        var result = converter.Convert(source);
        Assert.NotNull(result);
        Equal(source[0], result.First());
    }
}
