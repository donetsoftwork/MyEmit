using PocoEmit.CollectionsUnitTests.Supports;
using System.Collections.Specialized;

namespace PocoEmit.CollectionsUnitTests.Converters.ToIDictionary;

public class FromNameValueCollectionTests : CollectionTestBase
{
    [Fact]
    public void Convert()
    {
        const string key1 = "Key1";
        var source = new NameValueCollection
        {
            { key1, "Value1" },
            { "Key2", "Value2" },
            { "Key3", "Value3" }
        };
        Assert.Throws<InvalidOperationException>(() => _mapper.Convert<NameValueCollection, IDictionary<string, string>>(source));
        //var result = _mapper.Convert<NameValueCollection, IDictionary<string, string>>(source);
        //Assert.NotNull(result);
        //Assert.Equal(source.Count, result.Count);
        //Assert.Equal(source.Get(key1), result[key1]);
    }
}
