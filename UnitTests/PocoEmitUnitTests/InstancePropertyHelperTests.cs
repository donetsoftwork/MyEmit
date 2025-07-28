using PocoEmit;
using System.Reflection;

namespace PocoEmitUnitTests;

public class InstancePropertyHelperTests
{
    #region GetReadFunc
    #region 读取结构体属性Id的测试
    [Fact]
    public void GetReadFuncId0()
    {
        var property = typeof(InstancePropertyHelperTests).GetProperty(nameof(Id), BindingFlags.Instance | BindingFlags.Public);
        int id = 40;
        var instance = new InstancePropertyHelperTests { Id = id };
        var getter = InstanceHelper.GetReadFunc<InstancePropertyHelperTests, int>(property);
        var value = getter(instance);
        Assert.Equal(id, value);
    }

    [Fact]
    public void GetReadFuncId1()
    {
        int id = 41;
        var instance = new InstancePropertyHelperTests { Id = id };
        var getter = InstanceHelper.GetReadFunc<InstancePropertyHelperTests, int>(nameof(Id));
        var value = getter(instance);
        Assert.Equal(id, value);
    }

    [Fact]
    public void GetReadFuncId2()
    {
        int id = 42;
        var instance = new InstancePropertyHelperTests { Id = id };
        var getter = InstanceHelper.GetReadFunc<InstancePropertyHelperTests, object>(nameof(Id));
        var value = getter(instance);
        Assert.Equal(id, value);
    }

    [Fact]
    public void GetReadFuncId3()
    {
        int id = 43;
        var instance = new InstancePropertyHelperTests { Id = id };
        var property = typeof(InstancePropertyHelperTests).GetProperty(nameof(Id), BindingFlags.Instance | BindingFlags.Public);
        var getter = InstanceHelper.GetReadFunc<object, int>(property);
        var value = getter(instance);
        Assert.Equal(id, value);
    }

    [Fact]
    public void GetReadFuncId4()
    {
        int id = 44;
        var instance = new InstancePropertyHelperTests { Id = id };
        var property = typeof(InstancePropertyHelperTests).GetProperty(nameof(Id), BindingFlags.Instance | BindingFlags.Public);
        var getter = InstanceHelper.GetReadFunc<object, object>(property);
        var value = getter(instance);
        Assert.Equal(id, value);
    }
    [Fact]
    public void GetReadFuncId5()
    {
        int? id = 55;
        var instance = new InstancePropertyHelperTests { Id = id.Value };
        var property = typeof(InstancePropertyHelperTests).GetProperty(nameof(Id), BindingFlags.Instance | BindingFlags.Public);
        var getter = InstanceHelper.GetReadFunc<object, int?>(property);
        var value = getter(instance);
        Assert.Equal(id, value);
    }
    [Fact]
    public void GetReadFuncId6()
    {
        short id = 66;
        var instance = new InstancePropertyHelperTests { Id = id };
        var property = typeof(InstancePropertyHelperTests).GetProperty(nameof(Id), BindingFlags.Instance | BindingFlags.Public);
        var getter = InstanceHelper.GetReadFunc<object, short>(property);
        var value = getter(instance);
        Assert.Equal(id, value);
    }
    [Fact]
    public void GetReadFuncId7()
    {
        ulong id = 77;
        var instance = new InstancePropertyHelperTests { Id = (int)id };
        var property = typeof(InstancePropertyHelperTests).GetProperty(nameof(Id), BindingFlags.Instance | BindingFlags.Public);
        var getter = InstanceHelper.GetReadFunc<object, ulong>(property);
        var value = getter(instance);
        Assert.Equal(id, value);
    }
    [Fact]
    public void GetReadFuncId8()
    {
        decimal id = 88.0M;
        var instance = new InstancePropertyHelperTests { Id = (int)id };
        var property = typeof(InstancePropertyHelperTests).GetProperty(nameof(Id), BindingFlags.Instance | BindingFlags.Public);
        var getter = InstanceHelper.GetReadFunc<object, decimal>(property);
        var value = getter(instance);
        Assert.Equal(id, value);
    }
    //[Fact]
    //public void GetReadFuncId8()
    //{
    //    string id = "88";
    //    var instance = new InstancePropertyHelperTests { Id = int.Parse(id) };
    //    var property = typeof(InstancePropertyHelperTests).GetProperty(nameof(Id), BindingFlags.Instance | BindingFlags.Public);
    //    var getter = InstanceHelper.GetReadFunc<object, string>(property);
    //    var value = getter(instance);
    //    Assert.Equal(id, value);
    //}
    #endregion
    #region 读取可空属性Name的测试
    [Fact]
    public void GetReadFuncName00()
    {
        var property = typeof(InstancePropertyHelperTests).GetProperty(nameof(Name), BindingFlags.Instance | BindingFlags.Public);
        string? name = "Test0";
        var instance = new InstancePropertyHelperTests { Name = name };
        var getter = InstanceHelper.GetReadFunc<InstancePropertyHelperTests, string?>(property);
        var value = getter(instance);
        Assert.Equal(name, value);
    }
    [Fact]
    public void GetReadFuncName01()
    {
        string? name = "Test1";
        var instance = new InstancePropertyHelperTests { Name = name };
        var getter = InstanceHelper.GetReadFunc<InstancePropertyHelperTests, string?>(nameof(Name));
        var value = getter(instance);
        Assert.Equal(name, value);
    }

