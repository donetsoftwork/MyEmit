using PocoEmit;

namespace PocoEmitUnitTests;

public class ReflectionHelperTests
{
    [Fact]
    public void HasGenericType()
    {
        var list = ReflectionHelper.HasGenericType(typeof(List<int>), typeof(IEnumerable<>));
        Assert.True(list);
        var str = ReflectionHelper.HasGenericType(typeof(string), typeof(IEnumerable<>));
        Assert.True(str);       
    }
}
