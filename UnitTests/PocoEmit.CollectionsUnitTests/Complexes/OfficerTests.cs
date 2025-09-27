using PocoEmit.CollectionsUnitTests.Supports;
using PocoEmit.Configuration;
using PocoEmit.Resolves;

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
        var mapper = PocoEmit.Mapper.Create(new MapperOptions { Cached = ComplexCached.Always })
            .UseCollection();
        var army = Officer.GetArmy();
        var list = army.Keys.Concat(army.Values).Distinct().ToArray();
        Assert.Equal(7, list.Length);
        var expression = mapper.BuildConverter<Dictionary<Officer, Officer>, Dictionary<OfficerDTO, OfficerDTO>>();
        Assert.NotNull(expression);
        //var code = FastExpressionCompiler.ToCSharpPrinter.ToCSharpString(expression);
        //Assert.NotNull(code);
        var dto = mapper.Convert<Dictionary<Officer, Officer>, Dictionary<OfficerDTO, OfficerDTO>>(army);
        Assert.NotNull(dto);
        var dtoList = dto.Keys.Concat(dto.Values).Distinct().ToArray();
        Assert.Equal(7, dtoList.Length);
    }
    [Fact]
    public void ConvertByContextFunc()
    {
        var mapper = PocoEmit.Mapper.Create()
            .UseCollection();
        var army = Officer.GetArmy();
        var list = army.Keys.Concat(army.Values).Distinct().ToArray();
        Assert.Equal(7, list.Length);
        var converter = mapper.GetEmitContextConverter<Dictionary<Officer, Officer>, Dictionary<OfficerDTO, OfficerDTO>>();
        Assert.NotNull(converter);
        var expression = converter.Build();
        Assert.NotNull(expression);
        //var code = FastExpressionCompiler.ToCSharpPrinter.ToCSharpString(expression);
        //Assert.NotNull(code);
        var func = mapper.GetContextConvertFunc<Dictionary<Officer, Officer>, Dictionary<OfficerDTO, OfficerDTO>>();
        using var context = SingleContext<Officer, OfficerDTO>.Pool.Get();
        var dto = func(context, army);
        Assert.NotNull(dto);
        var dtoList = dto.Keys.Concat(dto.Values).Distinct().ToArray();
        Assert.Equal(7, dtoList.Length);
    }
}
