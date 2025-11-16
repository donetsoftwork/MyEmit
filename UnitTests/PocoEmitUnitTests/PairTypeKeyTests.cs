using Hand.Reflection;
using PocoEmit.Configuration;

namespace PocoEmitUnitTests;

public class PairTypeKeyTests
{
    [Fact]
    public void CheckValueType()
    {
        var str = PairTypeKey.CheckValueType(typeof(string), typeof(object));
        Assert.True(str);
        var obj = PairTypeKey.CheckValueType(typeof(object), typeof(string));
        Assert.False(obj);
    }
}
