using PocoEmit.CollectionsUnitTests.Supports;

namespace PocoEmit.CollectionsUnitTests.Complexes;

public class DtoTests
{
    [Fact]
    public void Test()
    {
        var func = GetPocoFunc();
        var test = DtoTest.GetTest();
        var result = func(test);
        Assert.NotNull(result);
    }

    private static Func<DtoTest, DtoTest2> GetPocoFunc()
    {
        var poco = PocoEmit.Mapper.Create(new PocoEmit.Configuration.MapperOptions { Cached = PocoEmit.Configuration.ComplexCached.Circle })
           .UseCollection();
        var func = poco.GetConvertFunc<DtoTest, DtoTest2>();
        return func;
    }
}
