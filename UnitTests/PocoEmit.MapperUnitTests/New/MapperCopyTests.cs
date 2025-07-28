using PocoEmit.MapperUnitTests.Supports;

namespace PocoEmit.MapperUnitTests.New;

public class MapperCopyTests : MapperConvertTestBase
{
    [Fact]
    public void Copy_User2DTO()
    {
        var source = new User { Id = 1, Name = "Jxj" };
        var dest = new UserDTO();
        _mapper.Copy(source, dest);
        Assert.Equal(source.Id, dest.Id);
        Assert.Equal(source.Name, dest.Name);
    }
    [Fact]
    public void Copy_DTO2User()
    {
        var source = new UserDTO { Id = 3, Name = "张三" };
        var dest = new User();
        _mapper.Copy(source, dest);
        Assert.Equal(source.Id, dest.Id);
        Assert.Equal(source.Name, dest.Name);
    }
    [Fact]
    public void Copy_Manager2DTO()
    {
        var source = new Manager { Id = 11, Name = "AAA", User = new User { Id = 1, Name = "Jxj" } };
        var dest = new ManagerDTO();
        _mapper.Copy(source, dest);
        Assert.Equal(source.Id, dest.Id);
        Assert.Equal(source.Name, dest.Name);
        Assert.Equal(source.User.Id, dest.User.Id);
        Assert.Equal(source.User.Name, dest.User.Name);
    }
    [Fact]
    public void Copy_DTO2Manager()
    {
        var source = new ManagerDTO { Id = 22, Name = "BBB", User = new UserDTO { Id = 3, Name = "张三" } };
        var dest = new Manager();
        _mapper.Copy(source, dest);
        Assert.Equal(source.Id, dest.Id);
        Assert.Equal(source.Name, dest.Name);
        Assert.Equal(source.User.Id, dest.User.Id);
        Assert.Equal(source.User.Name, dest.User.Name);
    }
    [Fact]
    public void Copy_Manager2DTOWithNull()
    {
        var source = new Manager { Id = 11, Name = "AAA" };
        var dest = new Manager();
        _mapper.Copy(source, dest);
        Assert.Equal(source.Id, dest.Id);
        Assert.Equal(source.Name, dest.Name);
    }
    [Fact]
    public void Copy_DTO2ManagerWithNull()
    {
        var source = new ManagerDTO { Id = 22, Name = "BBB" };
        var dest = new Manager();
        _mapper.Copy(source, dest);
        Assert.Equal(source.Id, dest.Id);
        Assert.Equal(source.Name, dest.Name);
    }

    [Fact]
    public void Copy_Struct()
    {
        var source = new StructSource { Id = 22, Name = "BBB" };
        var dest = new UserDTO();
        _mapper.Copy(source, dest);
        Assert.Equal(source.Id, dest.Id);
        Assert.Equal(source.Name, dest.Name);
    }
    
}
