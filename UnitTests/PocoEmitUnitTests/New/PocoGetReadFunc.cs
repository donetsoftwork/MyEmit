using PocoEmit;
using PocoEmitUnitTests.Supports;

namespace PocoEmitUnitTests.New;

public class PocoGetReadFunc : PocoMemberTestBase
{
    [Fact]
    public void GetReadFunc_int()
    {
        // Act
        var readFunc = _poco.GetReadFunc<TestInstance, int>(nameof(_instance.IntProperty));
        // Assert
        Assert.NotNull(readFunc);
        int source = _instance.IntProperty;
        Assert.Equal(source, readFunc(_instance));
    }
    [Fact]
    public void GetReadFunc_int2()
    {
        // Act
        var readFunc = _poco.GetReadFunc<TestInstance, int>(nameof(_instance.IntField));
        // Assert
        Assert.NotNull(readFunc);
        int source = _instance.IntField!.Value;
        Assert.Equal(source, readFunc(_instance));
    }
    [Fact]
    public void GetReadFunc_Nint()
    {
        // Act
        var readFunc = _poco.GetReadFunc<TestInstance, int?>(nameof(_instance.IntField));
        // Assert
        Assert.NotNull(readFunc);
        int? source = _instance.IntField;
        Assert.Equal(source, readFunc(_instance));
    }
    [Fact]
    public void GetReadFunc_Nint2()
    {
        // Act
        var readFunc = _poco.GetReadFunc<TestInstance, int?>(nameof(_instance.IntProperty));
        // Assert
        Assert.NotNull(readFunc);
        int? source = _instance.IntProperty;
        Assert.Equal(source, readFunc(_instance));
    }
    [Fact]
    public void GetReadFunc_decimal()
    {
        // Act
        var readFunc = _poco.GetReadFunc<TestInstance, decimal>(nameof(_instance.DecimalProperty));
        // Assert
        Assert.NotNull(readFunc);
        decimal source = _instance.DecimalProperty;
        Assert.Equal(source, readFunc(_instance));
    }
    [Fact]
    public void GetReadFunc_decimal2()
    {
        // Act
        var readFunc = _poco.GetReadFunc<TestInstance, decimal>(nameof(_instance.DecimalField));
        // Assert
        Assert.NotNull(readFunc);
        decimal source = _instance.DecimalField!.Value;
        Assert.Equal(source, readFunc(_instance));
    }
    [Fact]
    public void GetReadFunc_Ndecimal()
    {
        // Act
        var readFunc = _poco.GetReadFunc<TestInstance, decimal?>(nameof(_instance.DecimalField));
        // Assert
        Assert.NotNull(readFunc);
        decimal? source = _instance.DecimalField;
        Assert.Equal(source, readFunc(_instance));
    }
    [Fact]
    public void GetReadFunc_Ndecimal2()
    {
        // Act
        var readFunc = _poco.GetReadFunc<TestInstance, decimal?>(nameof(_instance.DecimalProperty));
        // Assert
        Assert.NotNull(readFunc);
        decimal? source = _instance.DecimalProperty;
        Assert.Equal(source, readFunc(_instance));
    }
    [Fact]
    public void GetReadFunc_string()
    {
        // Act
        var readFunc = _poco.GetReadFunc<TestInstance, string>(nameof(_instance.StringField));
        // Assert
        Assert.NotNull(readFunc);
        string source = _instance.StringField;
        Assert.Equal(source, readFunc(_instance));
    }
    [Fact]
    public void GetReadFunc_string2()
    {
        // Act
        var readFunc = _poco.GetReadFunc<TestInstance, string>(nameof(_instance.StringProperty));
        // Assert
        Assert.NotNull(readFunc);
        string source = _instance.StringProperty!;
        Assert.Equal(source, readFunc(_instance));
    }
    [Fact]
    public void GetReadFunc_Nstring()
    {
        // Act
        var readFunc = _poco.GetReadFunc<TestInstance, string?>(nameof(_instance.StringProperty));
        // Assert
        Assert.NotNull(readFunc);
        string? source = _instance.StringProperty;
        Assert.Equal(source, readFunc(_instance));
    }
    [Fact]
    public void GetReadFunc_Nstring2()
    {
        // Act
        var readFunc = _poco.GetReadFunc<TestInstance, string?>(nameof(_instance.StringField));
        // Assert
        Assert.NotNull(readFunc);
        string? source = _instance.StringField;
        Assert.Equal(source, readFunc(_instance));
    }
    [Fact]
    public void GetReadFunc_DateTime()
    {
        // Act
        var readFunc = _poco.GetReadFunc<TestInstance, DateTime>(nameof(_instance.TimeField));
        // Assert
        Assert.NotNull(readFunc);
        DateTime source = _instance.TimeField!.Value;
        Assert.Equal(source, readFunc(_instance));
    }
    [Fact]
    public void GetReadFunc_DateTime2()
    {
        // Act
        var readFunc = _poco.GetReadFunc<TestInstance, DateTime>(nameof(_instance.TimeProperty));
        // Assert
        Assert.NotNull(readFunc);
        DateTime source = _instance.TimeProperty!.Value;
        Assert.Equal(source, readFunc(_instance));
    }
    [Fact]
    public void GetReadFunc_NDateTime()
    {
        // Act
        var readFunc = _poco.GetReadFunc<TestInstance, DateTime?>(nameof(_instance.TimeProperty));
        // Assert
        Assert.NotNull(readFunc);
        DateTime? source = _instance.TimeProperty;
        Assert.Equal(source, readFunc(_instance));
    }
    [Fact]
    public void GetReadFunc_NDateTime2()
    {
        // Act
        var readFunc = _poco.GetReadFunc<TestInstance, DateTime?>(nameof(_instance.TimeField));
        // Assert
        Assert.NotNull(readFunc);
        DateTime? source = _instance.TimeField;
        Assert.Equal(source, readFunc(_instance));
    }
    [Fact]
    public void GetReadFunc_object()
    {
        // Act
        var readFunc = _poco.GetReadFunc<TestInstance, object>(nameof(_instance.ObjectProperty));
        // Assert
        Assert.NotNull(readFunc);
        object source = _instance.ObjectProperty;
        Assert.Equal(source, readFunc(_instance));
    }
    [Fact]
    public void GetReadFunc_object2()
    {
        // Act
        var readFunc = _poco.GetReadFunc<TestInstance, object>(nameof(_instance.ObjectField));
        // Assert
        Assert.NotNull(readFunc);
        object source = _instance.ObjectField!;
        Assert.Equal(source, readFunc(_instance));
    }
    [Fact]
    public void GetReadFunc_Nobject()
    {
        // Act
        var readFunc = _poco.GetReadFunc<TestInstance, object?>(nameof(_instance.ObjectField));
        // Assert
        Assert.NotNull(readFunc);
        object? source = _instance.ObjectField;
        Assert.Equal(source, readFunc(_instance));
    }
    [Fact]
    public void GetReadFunc_Nobject2()
    {
        // Act
        var readFunc = _poco.GetReadFunc<TestInstance, object?>(nameof(_instance.ObjectProperty));
        // Assert
        Assert.NotNull(readFunc);
        object? source = _instance.ObjectProperty;
        Assert.Equal(source, readFunc(_instance));
    }
}
