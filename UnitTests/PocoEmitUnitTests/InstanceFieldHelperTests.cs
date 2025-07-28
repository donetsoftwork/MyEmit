using PocoEmit;
using System.Reflection;

namespace PocoEmitUnitTests;

public class InstanceFieldHelperTests
{
    #region GetReadFunc
    #region 读取结构体属性Id的测试
    [Fact]
    public void GetReadFuncId0()
    {
        var field = typeof(InstanceFieldHelperTests).GetField(nameof(Id), BindingFlags.Instance | BindingFlags.Public);
        int id = 40;
        var instance = new InstanceFieldHelperTests { Id = id };
        var getter = InstanceHelper.GetReadFunc<InstanceFieldHelperTests, int>(field);
        var value = getter(instance);
        Assert.Equal(id, value);
    }

    [Fact]
    public void GetReadFuncId1()
    {
        int id = 41;
        var instance = new InstanceFieldHelperTests { Id = id };
        var getter = InstanceHelper.GetReadFunc<InstanceFieldHelperTests, int>(nameof(Id));
        var value = getter(instance);
        Assert.Equal(id, value);
    }

    [Fact]
    public void GetReadFuncId2()
    {
        int id = 42;
        var instance = new InstanceFieldHelperTests { Id = id };
        var getter = InstanceHelper.GetReadFunc<InstanceFieldHelperTests, object>(nameof(Id));
        var value = getter(instance);
        Assert.Equal(id, value);
    }

    [Fact]
    public void GetReadFuncId3()
    {
        int id = 43;
        var instance = new InstanceFieldHelperTests { Id = id };
        var field = typeof(InstanceFieldHelperTests).GetField(nameof(Id), BindingFlags.Instance | BindingFlags.Public);
        var getter = InstanceHelper.GetReadFunc<object, int>(field);
        var value = getter(instance);
        Assert.Equal(id, value);
    }

    [Fact]
    public void GetReadFuncId4()
    {
        int id = 44;
        var instance = new InstanceFieldHelperTests { Id = id };
        var field = typeof(InstanceFieldHelperTests).GetField(nameof(Id), BindingFlags.Instance | BindingFlags.Public);
        var getter = InstanceHelper.GetReadFunc<object, object>(field);
        var value = getter(instance);
        Assert.Equal(id, value);
    }
    #endregion
    #region 读取可空属性Name的测试
    [Fact]
    public void GetReadFuncName00()
    {
        var field = typeof(InstanceFieldHelperTests).GetField(nameof(Name), BindingFlags.Instance | BindingFlags.Public);
        string? name = "Test0";
        var instance = new InstanceFieldHelperTests { Name = name };
        var getter = InstanceHelper.GetReadFunc<InstanceFieldHelperTests, string?>(field);
        var value = getter(instance);
        Assert.Equal(name, value);
    }

    [Fact]
    public void GetReadFuncName01()
    {
        string? name = "Test1";
        var instance = new InstanceFieldHelperTests { Name = name };
        var getter = InstanceHelper.GetReadFunc<InstanceFieldHelperTests, string?>(nameof(Name));
        var value = getter(instance);
        Assert.Equal(name, value);
    }

    [Fact]
    public void GetReadFuncName02()
    {
        string? name = "Test2";
        var instance = new InstanceFieldHelperTests { Name = name };
        var getter = InstanceHelper.GetReadFunc<InstanceFieldHelperTests, object>(nameof(Name));
        var value = getter(instance);
        Assert.Equal(name, value);
    }

    [Fact]
    public void GetReadFuncName03()
    {
        string? name = "Test3";
        var instance = new InstanceFieldHelperTests { Name = name };
        var field = typeof(InstanceFieldHelperTests).GetField(nameof(Name), BindingFlags.Instance | BindingFlags.Public);
        var getter = InstanceHelper.GetReadFunc<object, string?>(field);
        var value = getter(instance);
        Assert.Equal(name, value);
    }

    [Fact]
    public void GetReadFuncName04()
    {
        string? name = "Test4";
        var instance = new InstanceFieldHelperTests { Name = name };
        var field = typeof(InstanceFieldHelperTests).GetField(nameof(Name), BindingFlags.Instance | BindingFlags.Public);
        var getter = InstanceHelper.GetReadFunc<object, object>(field);
        var value = getter(instance);
        object expected = name;
        Assert.Equal(expected, value);
    }
    #endregion
    #region 读取字符串属性Name的测试
    [Fact]
    public void GetReadFuncName0()
    {
        var field = typeof(InstanceFieldHelperTests).GetField(nameof(Name), BindingFlags.Instance | BindingFlags.Public);
        string name = "Test0";
        var instance = new InstanceFieldHelperTests { Name = name };
        var getter = InstanceHelper.GetReadFunc<InstanceFieldHelperTests, string>(field);
        var value = getter(instance);
        Assert.Equal(name, value);
    }

