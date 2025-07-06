using PocoEmit;
using System.Reflection;

namespace PocoEmitUnitTests;

public class InstancePropertyHelperTests
{
    #region EmitGetter
    #region 读取结构体属性Id的测试
    [Fact]
    public void EmitGetterId0()
    {
        var property = typeof(InstancePropertyHelperTests).GetProperty(nameof(Id), BindingFlags.Instance | BindingFlags.Public);
        int id = 40;
        var instance = new InstancePropertyHelperTests { Id = id };
        var getter = InstancePropertyHelper.EmitGetter<InstancePropertyHelperTests, int>(property);
        var value = getter(instance);
        Assert.Equal(id, value);
    }

    [Fact]
    public void EmitGetterId1()
    {
        int id = 41;
        var instance = new InstancePropertyHelperTests { Id = id };
        var getter = InstancePropertyHelper.EmitGetter<InstancePropertyHelperTests, int>(nameof(Id));
        var value = getter(instance);
        Assert.Equal(id, value);
    }

    [Fact]
    public void EmitGetterId2()
    {
        int id = 42;
        var instance = new InstancePropertyHelperTests { Id = id };
        var getter = InstancePropertyHelper.EmitGetter<InstancePropertyHelperTests, object>(nameof(Id));
        var value = getter(instance);
        Assert.Equal(id, value);
    }

    [Fact]
    public void EmitGetterId3()
    {
        int id = 43;
        var instance = new InstancePropertyHelperTests { Id = id };
        var property = typeof(InstancePropertyHelperTests).GetProperty(nameof(Id), BindingFlags.Instance | BindingFlags.Public);
        var getter = InstancePropertyHelper.EmitGetter<object, int>(property);
        var value = getter(instance);
        Assert.Equal(id, value);
    }

    [Fact]
    public void EmitGetterId4()
    {
        int id = 44;
        var instance = new InstancePropertyHelperTests { Id = id };
        var property = typeof(InstancePropertyHelperTests).GetProperty(nameof(Id), BindingFlags.Instance | BindingFlags.Public);
        var getter = InstancePropertyHelper.EmitGetter<object, object>(property);
        var value = getter(instance);
        Assert.Equal(id, value);
    }
    #endregion
    #region 读取可空属性Name的测试
    [Fact]
    public void EmitGetterName00()
    {
        var property = typeof(InstancePropertyHelperTests).GetProperty(nameof(Name), BindingFlags.Instance | BindingFlags.Public);
        string? name = "Test0";
        var instance = new InstancePropertyHelperTests { Name = name };
        var getter = InstancePropertyHelper.EmitGetter<InstancePropertyHelperTests, string?>(property);
        var value = getter(instance);
        Assert.Equal(name, value);
    }

    [Fact]
    public void EmitGetterName01()
    {
        string? name = "Test1";
        var instance = new InstancePropertyHelperTests { Name = name };
        var getter = InstancePropertyHelper.EmitGetter<InstancePropertyHelperTests, string?>(nameof(Name));
        var value = getter(instance);
        Assert.Equal(name, value);
    }

    [Fact]
    public void EmitGetterName02()
    {
        string? name = "Test2";
        var instance = new InstancePropertyHelperTests { Name = name };
        var getter = InstancePropertyHelper.EmitGetter<InstancePropertyHelperTests, object>(nameof(Name));
        var value = getter(instance);
        Assert.Equal(name, value);
    }

    [Fact]
    public void EmitGetterName03()
    {
        string? name = "Test3";
        var instance = new InstancePropertyHelperTests { Name = name };
        var property = typeof(InstancePropertyHelperTests).GetProperty(nameof(Name), BindingFlags.Instance | BindingFlags.Public);
        var getter = InstancePropertyHelper.EmitGetter<object, string?>(property);
        var value = getter(instance);
        Assert.Equal(name, value);
    }

    [Fact]
    public void EmitGetterName04()
    {
        string? name = "Test4";
        var instance = new InstancePropertyHelperTests { Name = name };
        var property = typeof(InstancePropertyHelperTests).GetProperty(nameof(Name), BindingFlags.Instance | BindingFlags.Public);
        var getter = InstancePropertyHelper.EmitGetter<object, object>(property);
        var value = getter(instance);
        object expected = name;
        Assert.Equal(expected, value);
    }
    #endregion
    #region 读取字符串属性Name的测试
    [Fact]
    public void EmitGetterName0()
    {
        var property = typeof(InstancePropertyHelperTests).GetProperty(nameof(Name), BindingFlags.Instance | BindingFlags.Public);
        string name = "Test0";
        var instance = new InstancePropertyHelperTests { Name = name };
        var getter = InstancePropertyHelper.EmitGetter<InstancePropertyHelperTests, string>(property);
        var value = getter(instance);
        Assert.Equal(name, value);
    }

