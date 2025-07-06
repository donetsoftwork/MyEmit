using PocoEmit;
using System.Reflection;

namespace PocoEmitUnitTests;

public class InstanceFieldHelperTests
{
    #region EmitGetter
    #region 读取结构体属性Id的测试
    [Fact]
    public void EmitGetterId0()
    {
        var field = typeof(InstanceFieldHelperTests).GetField(nameof(Id), BindingFlags.Instance | BindingFlags.Public);
        int id = 40;
        var instance = new InstanceFieldHelperTests { Id = id };
        var getter = InstanceFieldHelper.EmitGetter<InstanceFieldHelperTests, int>(field);
        var value = getter(instance);
        Assert.Equal(id, value);
    }

    [Fact]
    public void EmitGetterId1()
    {
        int id = 41;
        var instance = new InstanceFieldHelperTests { Id = id };
        var getter = InstanceFieldHelper.EmitGetter<InstanceFieldHelperTests, int>(nameof(Id));
        var value = getter(instance);
        Assert.Equal(id, value);
    }

    [Fact]
    public void EmitGetterId2()
    {
        int id = 42;
        var instance = new InstanceFieldHelperTests { Id = id };
        var getter = InstanceFieldHelper.EmitGetter<InstanceFieldHelperTests, object>(nameof(Id));
        var value = getter(instance);
        Assert.Equal(id, value);
    }

    [Fact]
    public void EmitGetterId3()
    {
        int id = 43;
        var instance = new InstanceFieldHelperTests { Id = id };
        var field = typeof(InstanceFieldHelperTests).GetField(nameof(Id), BindingFlags.Instance | BindingFlags.Public);
        var getter = InstanceFieldHelper.EmitGetter<object, int>(field);
        var value = getter(instance);
        Assert.Equal(id, value);
    }

    [Fact]
    public void EmitGetterId4()
    {
        int id = 44;
        var instance = new InstanceFieldHelperTests { Id = id };
        var field = typeof(InstanceFieldHelperTests).GetField(nameof(Id), BindingFlags.Instance | BindingFlags.Public);
        var getter = InstanceFieldHelper.EmitGetter<object, object>(field);
        var value = getter(instance);
        Assert.Equal(id, value);
    }
    #endregion
    #region 读取可空属性Name的测试
    [Fact]
    public void EmitGetterName00()
    {
        var field = typeof(InstanceFieldHelperTests).GetField(nameof(Name), BindingFlags.Instance | BindingFlags.Public);
        string? name = "Test0";
        var instance = new InstanceFieldHelperTests { Name = name };
        var getter = InstanceFieldHelper.EmitGetter<InstanceFieldHelperTests, string?>(field);
        var value = getter(instance);
        Assert.Equal(name, value);
    }

    [Fact]
    public void EmitGetterName01()
    {
        string? name = "Test1";
        var instance = new InstanceFieldHelperTests { Name = name };
        var getter = InstanceFieldHelper.EmitGetter<InstanceFieldHelperTests, string?>(nameof(Name));
        var value = getter(instance);
        Assert.Equal(name, value);
    }

    [Fact]
    public void EmitGetterName02()
    {
        string? name = "Test2";
        var instance = new InstanceFieldHelperTests { Name = name };
        var getter = InstanceFieldHelper.EmitGetter<InstanceFieldHelperTests, object>(nameof(Name));
        var value = getter(instance);
        Assert.Equal(name, value);
    }

    [Fact]
    public void EmitGetterName03()
    {
        string? name = "Test3";
        var instance = new InstanceFieldHelperTests { Name = name };
        var field = typeof(InstanceFieldHelperTests).GetField(nameof(Name), BindingFlags.Instance | BindingFlags.Public);
        var getter = InstanceFieldHelper.EmitGetter<object, string?>(field);
        var value = getter(instance);
        Assert.Equal(name, value);
    }

    [Fact]
    public void EmitGetterName04()
    {
        string? name = "Test4";
        var instance = new InstanceFieldHelperTests { Name = name };
        var field = typeof(InstanceFieldHelperTests).GetField(nameof(Name), BindingFlags.Instance | BindingFlags.Public);
        var getter = InstanceFieldHelper.EmitGetter<object, object>(field);
        var value = getter(instance);
        object expected = name;
        Assert.Equal(expected, value);
    }
    #endregion
    #region 读取字符串属性Name的测试
    [Fact]
    public void EmitGetterName0()
    {
        var field = typeof(InstanceFieldHelperTests).GetField(nameof(Name), BindingFlags.Instance | BindingFlags.Public);
        string name = "Test0";
        var instance = new InstanceFieldHelperTests { Name = name };
        var getter = InstanceFieldHelper.EmitGetter<InstanceFieldHelperTests, string>(field);
        var value = getter(instance);
        Assert.Equal(name, value);
    }

