using PocoEmit;
using System.Reflection;

namespace PocoEmitUnitTests;

public class EmitCacheTests
{
    [Fact]
    public void PropertyGetReadFunc()
    {
        int id = 43;
        var instance = new InstancePropertyHelperTests { Id = id };
        var property = typeof(InstancePropertyHelperTests).GetProperty(nameof(instance.Id), BindingFlags.Instance | BindingFlags.Public);
        var getter = InstanceHelper.GetReadFunc(property);
        var value = getter(instance);
        Assert.Equal(id, value);
    }
    [Fact]
    public void FieldGetter()
    {
        int id = 43;
        var instance = new InstanceFieldHelperTests { Id = id };
        var field = typeof(InstanceFieldHelperTests).GetField(nameof(instance.Id), BindingFlags.Instance | BindingFlags.Public);
        var getter = InstanceHelper.GetReadFunc(field);
        var value = getter(instance);
        Assert.Equal(id, value);
    }
    [Fact]
    public void PropertyGetWriteAction()
    {
        string name = "Test4";
        var instance = new InstancePropertyHelperTests { Name = name };
        var property = typeof(InstancePropertyHelperTests).GetProperty(nameof(instance.Name), BindingFlags.Instance | BindingFlags.Public);
        var setter = InstanceHelper.GetWriteAction(property);
        setter(instance, name);
        Assert.Equal(name, instance.Name);
    }
    [Fact]
    public void FieldGetWriteAction()
    {
        string name = "Test4";
        var instance = new InstanceFieldHelperTests { Name = name };
        var field = typeof(InstanceFieldHelperTests).GetField(nameof(instance.Name), BindingFlags.Instance | BindingFlags.Public);
        var setter = InstanceHelper.GetWriteAction(field);
        setter(instance, name);
        Assert.Equal(name, instance.Name);
    }
}