    [Fact]
    public void EmitGetterName1()
    {
        string name = "Test1";
        var instance = new InstancePropertyHelperTests { Name = name };
        var getter = InstancePropertyHelper.EmitGetter<InstancePropertyHelperTests, string>(nameof(Name));
        var value = getter(instance);
        Assert.Equal(name, value);
    }

    [Fact]
    public void EmitGetterName2()
    {
        string name = "Test2";
        var instance = new InstancePropertyHelperTests { Name = name };
        var getter = InstancePropertyHelper.EmitGetter<InstancePropertyHelperTests, object>(nameof(Name));
        var value = getter(instance);
        Assert.Equal(name, value);
    }

    [Fact]
    public void EmitGetterName3()
    {
        string name = "Test3";
        var instance = new InstancePropertyHelperTests { Name = name };
        var property = typeof(InstancePropertyHelperTests).GetProperty(nameof(Name), BindingFlags.Instance | BindingFlags.Public);
        var getter = InstancePropertyHelper.EmitGetter<object, string>(property);
        var value = getter(instance);
        Assert.Equal(name, value);
    }

    [Fact]
    public void EmitGetterName4()
    {
        string name = "Test4";
        var instance = new InstancePropertyHelperTests { Name = name };
        var property = typeof(InstancePropertyHelperTests).GetProperty(nameof(Name), BindingFlags.Instance | BindingFlags.Public);
        var getter = InstancePropertyHelper.EmitGetter<object, object>(property);
        var value = getter(instance);
        object expected = name;
        Assert.Equal(expected, value);
    }
    #endregion
    #region 读取可空属性PublishedAt的测试
    [Fact]
    public void EmitGetterPublishedAt0()
    {
        var property = typeof(InstancePropertyHelperTests).GetProperty(nameof(PublishedAt), BindingFlags.Instance | BindingFlags.Public);
        DateTime? publishedAt = DateTime.Now;
        var instance = new InstancePropertyHelperTests { PublishedAt = publishedAt };
        var getter = InstancePropertyHelper.EmitGetter<InstancePropertyHelperTests, DateTime?>(property);
        var value = getter(instance);
        Assert.Equal(publishedAt, value);
    }

    [Fact]
    public void EmitGetterPublishedAt1()
    {
        DateTime? publishedAt = DateTime.Now;
        var instance = new InstancePropertyHelperTests { PublishedAt = publishedAt };
        var getter = InstancePropertyHelper.EmitGetter<InstancePropertyHelperTests, DateTime?>(nameof(PublishedAt));
        var value = getter(instance);
        Assert.Equal(publishedAt, value);
    }

    [Fact]
    public void EmitGetterPublishedAt2()
    {
        DateTime? publishedAt = DateTime.Now;
        var instance = new InstancePropertyHelperTests { PublishedAt = publishedAt };
        var getter = InstancePropertyHelper.EmitGetter<InstancePropertyHelperTests, object>(nameof(PublishedAt));
        var value = getter(instance);
        Assert.Equal(publishedAt, value);
    }

    [Fact]
    public void EmitGetterPublishedAt3()
    {
        DateTime? publishedAt = DateTime.Now;
        var instance = new InstancePropertyHelperTests { PublishedAt = publishedAt };
        var property = typeof(InstancePropertyHelperTests).GetProperty(nameof(PublishedAt), BindingFlags.Instance | BindingFlags.Public);
        var getter = InstancePropertyHelper.EmitGetter<object, DateTime?>(property);
        var value = getter(instance);
        Assert.Equal(publishedAt, value);
    }

    [Fact]
    public void EmitGetterPublishedAt4()
    {
        DateTime? publishedAt = DateTime.Now;
        var instance = new InstancePropertyHelperTests { PublishedAt = publishedAt };
        var property = typeof(InstancePropertyHelperTests).GetProperty(nameof(PublishedAt), BindingFlags.Instance | BindingFlags.Public);
        var getter = InstancePropertyHelper.EmitGetter<object, object>(property);
        var value = getter(instance);
        object expected = publishedAt;
        Assert.Equal(expected, value);
    }
    #endregion
    #endregion
    #region EmitSetter
    #region 写入结构体属性Id的测试
    [Fact]
    public void EmitSetterId0()
    {
        var property = typeof(InstancePropertyHelperTests).GetProperty(nameof(Id), BindingFlags.Instance | BindingFlags.Public);
        int id = 40;
        var instance = new InstancePropertyHelperTests { Id = id };
        var setter = InstancePropertyHelper.EmitSetter<InstancePropertyHelperTests, int>(property);
        setter(instance, id);
        Assert.Equal(id, instance.Id);
    }