    [Fact]
    public void EmitGetterName1()
    {
        string name = "Test1";
        var instance = new InstanceFieldHelperTests { Name = name };
        var getter = InstanceFieldHelper.EmitGetter<InstanceFieldHelperTests, string>(nameof(Name));
        var value = getter(instance);
        Assert.Equal(name, value);
    }

    [Fact]
    public void EmitGetterName2()
    {
        string name = "Test2";
        var instance = new InstanceFieldHelperTests { Name = name };
        var getter = InstanceFieldHelper.EmitGetter<InstanceFieldHelperTests, object>(nameof(Name));
        var value = getter(instance);
        Assert.Equal(name, value);
    }

    [Fact]
    public void EmitGetterName3()
    {
        string name = "Test3";
        var instance = new InstanceFieldHelperTests { Name = name };
        var field = typeof(InstanceFieldHelperTests).GetField(nameof(Name), BindingFlags.Instance | BindingFlags.Public);
        var getter = InstanceFieldHelper.EmitGetter<object, string>(field);
        var value = getter(instance);
        Assert.Equal(name, value);
    }

    [Fact]
    public void EmitGetterName4()
    {
        string name = "Test4";
        var instance = new InstanceFieldHelperTests { Name = name };
        var field = typeof(InstanceFieldHelperTests).GetField(nameof(Name), BindingFlags.Instance | BindingFlags.Public);
        var getter = InstanceFieldHelper.EmitGetter<object, object>(field);
        var value = getter(instance);
        object expected = name;
        Assert.Equal(expected, value);
    }
    #endregion
    #region 读取可空属性PublishedAt的测试
    [Fact]
    public void EmitGetterPublishedAt0()
    {
        var field = typeof(InstanceFieldHelperTests).GetField(nameof(PublishedAt), BindingFlags.Instance | BindingFlags.Public);
        DateTime? publishedAt = DateTime.Now;
        var instance = new InstanceFieldHelperTests { PublishedAt = publishedAt };
        var getter = InstanceFieldHelper.EmitGetter<InstanceFieldHelperTests, DateTime?>(field);
        var value = getter(instance);
        Assert.Equal(publishedAt, value);
    }

    [Fact]
    public void EmitGetterPublishedAt1()
    {
        DateTime? publishedAt = DateTime.Now;
        var instance = new InstanceFieldHelperTests { PublishedAt = publishedAt };
        var getter = InstanceFieldHelper.EmitGetter<InstanceFieldHelperTests, DateTime?>(nameof(PublishedAt));
        var value = getter(instance);
        Assert.Equal(publishedAt, value);
    }

    [Fact]
    public void EmitGetterPublishedAt2()
    {
        DateTime? publishedAt = DateTime.Now;
        var instance = new InstanceFieldHelperTests { PublishedAt = publishedAt };
        var getter = InstanceFieldHelper.EmitGetter<InstanceFieldHelperTests, object>(nameof(PublishedAt));
        var value = getter(instance);
        Assert.Equal(publishedAt, value);
    }

    [Fact]
    public void EmitGetterPublishedAt3()
    {
        DateTime? publishedAt = DateTime.Now;
        var instance = new InstanceFieldHelperTests { PublishedAt = publishedAt };
        var field = typeof(InstanceFieldHelperTests).GetField(nameof(PublishedAt), BindingFlags.Instance | BindingFlags.Public);
        var getter = InstanceFieldHelper.EmitGetter<object, DateTime?>(field);
        var value = getter(instance);
        Assert.Equal(publishedAt, value);
    }

    [Fact]
    public void EmitGetterPublishedAt4()
    {
        DateTime? publishedAt = DateTime.Now;
        var instance = new InstanceFieldHelperTests { PublishedAt = publishedAt };
        var field = typeof(InstanceFieldHelperTests).GetField(nameof(PublishedAt), BindingFlags.Instance | BindingFlags.Public);
        var getter = InstanceFieldHelper.EmitGetter<object, object>(field);
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
        var field = typeof(InstanceFieldHelperTests).GetField(nameof(Id), BindingFlags.Instance | BindingFlags.Public);
        int id = 40;
        var instance = new InstanceFieldHelperTests { Id = id };
        var setter = InstanceFieldHelper.EmitSetter<InstanceFieldHelperTests, int>(field);
        setter(instance, id);
        Assert.Equal(id, instance.Id);
    }