    [Fact]
    public void GetReadFuncName1()
    {
        string name = "Test1";
        var instance = new InstanceFieldHelperTests { Name = name };
        var getter = InstanceHelper.GetReadFunc<InstanceFieldHelperTests, string>(nameof(Name));
        var value = getter(instance);
        Assert.Equal(name, value);
    }

    [Fact]
    public void GetReadFuncName2()
    {
        string name = "Test2";
        var instance = new InstanceFieldHelperTests { Name = name };
        var getter = InstanceHelper.GetReadFunc<InstanceFieldHelperTests, object>(nameof(Name));
        var value = getter(instance);
        Assert.Equal(name, value);
    }

    [Fact]
    public void GetReadFuncName3()
    {
        string name = "Test3";
        var instance = new InstanceFieldHelperTests { Name = name };
        var field = typeof(InstanceFieldHelperTests).GetField(nameof(Name), BindingFlags.Instance | BindingFlags.Public);
        var getter = InstanceHelper.GetReadFunc<object, string>(field);
        var value = getter(instance);
        Assert.Equal(name, value);
    }

    [Fact]
    public void GetReadFuncName4()
    {
        string name = "Test4";
        var instance = new InstanceFieldHelperTests { Name = name };
        var field = typeof(InstanceFieldHelperTests).GetField(nameof(Name), BindingFlags.Instance | BindingFlags.Public);
        var getter = InstanceHelper.GetReadFunc<object, object>(field);
        var value = getter(instance);
        object expected = name;
        Assert.Equal(expected, value);
    }
    #endregion
    #region 读取可空属性PublishedAt的测试
    [Fact]
    public void GetReadFuncPublishedAt0()
    {
        var field = typeof(InstanceFieldHelperTests).GetField(nameof(PublishedAt), BindingFlags.Instance | BindingFlags.Public);
        DateTime? publishedAt = DateTime.Now;
        var instance = new InstanceFieldHelperTests { PublishedAt = publishedAt };
        var getter = InstanceHelper.GetReadFunc<InstanceFieldHelperTests, DateTime?>(field);
        var value = getter(instance);
        Assert.Equal(publishedAt, value);
    }

    [Fact]
    public void GetReadFuncPublishedAt1()
    {
        DateTime? publishedAt = DateTime.Now;
        var instance = new InstanceFieldHelperTests { PublishedAt = publishedAt };
        var getter = InstanceHelper.GetReadFunc<InstanceFieldHelperTests, DateTime?>(nameof(PublishedAt));
        var value = getter(instance);
        Assert.Equal(publishedAt, value);
    }

    [Fact]
    public void GetReadFuncPublishedAt2()
    {
        DateTime? publishedAt = DateTime.Now;
        var instance = new InstanceFieldHelperTests { PublishedAt = publishedAt };
        var getter = InstanceHelper.GetReadFunc<InstanceFieldHelperTests, object>(nameof(PublishedAt));
        var value = getter(instance);
        Assert.Equal(publishedAt, value);
    }

    [Fact]
    public void GetReadFuncPublishedAt3()
    {
        DateTime? publishedAt = DateTime.Now;
        var instance = new InstanceFieldHelperTests { PublishedAt = publishedAt };
        var field = typeof(InstanceFieldHelperTests).GetField(nameof(PublishedAt), BindingFlags.Instance | BindingFlags.Public);
        var getter = InstanceHelper.GetReadFunc<object, DateTime?>(field);
        var value = getter(instance);
        Assert.Equal(publishedAt, value);
    }