    [Fact]
    public void GetReadFuncName02()
    {
        string? name = "Test2";
        var instance = new InstancePropertyHelperTests { Name = name };
        var getter = InstanceHelper.GetReadFunc<InstancePropertyHelperTests, object>(nameof(Name));
        var value = getter(instance);
        Assert.Equal(name, value);
    }

    [Fact]
    public void GetReadFuncName03()
    {
        string? name = "Test3";
        var instance = new InstancePropertyHelperTests { Name = name };
        var property = typeof(InstancePropertyHelperTests).GetProperty(nameof(Name), BindingFlags.Instance | BindingFlags.Public);
        var getter = InstanceHelper.GetReadFunc<object, string?>(property);
        var value = getter(instance);
        Assert.Equal(name, value);
    }

    [Fact]
    public void GetReadFuncName04()
    {
        string? name = "Test4";
        var instance = new InstancePropertyHelperTests { Name = name };
        var property = typeof(InstancePropertyHelperTests).GetProperty(nameof(Name), BindingFlags.Instance | BindingFlags.Public);
        var getter = InstanceHelper.GetReadFunc<object, object>(property);
        var value = getter(instance);
        object expected = name;
        Assert.Equal(expected, value);
    }
    #endregion
    #region 读取字符串属性Name的测试
    [Fact]
    public void GetReadFuncName0()
    {
        var property = typeof(InstancePropertyHelperTests).GetProperty(nameof(Name), BindingFlags.Instance | BindingFlags.Public);
        string name = "Test0";
        var instance = new InstancePropertyHelperTests { Name = name };
        var getter = InstanceHelper.GetReadFunc<InstancePropertyHelperTests, string>(property);
        var value = getter(instance);
        Assert.Equal(name, value);
    }

    [Fact]
    public void GetReadFuncName1()
    {
        string name = "Test1";
        var instance = new InstancePropertyHelperTests { Name = name };
        var getter = InstanceHelper.GetReadFunc<InstancePropertyHelperTests, string>(nameof(Name));
        var value = getter(instance);
        Assert.Equal(name, value);
    }

    [Fact]
    public void GetReadFuncName2()
    {
        string name = "Test2";
        var instance = new InstancePropertyHelperTests { Name = name };
        var getter = InstanceHelper.GetReadFunc<InstancePropertyHelperTests, object>(nameof(Name));
        var value = getter(instance);
        Assert.Equal(name, value);
    }

    [Fact]
    public void GetReadFuncName3()
    {
        string name = "Test3";
        var instance = new InstancePropertyHelperTests { Name = name };
        var property = typeof(InstancePropertyHelperTests).GetProperty(nameof(Name), BindingFlags.Instance | BindingFlags.Public);
        var getter = InstanceHelper.GetReadFunc<object, string>(property);
        var value = getter(instance);
        Assert.Equal(name, value);
    }