    [Fact]
    public void EmitSetterId1()
    {
        int id = 41;
        var instance = new InstancePropertyHelperTests { Id = id };
        var setter = InstancePropertyHelper.EmitSetter<InstancePropertyHelperTests, int>(nameof(Id));
        setter(instance, id);
        Assert.Equal(id, instance.Id);
    }

    [Fact]
    public void EmitSetterId2()
    {
        int id = 42;
        var instance = new InstancePropertyHelperTests { Id = id };
        var setter = InstancePropertyHelper.EmitSetter<InstancePropertyHelperTests, object>(nameof(Id));
        setter(instance, id);
        Assert.Equal(id, instance.Id);
    }

    [Fact]
    public void EmitSetterId3()
    {
        int id = 43;
        var instance = new InstancePropertyHelperTests { Id = id };
        var property = typeof(InstancePropertyHelperTests).GetProperty(nameof(Id), BindingFlags.Instance | BindingFlags.Public);
        var setter = InstancePropertyHelper.EmitSetter<object, int>(property);
        setter(instance, id);
        Assert.Equal(id, instance.Id);
    }

    [Fact]
    public void EmitSetterId4()
    {
        int id = 44;
        var instance = new InstancePropertyHelperTests { Id = id };
        var property = typeof(InstancePropertyHelperTests).GetProperty(nameof(Id), BindingFlags.Instance | BindingFlags.Public);
        var setter = InstancePropertyHelper.EmitSetter<object, object>(property);
        setter(instance, id);
        Assert.Equal(id, instance.Id);
    }
    #endregion
    #region 写入可空属性Name的测试
    [Fact]
    public void EmitSetterName00()
    {
        var property = typeof(InstancePropertyHelperTests).GetProperty(nameof(Name), BindingFlags.Instance | BindingFlags.Public);
        string? name = "Test0";
        var instance = new InstancePropertyHelperTests { Name = name };
        var setter = InstancePropertyHelper.EmitSetter<InstancePropertyHelperTests, string?>(property);
        setter(instance, name);
        Assert.Equal(name, instance.Name);
    }

    [Fact]
    public void EmitSetterName01()
    {
        string? name = "Test1";
        var instance = new InstancePropertyHelperTests { Name = name };
        var setter = InstancePropertyHelper.EmitSetter<InstancePropertyHelperTests, string?>(nameof(Name));
        setter(instance, name);
        Assert.Equal(name, instance.Name);
    }

    [Fact]
    public void EmitSetterName02()
    {
        string? name = "Test2";
        var instance = new InstancePropertyHelperTests { Name = name };
        var setter = InstancePropertyHelper.EmitSetter<InstancePropertyHelperTests, object>(nameof(Name));
        setter(instance, name);
        Assert.Equal(name, instance.Name);
    }

    [Fact]
    public void EmitSetterName03()
    {
        string? name = "Test3";
        var instance = new InstancePropertyHelperTests { Name = name };
        var property = typeof(InstancePropertyHelperTests).GetProperty(nameof(Name), BindingFlags.Instance | BindingFlags.Public);
        var setter = InstancePropertyHelper.EmitSetter<object, string?>(property);
        setter(instance, name);
        Assert.Equal(name, instance.Name);
    }

    [Fact]
    public void EmitSetterName04()
    {
        string? name = "Test4";
        var instance = new InstancePropertyHelperTests { Name = name };
        var property = typeof(InstancePropertyHelperTests).GetProperty(nameof(Name), BindingFlags.Instance | BindingFlags.Public);
        var setter = InstancePropertyHelper.EmitSetter<object, object>(property);
        setter(instance, name);
        Assert.Equal(name, instance.Name);
    }
    #endregion
    #region 写入字符串属性Name的测试
    [Fact]
    public void EmitSetterName0()
    {
        var property = typeof(InstancePropertyHelperTests).GetProperty(nameof(Name), BindingFlags.Instance | BindingFlags.Public);
        string name = "Test0";
        var instance = new InstancePropertyHelperTests { Name = name };
        var setter = InstancePropertyHelper.EmitSetter<InstancePropertyHelperTests, string>(property);
        setter(instance, name);
        Assert.Equal(name, instance.Name);
    }

