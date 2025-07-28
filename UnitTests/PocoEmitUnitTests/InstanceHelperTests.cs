using PocoEmit;
using System.Reflection;

namespace PocoEmitUnitTests;

public class InstanceHelperTests
{
    #region GetReadFunc
    #region 读取结构体属性Id的测试
    [Fact]
    public void GetReadFuncId1()
    {
        int id = 41;
        var instance = new InstanceHelperTests { Id = id };
        var getter = InstanceHelper.GetReadFunc<InstanceHelperTests, int>(nameof(Id));
        var value = getter(instance);
        Assert.Equal(id, value);
    }

    [Fact]
    public void GetReadFuncId2()
    {
        int id = 42;
        var instance = new InstanceHelperTests { Id = id };
        var getter = InstanceHelper.GetReadFunc<InstanceHelperTests, object>(nameof(Id));
        var value = getter(instance);
        Assert.Equal(id, value);
    }

    #endregion
    #region 读取可空属性Name的测试

    [Fact]
    public void GetReadFuncName01()
    {
        string? name = "Test1";
        var instance = new InstanceHelperTests { Name = name };
        var getter = InstanceHelper.GetReadFunc<InstanceHelperTests, string?>(nameof(Name));
        var value = getter(instance);
        Assert.Equal(name, value);
    }

    [Fact]
    public void GetReadFuncName02()
    {
        string? name = "Test2";
        var instance = new InstanceHelperTests { Name = name };
        var getter = InstanceHelper.GetReadFunc<InstanceHelperTests, object>(nameof(Name));
        var value = getter(instance);
        Assert.Equal(name, value);
    }
    #endregion
    #region 读取字符串属性Name的测试

    [Fact]
    public void GetReadFuncName1()
    {
        string name = "Test1";
        var instance = new InstanceHelperTests { Name = name };
        var getter = InstanceHelper.GetReadFunc<InstanceHelperTests, string>(nameof(Name));
        var value = getter(instance);
        Assert.Equal(name, value);
    }

    [Fact]
    public void GetReadFuncName2()
    {
        string name = "Test2";
        var instance = new InstanceHelperTests { Name = name };
        var getter = InstanceHelper.GetReadFunc<InstanceHelperTests, object>(nameof(Name));
        var value = getter(instance);
        Assert.Equal(name, value);
    }

    #endregion
    #region 读取可空属性PublishedAt的测试

    [Fact]
    public void GetReadFuncPublishedAt1()
    {
        DateTime? publishedAt = DateTime.Now;
        var instance = new InstanceHelperTests { PublishedAt = publishedAt };
        var getter = InstanceHelper.GetReadFunc<InstanceHelperTests, DateTime?>(nameof(PublishedAt));
        var value = getter(instance);
        Assert.Equal(publishedAt, value);
    }

    [Fact]
    public void GetReadFuncPublishedAt2()
    {
        DateTime? publishedAt = DateTime.Now;
        var instance = new InstanceHelperTests { PublishedAt = publishedAt };
        var getter = InstanceHelper.GetReadFunc<InstanceHelperTests, object>(nameof(PublishedAt));
        var value = getter(instance);
        Assert.Equal(publishedAt, value);
    }
    #endregion
    #endregion
    #region GetWriteAction
    #region 写入结构体属性Id的测试
    [Fact]
    public void GetWriteActionId1()
    {
        int id = 41;
        var instance = new InstanceHelperTests { Id = id };
        var setter = InstanceHelper.GetWriteAction<InstanceHelperTests, int>(nameof(Id));
        setter(instance, id);
        Assert.Equal(id, instance.Id);
    }

    [Fact]
    public void GetWriteActionId2()
    {
        int id = 42;
        var instance = new InstanceHelperTests { Id = id };
        var setter = InstanceHelper.GetWriteAction<InstanceHelperTests, object>(nameof(Id));
        setter(instance, id);
        Assert.Equal(id, instance.Id);
    }
    #endregion
    #region 写入可空属性Name的测试
    [Fact]
    public void GetWriteActionName01()
    {
        string? name = "Test1";
        var instance = new InstanceHelperTests { Name = name };
        var setter = InstanceHelper.GetWriteAction<InstanceHelperTests, string?>(nameof(Name));
        setter(instance, name);
        Assert.Equal(name, instance.Name);
    }

    [Fact]
    public void GetWriteActionName02()
    {
        string? name = "Test2";
        var instance = new InstanceHelperTests { Name = name };
        var setter = InstanceHelper.GetWriteAction<InstanceHelperTests, object>(nameof(Name));
        setter(instance, name);
        Assert.Equal(name, instance.Name);
    }
    #endregion
    #region 写入字符串属性Name的测试
    [Fact]
    public void GetWriteActionName1()
    {
        string name = "Test1";
        var instance = new InstanceHelperTests { Name = name };
        var setter = InstanceHelper.GetWriteAction<InstanceHelperTests, string>(nameof(Name));
        setter(instance, name);
        Assert.Equal(name, instance.Name);
    }

    [Fact]
    public void GetWriteActionName2()
    {
        string name = "Test2";
        var instance = new InstanceHelperTests { Name = name };
        var setter = InstanceHelper.GetWriteAction<InstanceHelperTests, object>(nameof(Name));
        setter(instance, name);
        Assert.Equal(name, instance.Name);
    }
    #endregion
    #region 写入可空属性PublishedAt的测试
    [Fact]
    public void GetWriteActionPublishedAt1()
    {
        DateTime? publishedAt = DateTime.Now;
        var instance = new InstanceHelperTests { PublishedAt = publishedAt };
        var setter = InstanceHelper.GetWriteAction<InstanceHelperTests, DateTime?>(nameof(PublishedAt));
        setter(instance, publishedAt);
        Assert.Equal(publishedAt, instance.PublishedAt);
    }

    [Fact]
    public void GetWriteActionPublishedAt2()
    {
        DateTime? publishedAt = DateTime.Now;
        var instance = new InstanceHelperTests { PublishedAt = publishedAt };
        var setter = InstanceHelper.GetWriteAction<InstanceHelperTests, object>(nameof(PublishedAt));
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