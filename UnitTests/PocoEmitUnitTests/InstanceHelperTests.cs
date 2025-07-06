using PocoEmit;
using System.Reflection;

namespace PocoEmitUnitTests;

public class InstanceHelperTests
{
    #region EmitGetter
    #region 读取结构体属性Id的测试
    [Fact]
    public void EmitGetterId1()
    {
        int id = 41;
        var instance = new InstanceHelperTests { Id = id };
        var getter = InstanceHelper.EmitGetter<InstanceHelperTests, int>(nameof(Id));
        var value = getter(instance);
        Assert.Equal(id, value);
    }

    [Fact]
    public void EmitGetterId2()
    {
        int id = 42;
        var instance = new InstanceHelperTests { Id = id };
        var getter = InstanceHelper.EmitGetter<InstanceHelperTests, object>(nameof(Id));
        var value = getter(instance);
        Assert.Equal(id, value);
    }

    #endregion
    #region 读取可空属性Name的测试

    [Fact]
    public void EmitGetterName01()
    {
        string? name = "Test1";
        var instance = new InstanceHelperTests { Name = name };
        var getter = InstanceHelper.EmitGetter<InstanceHelperTests, string?>(nameof(Name));
        var value = getter(instance);
        Assert.Equal(name, value);
    }

    [Fact]
    public void EmitGetterName02()
    {
        string? name = "Test2";
        var instance = new InstanceHelperTests { Name = name };
        var getter = InstanceHelper.EmitGetter<InstanceHelperTests, object>(nameof(Name));
        var value = getter(instance);
        Assert.Equal(name, value);
    }
    #endregion
    #region 读取字符串属性Name的测试

    [Fact]
    public void EmitGetterName1()
    {
        string name = "Test1";
        var instance = new InstanceHelperTests { Name = name };
        var getter = InstanceHelper.EmitGetter<InstanceHelperTests, string>(nameof(Name));
        var value = getter(instance);
        Assert.Equal(name, value);
    }

    [Fact]
    public void EmitGetterName2()
    {
        string name = "Test2";
        var instance = new InstanceHelperTests { Name = name };
        var getter = InstanceHelper.EmitGetter<InstanceHelperTests, object>(nameof(Name));
        var value = getter(instance);
        Assert.Equal(name, value);
    }

    #endregion
    #region 读取可空属性PublishedAt的测试

    [Fact]
    public void EmitGetterPublishedAt1()
    {
        DateTime? publishedAt = DateTime.Now;
        var instance = new InstanceHelperTests { PublishedAt = publishedAt };
        var getter = InstanceHelper.EmitGetter<InstanceHelperTests, DateTime?>(nameof(PublishedAt));
        var value = getter(instance);
        Assert.Equal(publishedAt, value);
    }

    [Fact]
    public void EmitGetterPublishedAt2()
    {
        DateTime? publishedAt = DateTime.Now;
        var instance = new InstanceHelperTests { PublishedAt = publishedAt };
        var getter = InstanceHelper.EmitGetter<InstanceHelperTests, object>(nameof(PublishedAt));
        var value = getter(instance);
        Assert.Equal(publishedAt, value);
    }
    #endregion
    #endregion
    #region EmitSetter
    #region 写入结构体属性Id的测试
    [Fact]
    public void EmitSetterId1()
    {
        int id = 41;
        var instance = new InstanceHelperTests { Id = id };
        var setter = InstanceHelper.EmitSetter<InstanceHelperTests, int>(nameof(Id));
        setter(instance, id);
        Assert.Equal(id, instance.Id);
    }

    [Fact]
    public void EmitSetterId2()
    {
        int id = 42;
        var instance = new InstanceHelperTests { Id = id };
        var setter = InstanceHelper.EmitSetter<InstanceHelperTests, object>(nameof(Id));
        setter(instance, id);
        Assert.Equal(id, instance.Id);
    }
    #endregion
    #region 写入可空属性Name的测试
    [Fact]
    public void EmitSetterName01()
    {
        string? name = "Test1";
        var instance = new InstanceHelperTests { Name = name };
        var setter = InstanceHelper.EmitSetter<InstanceHelperTests, string?>(nameof(Name));
        setter(instance, name);
        Assert.Equal(name, instance.Name);
    }

    [Fact]
    public void EmitSetterName02()
    {
        string? name = "Test2";
        var instance = new InstanceHelperTests { Name = name };
        var setter = InstanceHelper.EmitSetter<InstanceHelperTests, object>(nameof(Name));
        setter(instance, name);
        Assert.Equal(name, instance.Name);
    }
    #endregion
    #region 写入字符串属性Name的测试
    [Fact]
    public void EmitSetterName1()
    {
        string name = "Test1";
        var instance = new InstanceHelperTests { Name = name };
        var setter = InstanceHelper.EmitSetter<InstanceHelperTests, string>(nameof(Name));
        setter(instance, name);
        Assert.Equal(name, instance.Name);
    }

    [Fact]
    public void EmitSetterName2()
    {
        string name = "Test2";
        var instance = new InstanceHelperTests { Name = name };
        var setter = InstanceHelper.EmitSetter<InstanceHelperTests, object>(nameof(Name));
        setter(instance, name);
        Assert.Equal(name, instance.Name);
    }
    #endregion
    #region 写入可空属性PublishedAt的测试
    [Fact]
    public void EmitSetterPublishedAt1()
    {
        DateTime? publishedAt = DateTime.Now;
        var instance = new InstanceHelperTests { PublishedAt = publishedAt };
        var setter = InstanceHelper.EmitSetter<InstanceHelperTests, DateTime?>(nameof(PublishedAt));
        setter(instance, publishedAt);
        Assert.Equal(publishedAt, instance.PublishedAt);
    }

    [Fact]
    public void EmitSetterPublishedAt2()
    {
        DateTime? publishedAt = DateTime.Now;
        var instance = new InstanceHelperTests { PublishedAt = publishedAt };
        var setter = InstanceHelper.EmitSetter<InstanceHelperTests, object>(nameof(PublishedAt));
        setter(instance, publishedAt);
        Assert.Equal(publishedAt, instance.PublishedAt);
    }
    #endregion
    #endregion
    #region Properties
    public int Id { get; set; }
    public string? Name = string.Empty;
    public DateTime? PublishedAt { get; set; }
    #endregion
}