    [Fact]
    public void GetReadFuncName4()
    {
        string name = "Test4";
        var instance = new InstancePropertyHelperTests { Name = name };
        var property = typeof(InstancePropertyHelperTests).GetProperty(nameof(Name), BindingFlags.Instance | BindingFlags.Public);
        var getter = InstanceHelper.GetReadFunc<object, object>(property);
        var value = getter(instance);
        object expected = name;
        Assert.Equal(expected, value);
    }
    #endregion
    #region 读取可空属性PublishedAt的测试
    [Fact]
    public void GetReadFuncPublishedAt0()
    {
        var property = typeof(InstancePropertyHelperTests).GetProperty(nameof(PublishedAt), BindingFlags.Instance | BindingFlags.Public);
        DateTime? publishedAt = DateTime.Now;
        var instance = new InstancePropertyHelperTests { PublishedAt = publishedAt };
        var getter = InstanceHelper.GetReadFunc<InstancePropertyHelperTests, DateTime?>(property);
        var value = getter(instance);
        Assert.Equal(publishedAt, value);
    }

    [Fact]
    public void GetReadFuncPublishedAt1()
    {
        DateTime? publishedAt = DateTime.Now;
        var instance = new InstancePropertyHelperTests { PublishedAt = publishedAt };
        var getter = InstanceHelper.GetReadFunc<InstancePropertyHelperTests, DateTime?>(nameof(PublishedAt));
        var value = getter(instance);
        Assert.Equal(publishedAt, value);
    }

    [Fact]
    public void GetReadFuncPublishedAt2()
    {
        DateTime? publishedAt = DateTime.Now;
        var instance = new InstancePropertyHelperTests { PublishedAt = publishedAt };
        var getter = InstanceHelper.GetReadFunc<InstancePropertyHelperTests, object>(nameof(PublishedAt));
        var value = getter(instance);
        Assert.Equal(publishedAt, value);
    }

    [Fact]
    public void GetReadFuncPublishedAt3()
    {
        DateTime? publishedAt = DateTime.Now;
        var instance = new InstancePropertyHelperTests { PublishedAt = publishedAt };
        var property = typeof(InstancePropertyHelperTests).GetProperty(nameof(PublishedAt), BindingFlags.Instance | BindingFlags.Public);
        var getter = InstanceHelper.GetReadFunc<object, DateTime?>(property);
        var value = getter(instance);
        Assert.Equal(publishedAt, value);
    }

    [Fact]
    public void GetReadFuncPublishedAt4()
    {
        DateTime? publishedAt = DateTime.Now;
        var instance = new InstancePropertyHelperTests { PublishedAt = publishedAt };
        var property = typeof(InstancePropertyHelperTests).GetProperty(nameof(PublishedAt), BindingFlags.Instance | BindingFlags.Public);
        var getter = InstanceHelper.GetReadFunc<object, object>(property);
        var value = getter(instance);
        object expected = publishedAt;
        Assert.Equal(expected, value);
    }
    [Fact]
    public void GetReadFuncPublishedAt5()
    {
        DateTime publishedAt = DateTime.Now;
        var instance = new InstancePropertyHelperTests { PublishedAt = publishedAt };
        var property = typeof(InstancePropertyHelperTests).GetProperty(nameof(PublishedAt), BindingFlags.Instance | BindingFlags.Public);
        var getter = InstanceHelper.GetReadFunc<object, DateTime>(property);
        var value = getter(instance);
        Assert.Equal(publishedAt, value);
    }
    #endregion
    #endregion
    #region GetWriteAction
    #region 写入结构体属性Id的测试
    [Fact]
    public void GetWriteActionId0()
    {
        var property = typeof(InstancePropertyHelperTests).GetProperty(nameof(Id), BindingFlags.Instance | BindingFlags.Public);
        int id = 40;
        var instance = new InstancePropertyHelperTests();
        var setter = InstanceHelper.GetWriteAction<InstancePropertyHelperTests, int>(property);
        setter(instance, id);
        Assert.Equal(id, instance.Id);
    }

    [Fact]
    public void GetWriteActionId1()
    {
        int id = 41;
        var instance = new InstancePropertyHelperTests();
        var setter = InstanceHelper.GetWriteAction<InstancePropertyHelperTests, int>(nameof(Id));
        setter(instance, id);
        Assert.Equal(id, instance.Id);
    }

