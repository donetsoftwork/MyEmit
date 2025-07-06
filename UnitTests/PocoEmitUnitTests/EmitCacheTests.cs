using PocoEmit;
using System.Reflection;
using System.Xml.Linq;

namespace PocoEmitUnitTests;

public class EmitCacheTests
{
    [Fact]
    public void PropertyEmitGetter()
    {
        int id = 43;
        var instance = new InstancePropertyHelperTests { Id = id };
        var property = typeof(InstancePropertyHelperTests).GetProperty(nameof(instance.Id), BindingFlags.Instance | BindingFlags.Public);
        var getter = EmitCaches.EmitGetter(property);
        var value = getter(instance);
        Assert.Equal(id, value);
    }
    [Fact]
    public void FieldGetter()
    {
        int id = 43;
        var instance = new InstanceFieldHelperTests { Id = id };
        var field = typeof(InstanceFieldHelperTests).GetField(nameof(instance.Id), BindingFlags.Instance | BindingFlags.Public);
        var getter = EmitCaches.EmitGetter(field);
        var value = getter(instance);
        Assert.Equal(id, value);
    }
    [Fact]
    public void PropertyEmitSetter()
    {
        string name = "Test4";
        var instance = new InstancePropertyHelperTests { Name = name };
        var property = typeof(InstancePropertyHelperTests).GetProperty(nameof(instance.Name), BindingFlags.Instance | BindingFlags.Public);
        var setter = EmitCaches.EmitSetter(property);
        setter(instance, name);
        Assert.Equal(name, instance.Name);
    }
    [Fact]
    public void FieldEmitSetter()
    {
        string name = "Test4";
        var instance = new InstanceFieldHelperTests { Name = name };
        var field = typeof(InstanceFieldHelperTests).GetField(nameof(instance.Name), BindingFlags.Instance | BindingFlags.Public);
        var setter = EmitCaches.EmitSetter(field);
        setter(instance, name);
        Assert.Equal(name, instance.Name);
    }
}
