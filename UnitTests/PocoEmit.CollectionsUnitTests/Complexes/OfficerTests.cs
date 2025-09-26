using PocoEmit.CollectionsUnitTests.Supports;
using PocoEmit.Configuration;

namespace PocoEmit.CollectionsUnitTests.Complexes;

public class OfficerTests : CollectionTestBase
{
    [Fact]
    public void ConvertArmy()
    {
        var army = Officer.GetArmy();
        var list = army.Keys.Concat(army.Values).Distinct().ToArray();
        Assert.Equal(7, list.Length);
        var dto = _mapper.Convert<Dictionary<Officer, Officer>, Dictionary<OfficerDTO, OfficerDTO>>(army);
        Assert.NotNull(dto);
        var dtoList = dto.Keys.Concat(dto.Values).Distinct().ToArray();
        Assert.Equal(12, dtoList.Length);
    }

    [Fact]
    public void ConvertByCached()
    {
        var mapper = PocoEmit.Mapper.Create(new MapperOptions { Cached = ComplexCached.Always });
        mapper.UseCollection();
        var army = Officer.GetArmy();
        var list = army.Keys.Concat(army.Values).Distinct().ToArray();
        Assert.Equal(7, list.Length);
        //var expression = _mapper.BuildConverter<Dictionary<Officer, Officer>, Dictionary<OfficerDTO, OfficerDTO>>();
        //var code = FastExpressionCompiler.ToCSharpPrinter.ToCSharpString(expression);
        //Assert.NotNull(code);
        var dto = _mapper.Convert<Dictionary<Officer, Officer>, Dictionary<OfficerDTO, OfficerDTO>>(army);
        Assert.NotNull(dto);
        var dtoList = dto.Keys.Concat(dto.Values).ToArray();
        Assert.Equal(12, dtoList.Length);
    }
}