    [Fact]
    public void GetWriteActionId2()
    {
        int id = 42;
        var instance = new InstancePropertyHelperTests();
        var setter = InstanceHelper.GetWriteAction<InstancePropertyHelperTests, object>(nameof(Id));
        setter(instance, id);
        Assert.Equal(id, instance.Id);
    }

    [Fact]
    public void GetWriteActionId3()
    {
        int id = 43;
        var instance = new InstancePropertyHelperTests();
        var property = typeof(InstancePropertyHelperTests).GetProperty(nameof(Id), BindingFlags.Instance | BindingFlags.Public);
        var setter = InstanceHelper.GetWriteAction<object, int>(property);
        setter(instance, id);
        Assert.Equal(id, instance.Id);
    }

    [Fact]
    public void GetWriteActionId4()
    {
        int id = 44;
        var instance = new InstancePropertyHelperTests();
        var property = typeof(InstancePropertyHelperTests).GetProperty(nameof(Id), BindingFlags.Instance | BindingFlags.Public);
        var setter = InstanceHelper.GetWriteAction<object, object>(property);
        setter(instance, id);
        Assert.Equal(id, instance.Id);
    }
    [Fact]
    public void GetWriteActionId5()
    {
        int? id = 55;
        var instance = new InstancePropertyHelperTests();
        var property = typeof(InstancePropertyHelperTests).GetProperty(nameof(Id), BindingFlags.Instance | BindingFlags.Public);
        var setter = InstanceHelper.GetWriteAction<object, int?>(property);
        setter(instance, id);
        Assert.Equal(id, instance.Id);
    }
    #endregion
    #region 写入可空属性Name的测试
    [Fact]
    public void GetWriteActionName00()
    {
        var property = typeof(InstancePropertyHelperTests).GetProperty(nameof(Name), BindingFlags.Instance | BindingFlags.Public);
        string? name = "Test0";
        var instance = new InstancePropertyHelperTests();
        var setter = InstanceHelper.GetWriteAction<InstancePropertyHelperTests, string?>(property);
        setter(instance, name);
        Assert.Equal(name, instance.Name);
    }

    [Fact]
    public void GetWriteActionName01()
    {
        string? name = "Test1";
        var instance = new InstancePropertyHelperTests ();
        var setter = InstanceHelper.GetWriteAction<InstancePropertyHelperTests, string?>(nameof(Name));
        setter(instance, name);
        Assert.Equal(name, instance.Name);
    }

    [Fact]
    public void GetWriteActionName02()
    {
        string? name = "Test2";
        var instance = new InstancePropertyHelperTests();
        var setter = InstanceHelper.GetWriteAction<InstancePropertyHelperTests, object>(nameof(Name));
        setter(instance, name);
        Assert.Equal(name, instance.Name);
    }

    [Fact]
    public void GetWriteActionName03()
    {
        string? name = "Test3";
        var instance = new InstancePropertyHelperTests ();
        var property = typeof(InstancePropertyHelperTests).GetProperty(nameof(Name), BindingFlags.Instance | BindingFlags.Public);
        var setter = InstanceHelper.GetWriteAction<object, string?>(property);
        setter(instance, name);
        Assert.Equal(name, instance.Name);
    }

    [Fact]
    public void GetWriteActionName04()
    {
        string? name = "Test4";
        var instance = new InstancePropertyHelperTests ();
        var property = typeof(InstancePropertyHelperTests).GetProperty(nameof(Name), BindingFlags.Instance | BindingFlags.Public);
        var setter = InstanceHelper.GetWriteAction<object, object>(property);
        setter(instance, name);
        Assert.Equal(name, instance.Name);
    }
    #endregion
    #region 写入字符串属性Name的测试
    [Fact]
    public void GetWriteActionName0()
    {
        var property = typeof(InstancePropertyHelperTests).GetProperty(nameof(Name), BindingFlags.Instance | BindingFlags.Public);
        string name = "Test0";
        var instance = new InstancePropertyHelperTests ();
        var setter = InstanceHelper.GetWriteAction<InstancePropertyHelperTests, string>(property);
        setter(instance, name);
        Assert.Equal(name, instance.Name);
    }