    [Fact]
    public void GetReadFuncPublishedAt4()
    {
        DateTime? publishedAt = DateTime.Now;
        var instance = new InstanceFieldHelperTests { PublishedAt = publishedAt };
        var field = typeof(InstanceFieldHelperTests).GetField(nameof(PublishedAt), BindingFlags.Instance | BindingFlags.Public);
        var getter = InstanceHelper.GetReadFunc<object, object>(field);
        var value = getter(instance);
        object expected = publishedAt;
        Assert.Equal(expected, value);
    }
    #endregion
    #endregion
    #region GetWriteAction
    #region 写入结构体属性Id的测试
    [Fact]
    public void GetWriteActionId0()
    {
        var field = typeof(InstanceFieldHelperTests).GetField(nameof(Id), BindingFlags.Instance | BindingFlags.Public);
        int id = 40;
        var instance = new InstanceFieldHelperTests { Id = id };
        var setter = InstanceHelper.GetWriteAction<InstanceFieldHelperTests, int>(field);
        setter(instance, id);
        Assert.Equal(id, instance.Id);
    }

    [Fact]
    public void GetWriteActionId1()
    {
        int id = 41;
        var instance = new InstanceFieldHelperTests { Id = id };
        var setter = InstanceHelper.GetWriteAction<InstanceFieldHelperTests, int>(nameof(Id));
        setter(instance, id);
        Assert.Equal(id, instance.Id);
    }

    [Fact]
    public void GetWriteActionId2()
    {
        int id = 42;
        var instance = new InstanceFieldHelperTests { Id = id };
        var setter = InstanceHelper.GetWriteAction<InstanceFieldHelperTests, object>(nameof(Id));
        setter(instance, id);
        Assert.Equal(id, instance.Id);
    }

    [Fact]
    public void GetWriteActionId3()
    {
        int id = 43;
        var instance = new InstanceFieldHelperTests { Id = id };
        var field = typeof(InstanceFieldHelperTests).GetField(nameof(Id), BindingFlags.Instance | BindingFlags.Public);
        var setter = InstanceHelper.GetWriteAction<object, int>(field);
        setter(instance, id);
        Assert.Equal(id, instance.Id);
    }

    [Fact]
    public void GetWriteActionId4()
    {
        int id = 44;
        var instance = new InstanceFieldHelperTests { Id = id };
        var field = typeof(InstanceFieldHelperTests).GetField(nameof(Id), BindingFlags.Instance | BindingFlags.Public);
        var setter = InstanceHelper.GetWriteAction<object, object>(field);
        setter(instance, id);
        Assert.Equal(id, instance.Id);
    }
    #endregion
    #region 写入可空属性Name的测试
    [Fact]
    public void GetWriteActionName00()
    {
        var field = typeof(InstanceFieldHelperTests).GetField(nameof(Name), BindingFlags.Instance | BindingFlags.Public);
        string? name = "Test0";
        var instance = new InstanceFieldHelperTests { Name = name };
        var setter = InstanceHelper.GetWriteAction<InstanceFieldHelperTests, string?>(field);
        setter(instance, name);
        Assert.Equal(name, instance.Name);
    }

    [Fact]
    public void GetWriteActionName01()
    {
        string? name = "Test1";
        var instance = new InstanceFieldHelperTests { Name = name };
        var setter = InstanceHelper.GetWriteAction<InstanceFieldHelperTests, string?>(nameof(Name));
        setter(instance, name);
        Assert.Equal(name, instance.Name);
    }

    [Fact]
    public void GetWriteActionName02()
    {
        string? name = "Test2";
        var instance = new InstanceFieldHelperTests { Name = name };
        var setter = InstanceHelper.GetWriteAction<InstanceFieldHelperTests, object>(nameof(Name));
        setter(instance, name);
        Assert.Equal(name, instance.Name);
    }

    [Fact]
    public void GetWriteActionName03()
    {
        string? name = "Test3";
        var instance = new InstanceFieldHelperTests { Name = name };
        var field = typeof(InstanceFieldHelperTests).GetField(nameof(Name), BindingFlags.Instance | BindingFlags.Public);
        var setter = InstanceHelper.GetWriteAction<object, string?>(field);
        setter(instance, name);
        Assert.Equal(name, instance.Name);
    }

    [Fact]
    public void GetWriteActionName04()
    {
        string? name = "Test4";
        var instance = new InstanceFieldHelperTests { Name = name };
        var field = typeof(InstanceFieldHelperTests).GetField(nameof(Name), BindingFlags.Instance | BindingFlags.Public);
        var setter = InstanceHelper.GetWriteAction<object, object>(field);
        setter(instance, name);
        Assert.Equal(name, instance.Name);
    }
    #endregion
    #region 写入字符串属性Name的测试
    [Fact]
    public void GetWriteActionName0()
    {
        var field = typeof(InstanceFieldHelperTests).GetField(nameof(Name), BindingFlags.Instance | BindingFlags.Public);
        string name = "Test0";
        var instance = new InstanceFieldHelperTests { Name = name };
        var setter = InstanceHelper.GetWriteAction<InstanceFieldHelperTests, string>(field);
        setter(instance, name);
        Assert.Equal(name, instance.Name);
    }