    [Fact]
    public void EmitSetterName1()
    {
        string name = "Test1";
        var instance = new InstancePropertyHelperTests { Name = name };
        var setter = InstancePropertyHelper.EmitSetter<InstancePropertyHelperTests, string>(nameof(Name));
        setter(instance, name);
        Assert.Equal(name, instance.Name);
    }

    [Fact]
    public void EmitSetterName2()
    {
        string name = "Test2";
        var instance = new InstancePropertyHelperTests { Name = name };
        var setter = InstancePropertyHelper.EmitSetter<InstancePropertyHelperTests, object>(nameof(Name));
        setter(instance, name);
        Assert.Equal(name, instance.Name);
    }

    [Fact]
    public void EmitSetterName3()
    {
        string name = "Test3";
        var instance = new InstancePropertyHelperTests { Name = name };
        var property = typeof(InstancePropertyHelperTests).GetProperty(nameof(Name), BindingFlags.Instance | BindingFlags.Public);
        var setter = InstancePropertyHelper.EmitSetter<object, string>(property);
        setter(instance, name);
        Assert.Equal(name, instance.Name);
    }

    [Fact]
    public void EmitSetterName4()
    {
        string name = "Test4";
        var instance = new InstancePropertyHelperTests { Name = name };
        var property = typeof(InstancePropertyHelperTests).GetProperty(nameof(Name), BindingFlags.Instance | BindingFlags.Public);
        var setter = InstancePropertyHelper.EmitSetter<object, object>(property);
        setter(instance, name);
        Assert.Equal(name, instance.Name);
    }
    #endregion
    #region 写入可空属性PublishedAt的测试
    [Fact]
    public void EmitSetterPublishedAt0()
    {
        var property = typeof(InstancePropertyHelperTests).GetProperty(nameof(PublishedAt), BindingFlags.Instance | BindingFlags.Public);
        DateTime? publishedAt = DateTime.Now;
        var instance = new InstancePropertyHelperTests { PublishedAt = publishedAt };
        var setter = InstancePropertyHelper.EmitSetter<InstancePropertyHelperTests, DateTime?>(property);
        setter(instance, publishedAt);
        Assert.Equal(publishedAt, instance.PublishedAt);
    }

    [Fact]
    public void EmitSetterPublishedAt1()
    {
        DateTime? publishedAt = DateTime.Now;
        var instance = new InstancePropertyHelperTests { PublishedAt = publishedAt };
        var setter = InstancePropertyHelper.EmitSetter<InstancePropertyHelperTests, DateTime?>(nameof(PublishedAt));
        setter(instance, publishedAt);
        Assert.Equal(publishedAt, instance.PublishedAt);
    }

    [Fact]
    public void EmitSetterPublishedAt2()
    {
        DateTime? publishedAt = DateTime.Now;
        var instance = new InstancePropertyHelperTests { PublishedAt = publishedAt };
        var setter = InstancePropertyHelper.EmitSetter<InstancePropertyHelperTests, object>(nameof(PublishedAt));
        setter(instance, publishedAt);
        Assert.Equal(publishedAt, instance.PublishedAt);
    }

    [Fact]
    public void EmitSetterPublishedAt3()
    {
        DateTime? publishedAt = DateTime.Now;
        var instance = new InstancePropertyHelperTests { PublishedAt = publishedAt };
        var property = typeof(InstancePropertyHelperTests).GetProperty(nameof(PublishedAt), BindingFlags.Instance | BindingFlags.Public);
        var setter = InstancePropertyHelper.EmitSetter<object, DateTime?>(property);
        setter(instance, publishedAt);
        Assert.Equal(publishedAt, instance.PublishedAt);
    }

    [Fact]
    public void EmitSetterPublishedAt4()
    {
        DateTime? publishedAt = DateTime.Now;
        var instance = new InstancePropertyHelperTests { PublishedAt = publishedAt };
        var property = typeof(InstancePropertyHelperTests).GetProperty(nameof(PublishedAt), BindingFlags.Instance | BindingFlags.Public);
        var setter = InstancePropertyHelper.EmitSetter<object, object>(property);
        setter(instance, publishedAt);
        Assert.Equal(publishedAt, instance.PublishedAt);
    }
    #endregion
    #endregion
    #region Properties
    public int Id { get; set; }
    public string? Name { get; set; } = string.Empty;
    public DateTime? PublishedAt { get; set; }
    #endregion
}