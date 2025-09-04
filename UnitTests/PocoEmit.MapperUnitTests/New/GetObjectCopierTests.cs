using PocoEmit.Copies;
using PocoEmit.MapperUnitTests.Supports;

namespace PocoEmit.MapperUnitTests.New;

public class GetObjectCopierTests : MapperConvertTestBase
{    
    [Fact]
    public void CopyObject_User2DTO()
    {
        var copier = _mapper.GetObjectCopier(typeof(User), typeof(UserDTO)) as IObjectCopier;
        Assert.NotNull(copier);
        var source = new User { Id = 1, Name = "Jxj" };
        var result = new UserDTO();
        copier.CopyObject(source, result);
        Assert.NotNull(result);
        Assert.Equal(source.Id, result.Id);
        Assert.Equal(source.Name, result.Name);
    }
    [Fact]
    public void CopyObject_DTO2User()
    {
        var copier = _mapper.GetObjectCopier(typeof(UserDTO), typeof(User)) as IObjectCopier;
        Assert.NotNull(copier);
        var source = new UserDTO { Id = 3, Name = "张三" };
        var result = new User();
        copier.CopyObject(source, result);
        Assert.NotNull(result);
        Assert.Equal(source.Id, result.Id);
        Assert.Equal(source.Name, result.Name);
    }
}