    [Fact]
    public void GetWriteActionName1()
    {
        string name = "Test1";
        var instance = new InstancePropertyHelperTests ();
        var setter = InstanceHelper.GetWriteAction<InstancePropertyHelperTests, string>(nameof(Name));
        setter(instance, name);
        Assert.Equal(name, instance.Name);
    }

    [Fact]
    public void GetWriteActionName2()
    {
        string name = "Test2";
        var instance = new InstancePropertyHelperTests ();
        var setter = InstanceHelper.GetWriteAction<InstancePropertyHelperTests, object>(nameof(Name));
        setter(instance, name);
        Assert.Equal(name, instance.Name);
    }

    [Fact]
    public void GetWriteActionName3()
    {
        string name = "Test3";
        var instance = new InstancePropertyHelperTests ();
        var property = typeof(InstancePropertyHelperTests).GetProperty(nameof(Name), BindingFlags.Instance | BindingFlags.Public);
        var setter = InstanceHelper.GetWriteAction<object, string>(property);
        setter(instance, name);
        Assert.Equal(name, instance.Name);
    }

    [Fact]
    public void GetWriteActionName4()
    {
        string name = "Test4";
        var instance = new InstancePropertyHelperTests ();
        var property = typeof(InstancePropertyHelperTests).GetProperty(nameof(Name), BindingFlags.Instance | BindingFlags.Public);
        var setter = InstanceHelper.GetWriteAction<object, object>(property);
        setter(instance, name);
        Assert.Equal(name, instance.Name);
    }
    #endregion
    #region 写入可空属性PublishedAt的测试
    [Fact]
    public void GetWriteActionPublishedAt0()
    {
        var property = typeof(InstancePropertyHelperTests).GetProperty(nameof(PublishedAt), BindingFlags.Instance | BindingFlags.Public);
        DateTime? publishedAt = DateTime.Now;
        var instance = new InstancePropertyHelperTests ();
        var setter = InstanceHelper.GetWriteAction<InstancePropertyHelperTests, DateTime?>(property);
        setter(instance, publishedAt);
        Assert.Equal(publishedAt, instance.PublishedAt);
    }

    [Fact]
    public void GetWriteActionPublishedAt1()
    {
        DateTime? publishedAt = DateTime.Now;
        var instance = new InstancePropertyHelperTests ();
        var setter = InstanceHelper.GetWriteAction<InstancePropertyHelperTests, DateTime?>(nameof(PublishedAt));
        setter(instance, publishedAt);
        Assert.Equal(publishedAt, instance.PublishedAt);
    }

    [Fact]
    public void GetWriteActionPublishedAt2()
    {
        DateTime? publishedAt = DateTime.Now;
        var instance = new InstancePropertyHelperTests ();
        var setter = InstanceHelper.GetWriteAction<InstancePropertyHelperTests, object>(nameof(PublishedAt));
        setter(instance, publishedAt);
        Assert.Equal(publishedAt, instance.PublishedAt);
    }

    [Fact]
    public void GetWriteActionPublishedAt3()
    {
        DateTime? publishedAt = DateTime.Now;
        var instance = new InstancePropertyHelperTests ();
        var property = typeof(InstancePropertyHelperTests).GetProperty(nameof(PublishedAt), BindingFlags.Instance | BindingFlags.Public);
        var setter = InstanceHelper.GetWriteAction<object, DateTime?>(property);
        setter(instance, publishedAt);
        Assert.Equal(publishedAt, instance.PublishedAt);
    }

    [Fact]
    public void GetWriteActionPublishedAt4()
    {
        DateTime? publishedAt = DateTime.Now;
        var instance = new InstancePropertyHelperTests ();
        var property = typeof(InstancePropertyHelperTests).GetProperty(nameof(PublishedAt), BindingFlags.Instance | BindingFlags.Public);
        var setter = InstanceHelper.GetWriteAction<object, object>(property);
        setter(instance, publishedAt);
        Assert.Equal(publishedAt, instance.PublishedAt);
    }

    [Fact]
    public void GetWriteActionPublishedAt5()
    {
        DateTime publishedAt = DateTime.Now;
        var instance = new InstancePropertyHelperTests();
        var property = typeof(InstancePropertyHelperTests).GetProperty(nameof(PublishedAt), BindingFlags.Instance | BindingFlags.Public);
        var setter = InstanceHelper.GetWriteAction<object, DateTime>(property);
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