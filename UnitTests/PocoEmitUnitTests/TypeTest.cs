namespace PocoEmitUnitTests;

public class TypeTest
{
    [Fact]
    public void IsValueType()
    {
        Assert.True(typeof(int).IsValueType);
        Assert.True(typeof(int?).IsValueType);
        Assert.True(typeof(DateTime).IsValueType);
        Assert.True(typeof(DateTime?).IsValueType);
        Assert.False(typeof(string).IsValueType);
        Assert.False(typeof(object).IsValueType);
    }
}
