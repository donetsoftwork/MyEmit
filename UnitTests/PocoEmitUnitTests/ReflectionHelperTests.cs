using Hand.Cache;
using Hand.Reflection;
using PocoEmit;
using PocoEmit.Collections;

namespace PocoEmitUnitTests;

public class ReflectionHelperTests
{
    [Fact]
    public void HasGenericType()
    {
        var list = ReflectionType.HasGenericType(typeof(List<int>), typeof(IEnumerable<>));
        Assert.True(list);
        var str = ReflectionType.HasGenericType(typeof(string), typeof(IEnumerable<>));
        Assert.True(str);       
    }
    [Fact]
    public void GetGenericCloseInterfaces()
    {
        var listType = typeof(List<int>);
        var interfaces = ReflectionType.GetGenericCloseInterfaces(listType, typeof(IEnumerable<>))
            .ToArray();
        Assert.Single(interfaces);
        var @interface = interfaces[0];
        Assert.NotNull(@interface);
        Assert.True(@interface.IsGenericType);
        var elementType = @interface.GetGenericArguments()[0];
        Assert.Equal(typeof(int), elementType);
    }
    //[Fact]
    //public void GetGenericCloseInterfaces_None()
    //{
    //    var strType = typeof(string);
    //    var interfaces = ReflectionType.GetGenericCloseInterfaces(strType, typeof(IEnumerable<>))
    //        .ToArray();
    //    Assert.Empty(interfaces);
    //}
    [Fact]
    public void GetGenericCloseInterfaces_BaseType()
    {
        var cacherType = typeof(DoubleCacher);
        var interfaces = ReflectionType.GetGenericCloseInterfaces(cacherType, typeof(ICacher<,>))
            .ToArray();
        Assert.Single(interfaces);
        var @interface = interfaces[0];
        Assert.NotNull(@interface);
        Assert.True(@interface.IsGenericType);
        Assert.Equal(typeof(ICacher<int, int>), @interface);
        var cacher = new DoubleCacher();
        var value = cacher.Get(8);
        Assert.Equal(16, value);
    }


    class DoubleCacher(ICacher<int, int> cacher)
        : CacheFactoryBase<int, int>(cacher)
    {
        public DoubleCacher()
            : this(new DictionaryCacher<int, int>())
        {
        }
        protected override int CreateNew(in int key)
            => key << 1;
    }
}
