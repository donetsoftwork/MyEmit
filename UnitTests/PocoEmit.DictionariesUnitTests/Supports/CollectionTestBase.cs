namespace PocoEmit.DictionariesUnitTests.Supports;

public class CollectionTestBase
{
    protected readonly IMapper _mapper;
    public CollectionTestBase()
    {
        //CollectionContainer.GlobalUseCollection();
        _mapper = Mapper.Default;
        _mapper.UseCollection();
    }
}