    [Fact]
    public void EmitSetterId1()
    {
        int id = 41;
        var instance = new InstanceFieldHelperTests { Id = id };
        var setter = InstanceFieldHelper.EmitSetter<InstanceFieldHelperTests, int>(nameof(Id));
        setter(instance, id);
        Assert.Equal(id, instance.Id);
    }

    [Fact]
    public void EmitSetterId2()
    {
        int id = 42;
        var instance = new InstanceFieldHelperTests { Id = id };
        var setter = InstanceFieldHelper.EmitSetter<InstanceFieldHelperTests, object>(nameof(Id));
        setter(instance, id);
        Assert.Equal(id, instance.Id);
    }

    [Fact]
    public void EmitSetterId3()
    {
        int id = 43;
        var instance = new InstanceFieldHelperTests { Id = id };
        var field = typeof(InstanceFieldHelperTests).GetField(nameof(Id), BindingFlags.Instance | BindingFlags.Public);
        var setter = InstanceFieldHelper.EmitSetter<object, int>(field);
        setter(instance, id);
        Assert.Equal(id, instance.Id);
    }

    [Fact]
    public void EmitSetterId4()
    {
        int id = 44;
        var instance = new InstanceFieldHelperTests { Id = id };
        var field = typeof(InstanceFieldHelperTests).GetField(nameof(Id), BindingFlags.Instance | BindingFlags.Public);
        var setter = InstanceFieldHelper.EmitSetter<object, object>(field);
        setter(instance, id);
        Assert.Equal(id, instance.Id);
    }
    #endregion
    #region 写入可空属性Name的测试
    [Fact]
    public void EmitSetterName00()
    {
        var field = typeof(InstanceFieldHelperTests).GetField(nameof(Name), BindingFlags.Instance | BindingFlags.Public);
        string? name = "Test0";
        var instance = new InstanceFieldHelperTests { Name = name };
        var setter = InstanceFieldHelper.EmitSetter<InstanceFieldHelperTests, string?>(field);
        setter(instance, name);
        Assert.Equal(name, instance.Name);
    }

    [Fact]
    public void EmitSetterName01()
    {
        string? name = "Test1";
        var instance = new InstanceFieldHelperTests { Name = name };
        var setter = InstanceFieldHelper.EmitSetter<InstanceFieldHelperTests, string?>(nameof(Name));
        setter(instance, name);
        Assert.Equal(name, instance.Name);
    }

    [Fact]
    public void EmitSetterName02()
    {
        string? name = "Test2";
        var instance = new InstanceFieldHelperTests { Name = name };
        var setter = InstanceFieldHelper.EmitSetter<InstanceFieldHelperTests, object>(nameof(Name));
        setter(instance, name);
        Assert.Equal(name, instance.Name);
    }

    [Fact]
    public void EmitSetterName03()
    {
        string? name = "Test3";
        var instance = new InstanceFieldHelperTests { Name = name };
        var field = typeof(InstanceFieldHelperTests).GetField(nameof(Name), BindingFlags.Instance | BindingFlags.Public);
        var setter = InstanceFieldHelper.EmitSetter<object, string?>(field);
        setter(instance, name);
        Assert.Equal(name, instance.Name);
    }

    [Fact]
    public void EmitSetterName04()
    {
        string? name = "Test4";
        var instance = new InstanceFieldHelperTests { Name = name };
        var field = typeof(InstanceFieldHelperTests).GetField(nameof(Name), BindingFlags.Instance | BindingFlags.Public);
        var setter = InstanceFieldHelper.EmitSetter<object, object>(field);
        setter(instance, name);
        Assert.Equal(name, instance.Name);
    }
    #endregion
    #region 写入字符串属性Name的测试
    [Fact]
    public void EmitSetterName0()
    {
        var field = typeof(InstanceFieldHelperTests).GetField(nameof(Name), BindingFlags.Instance | BindingFlags.Public);
        string name = "Test0";
        var instance = new InstanceFieldHelperTests { Name = name };
        var setter = InstanceFieldHelper.EmitSetter<InstanceFieldHelperTests, string>(field);
        setter(instance, name);
        Assert.Equal(name, instance.Name);
    }

