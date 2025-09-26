namespace PocoEmit.CollectionsUnitTests.Supports;

public class CollectionTestBase
{
    protected readonly IMapper _mapper;
    public CollectionTestBase()
    {
        //CollectionContainer.GlobalUseCollection();
        _mapper = Mapper.Default;
        _mapper.UseCollection();
    }

    protected void Equal(User source, UserDTO dest)
    {
        Assert.Equal(source.Id, dest.Id);
        Assert.Equal(source.Name, dest.Name);
    }
    protected void Equal(AutoUserDTO source, User dest)
    {
        Assert.Equal(source.UserId, dest.Id.ToString());
        Assert.Equal(source.UserName, dest.Name);
    }
}
