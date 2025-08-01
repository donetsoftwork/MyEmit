using PocoEmit.MapperUnitTests.Supports;

namespace PocoEmit.MapperUnitTests.New;

public class MapperConvertIdTests : MapperConvertIdTestBase
{
    [Fact]
    public void Convert_Id()
    {
        int source = 11;
        var result = _mapper.Convert<int, MyMapperId>(source);
        Assert.Equal(source, result.Id);
    }
    [Fact]
    public void Convert_Id2()
    {
        int source = 11;
        // todo
        Assert.Throws<InvalidOperationException>(() => _mapper.Convert<int, MyMapperId2>(source));
        //var result = _mapper.Convert<int, MyMapperId2>(source);
        //Assert.Equal(source, result.Id);
    }
    [Fact]
    public void Convert_IdNullable()
    {
        int? source = 11;
        var result = _mapper.Convert<int?, MyMapperId>(source);
        Assert.Equal(source, result.Id);
    }
    [Fact]
    public void Convert_MyMapperId()
    {
        var source = new MyMapperId(22);
        var result = _mapper.Convert<MyMapperId, int>(source);
        Assert.Equal(source.Id, result);
    }
    [Fact]
    public void Convert_MyMapperId2()
    {
        var source = new MyMapperId2(22);
        var result = _mapper.Convert<MyMapperId2, int>(source);
        Assert.Equal(source.Id, result);
    }
    [Fact]
    public void Convert_MyMapperDTO()
    {
        var source = new MyMapper(new MyMapperId(111)) { Name = nameof(MyMapper) };
        var result = _mapper.Convert<MyMapper, MyMapperDTO>(source);
        Assert.Equal(source.Id.Id, result.Id);
        Assert.Equal(source.Name, result.Name);
    }
    [Fact]
    public void Convert_MyMapper()
    {
        var source = new MyMapperDTO { Id = 222, Name = nameof(MyMapperDTO) };
        var result = _mapper.Convert<MyMapperDTO, MyMapper>(source);
        Assert.Equal(source.Id, result.Id.Id);
        Assert.Equal(source.Name, result.Name);
    }

    [Fact]
    public void Convert_MyMapperDTO2()
    {
        var source = new MyMapper(new MyMapperId(111)) { Name = nameof(MyMapper) };
        var result = _mapper.Convert<MyMapper, MyMapperDTO2>(source);
        Assert.Equal(source.Id.Id, result.Id);
        Assert.Equal(source.Name, result.Name);
    }
}