    [Fact]
    public void EmitSetterName1()
    {
        string name = "Test1";
        var instance = new InstanceFieldHelperTests { Name = name };
        var setter = InstanceFieldHelper.EmitSetter<InstanceFieldHelperTests, string>(nameof(Name));
        setter(instance, name);
        Assert.Equal(name, instance.Name);
    }

    [Fact]
    public void EmitSetterName2()
    {
        string name = "Test2";
        var instance = new InstanceFieldHelperTests { Name = name };
        var setter = InstanceFieldHelper.EmitSetter<InstanceFieldHelperTests, object>(nameof(Name));
        setter(instance, name);
        Assert.Equal(name, instance.Name);
    }

    [Fact]
    public void EmitSetterName3()
    {
        string name = "Test3";
        var instance = new InstanceFieldHelperTests { Name = name };
        var field = typeof(InstanceFieldHelperTests).GetField(nameof(Name), BindingFlags.Instance | BindingFlags.Public);
        var setter = InstanceFieldHelper.EmitSetter<object, string>(field);
        setter(instance, name);
        Assert.Equal(name, instance.Name);
    }

    [Fact]
    public void EmitSetterName4()
    {
        string name = "Test4";
        var instance = new InstanceFieldHelperTests { Name = name };
        var field = typeof(InstanceFieldHelperTests).GetField(nameof(Name), BindingFlags.Instance | BindingFlags.Public);
        var setter = InstanceFieldHelper.EmitSetter<object, object>(field);
        setter(instance, name);
        Assert.Equal(name, instance.Name);
    }
    #endregion
    #region 写入可空属性PublishedAt的测试
    [Fact]
    public void EmitSetterPublishedAt0()
    {
        var field = typeof(InstanceFieldHelperTests).GetField(nameof(PublishedAt), BindingFlags.Instance | BindingFlags.Public);
        DateTime? publishedAt = DateTime.Now;
        var instance = new InstanceFieldHelperTests { PublishedAt = publishedAt };
        var setter = InstanceFieldHelper.EmitSetter<InstanceFieldHelperTests, DateTime?>(field);
        setter(instance, publishedAt);
        Assert.Equal(publishedAt, instance.PublishedAt);
    }

    [Fact]
    public void EmitSetterPublishedAt1()
    {
        DateTime? publishedAt = DateTime.Now;
        var instance = new InstanceFieldHelperTests { PublishedAt = publishedAt };
        var setter = InstanceFieldHelper.EmitSetter<InstanceFieldHelperTests, DateTime?>(nameof(PublishedAt));
        setter(instance, publishedAt);
        Assert.Equal(publishedAt, instance.PublishedAt);
    }

    [Fact]
    public void EmitSetterPublishedAt2()
    {
        DateTime? publishedAt = DateTime.Now;
        var instance = new InstanceFieldHelperTests { PublishedAt = publishedAt };
        var setter = InstanceFieldHelper.EmitSetter<InstanceFieldHelperTests, object>(nameof(PublishedAt));
        setter(instance, publishedAt);
        Assert.Equal(publishedAt, instance.PublishedAt);
    }

    [Fact]
    public void EmitSetterPublishedAt3()
    {
        DateTime? publishedAt = DateTime.Now;
        var instance = new InstanceFieldHelperTests { PublishedAt = publishedAt };
        var field = typeof(InstanceFieldHelperTests).GetField(nameof(PublishedAt), BindingFlags.Instance | BindingFlags.Public);
        var setter = InstanceFieldHelper.EmitSetter<object, DateTime?>(field);
        setter(instance, publishedAt);
        Assert.Equal(publishedAt, instance.PublishedAt);
    }

    [Fact]
    public void EmitSetterPublishedAt4()
    {
        DateTime? publishedAt = DateTime.Now;
        var instance = new InstanceFieldHelperTests { PublishedAt = publishedAt };
        var field = typeof(InstanceFieldHelperTests).GetField(nameof(PublishedAt), BindingFlags.Instance | BindingFlags.Public);
        var setter = InstanceFieldHelper.EmitSetter<object, object>(field);
        setter(instance, publishedAt);
        Assert.Equal(publishedAt, instance.PublishedAt);
    }
    #endregion
    #endregion
    #region Fields
    public int Id;
    public string? Name;
    public DateTime? PublishedAt;
    #endregion
}
