using PocoEmit.CollectionsUnitTests.Supports;
using System.Collections.Concurrent;

namespace PocoEmit.CollectionsUnitTests.Copies.ToIProducerConsumerCollection;

public class FromListTests : CollectionTestBase
{
    [Fact]
    public void Copy_User2DTO()
    {
        List<User> source = [new User { Id = 1, Name = "Jxj" }, new User { Id = 2, Name = "张三" }];
        IProducerConsumerCollection<UserDTO> result = new ConcurrentBag<UserDTO>();
        _mapper.Copy(source, result);
        Assert.Equal(source.Count, result.Count);
    }
    [Fact]
    public void Copy_UserArray2DTO()
    {
        User[] sourceItems = [new User { Id = 1, Name = "Jxj" }, new User { Id = 2, Name = "张三" }];
        var source = new UserArray { Name = "VIP", Users = sourceItems };
        var result = new UserDTOIProducerConsumerCollection();
        var expression = _mapper.BuildCopier<UserArray, UserDTOIProducerConsumerCollection>();
        var code = FastExpressionCompiler.ToCSharpPrinter.ToCSharpString(expression);
        Assert.NotNull(code);
        _mapper.Copy(source, result);
        Assert.Equal(source.Name, result.Name);
        var resultItems = result.Users;
        Assert.Equal(sourceItems.Length, resultItems.Count);
    }
    [Fact]
    public void ConfigureMap()
    {
        IMapper mapper = Mapper.Create();
        // Emit默认不支持字符串转int,需要扩展
        // mapper.UseSystemConvert();
        mapper.ConfigureMap<AutoUserDTO, User>();
        AutoUserDTO[] source = [new AutoUserDTO { UserId = "222", UserName = "Jxj" }, new AutoUserDTO { UserId = "333", UserName = "李四" }];
        IProducerConsumerCollection<User> result = new ConcurrentBag<User>();
        var copier = mapper.GetCopier<AutoUserDTO[], IProducerConsumerCollection<User>>();
        copier.Copy(source, result);
        Assert.Equal(source.Length, result.Count);
    }
}