    [Fact]
    public void GetWriteActionName1()
    {
        string name = "Test1";
        var instance = new InstanceFieldHelperTests { Name = name };
        var setter = InstanceHelper.GetWriteAction<InstanceFieldHelperTests, string>(nameof(Name));
        setter(instance, name);
        Assert.Equal(name, instance.Name);
    }

    [Fact]
    public void GetWriteActionName2()
    {
        string name = "Test2";
        var instance = new InstanceFieldHelperTests { Name = name };
        var setter = InstanceHelper.GetWriteAction<InstanceFieldHelperTests, object>(nameof(Name));
        setter(instance, name);
        Assert.Equal(name, instance.Name);
    }

    [Fact]
    public void GetWriteActionName3()
    {
        string name = "Test3";
        var instance = new InstanceFieldHelperTests { Name = name };
        var field = typeof(InstanceFieldHelperTests).GetField(nameof(Name), BindingFlags.Instance | BindingFlags.Public);
        var setter = InstanceHelper.GetWriteAction<object, string>(field);
        setter(instance, name);
        Assert.Equal(name, instance.Name);
    }

    [Fact]
    public void GetWriteActionName4()
    {
        string name = "Test4";
        var instance = new InstanceFieldHelperTests { Name = name };
        var field = typeof(InstanceFieldHelperTests).GetField(nameof(Name), BindingFlags.Instance | BindingFlags.Public);
        var setter = InstanceHelper.GetWriteAction<object, object>(field);
        setter(instance, name);
        Assert.Equal(name, instance.Name);
    }
    #endregion
    #region 写入可空属性PublishedAt的测试
    [Fact]
    public void GetWriteActionPublishedAt0()
    {
        var field = typeof(InstanceFieldHelperTests).GetField(nameof(PublishedAt), BindingFlags.Instance | BindingFlags.Public);
        DateTime? publishedAt = DateTime.Now;
        var instance = new InstanceFieldHelperTests { PublishedAt = publishedAt };
        var setter = InstanceHelper.GetWriteAction<InstanceFieldHelperTests, DateTime?>(field);
        setter(instance, publishedAt);
        Assert.Equal(publishedAt, instance.PublishedAt);
    }

    [Fact]
    public void GetWriteActionPublishedAt1()
    {
        DateTime? publishedAt = DateTime.Now;
        var instance = new InstanceFieldHelperTests { PublishedAt = publishedAt };
        var setter = InstanceHelper.GetWriteAction<InstanceFieldHelperTests, DateTime?>(nameof(PublishedAt));
        setter(instance, publishedAt);
        Assert.Equal(publishedAt, instance.PublishedAt);
    }

    [Fact]
    public void GetWriteActionPublishedAt2()
    {
        DateTime? publishedAt = DateTime.Now;
        var instance = new InstanceFieldHelperTests { PublishedAt = publishedAt };
        var setter = InstanceHelper.GetWriteAction<InstanceFieldHelperTests, object>(nameof(PublishedAt));
        setter(instance, publishedAt);
        Assert.Equal(publishedAt, instance.PublishedAt);
    }

    [Fact]
    public void GetWriteActionPublishedAt3()
    {
        DateTime? publishedAt = DateTime.Now;
        var instance = new InstanceFieldHelperTests { PublishedAt = publishedAt };
        var field = typeof(InstanceFieldHelperTests).GetField(nameof(PublishedAt), BindingFlags.Instance | BindingFlags.Public);
        var setter = InstanceHelper.GetWriteAction<object, DateTime?>(field);
        setter(instance, publishedAt);
        Assert.Equal(publishedAt, instance.PublishedAt);
    }

    [Fact]
    public void GetWriteActionPublishedAt4()
    {
        DateTime? publishedAt = DateTime.Now;
        var instance = new InstanceFieldHelperTests { PublishedAt = publishedAt };
        var field = typeof(InstanceFieldHelperTests).GetField(nameof(PublishedAt), BindingFlags.Instance | BindingFlags.Public);
        var setter = InstanceHelper.GetWriteAction<object, object>(field);
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
