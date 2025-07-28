using PocoEmit;
using PocoEmitUnitTests.Supports;

namespace PocoEmitUnitTests.New;

public class PocoGetWriteAction : PocoMemberWriteTestBase
{
    [Fact]
    public void GetWriteAction_int()
    {
        // Act
        var writeAction = _poco.GetWriteAction<TestInstance, int>(nameof(_instance.IntProperty));
        // Assert
        Assert.NotNull(writeAction);
        var source = IntProperty;
        writeAction(_instance, source);
        Assert.Equal(source, _instance.IntProperty);
    }
    [Fact]
    public void GetWriteAction_int2()
    {
        // Act
        var writeAction = _poco.GetWriteAction<TestInstance, int>(nameof(IntField));
        // Assert
        Assert.NotNull(writeAction);
        var source = IntField;
        writeAction(_instance, source!.Value);
        Assert.Equal(source, _instance.IntField);
    }
    [Fact]
    public void GetWriteAction_Nint()
    {
        // Act
        var writeAction = _poco.GetWriteAction<TestInstance, int?>(nameof(IntField));
        // Assert
        Assert.NotNull(writeAction);
        var source = IntField;
        writeAction(_instance, source!.Value);
        Assert.Equal(source, _instance.IntField);
    }
    [Fact]
    public void GetWriteAction_Nint2()
    {
        // Act
        var writeAction = _poco.GetWriteAction<TestInstance, int?>(nameof(IntProperty));
        // Assert
        Assert.NotNull(writeAction);
        var source = IntProperty;
        writeAction(_instance, source);
        Assert.Equal(source, _instance.IntProperty);
    }
    [Fact]
    public void GetWriteAction_decimal()
    {
        // Act
        var writeAction = _poco.GetWriteAction<TestInstance, decimal>(nameof(DecimalProperty));
        // Assert
        Assert.NotNull(writeAction);
        var source = DecimalProperty;
        writeAction(_instance, source);
        Assert.Equal(source, _instance.DecimalProperty);
    }
    [Fact]
    public void GetWriteAction_decimal2()
    {
        // Act
        var writeAction = _poco.GetWriteAction<TestInstance, decimal>(nameof(DecimalField));
        // Assert
        Assert.NotNull(writeAction);
        var source = DecimalField;
        writeAction(_instance, source!.Value);
        Assert.Equal(source, _instance.DecimalField);
    }
    [Fact]
    public void GetWriteAction_Ndecimal()
    {
        // Act
        var writeAction = _poco.GetWriteAction<TestInstance, decimal?>(nameof(DecimalField));
        // Assert
        Assert.NotNull(writeAction);
        var source = DecimalField;
        writeAction(_instance, source);
        Assert.Equal(source, _instance.DecimalField);
    }
    [Fact]
    public void GetWriteAction_Ndecimal2()
    {
        // Act
        var writeAction = _poco.GetWriteAction<TestInstance, decimal?>(nameof(DecimalProperty));
        // Assert
        Assert.NotNull(writeAction);
        var source = DecimalProperty;
        writeAction(_instance, source);
        Assert.Equal(source, _instance.DecimalProperty);
    }
    [Fact]
    public void GetWriteAction_string()
    {
        // Act
        var writeAction = _poco.GetWriteAction<TestInstance, string>(nameof(StringField));
        // Assert
        Assert.NotNull(writeAction);
        var source = StringField;
        writeAction(_instance, source);
        Assert.Equal(source, _instance.StringField);
    }
    [Fact]
    public void GetWriteAction_string2()
    {
        // Act
        var writeAction = _poco.GetWriteAction<TestInstance, string>(nameof(StringProperty));
        // Assert
        Assert.NotNull(writeAction);
        var source = StringProperty;
        writeAction(_instance, source!);
        Assert.Equal(source, _instance.StringProperty);
    }
    [Fact]
    public void GetWriteAction_Nstring()
    {
        // Act
        var writeAction = _poco.GetWriteAction<TestInstance, string?>(nameof(StringProperty));
        // Assert
        Assert.NotNull(writeAction);
        var source = StringProperty;
        writeAction(_instance, source);
        Assert.Equal(source, _instance.StringProperty);
    }
    [Fact]
    public void GetWriteAction_Nstring2()
    {
        // Act
        var writeAction = _poco.GetWriteAction<TestInstance, string?>(nameof(StringField));
        // Assert
        Assert.NotNull(writeAction);
        var source = StringField;
        writeAction(_instance, source);
        Assert.Equal(source, _instance.StringField);
    }
    [Fact]
    public void GetWriteAction_DateTime()
    {
        // Act
        var writeAction = _poco.GetWriteAction<TestInstance, DateTime>(nameof(TimeField));
        // Assert
        Assert.NotNull(writeAction);
        var source = TimeField!.Value;
        writeAction(_instance, source);
        Assert.Equal(source, _instance.TimeField);
    }
    [Fact]
    public void GetWriteAction_DateTime2()
    {
        // Act
        var writeAction = _poco.GetWriteAction<TestInstance, DateTime>(nameof(TimeProperty));
        // Assert
        Assert.NotNull(writeAction);
        var source = TimeProperty;
        writeAction(_instance, source!.Value);
        Assert.Equal(source, _instance.TimeProperty);
    }
    [Fact]
    public void GetWriteAction_NDateTime()
    {
        // Act
        var writeAction = _poco.GetWriteAction<TestInstance, DateTime?>(nameof(TimeProperty));
        // Assert
        Assert.NotNull(writeAction);
        var source = TimeProperty;
        writeAction(_instance, source);
        Assert.Equal(source, _instance.TimeProperty);
    }
    [Fact]
    public void GetWriteAction_NDateTime2()
    {
        // Act
        var writeAction = _poco.GetWriteAction<TestInstance, DateTime?>(nameof(TimeField));
        // Assert
        Assert.NotNull(writeAction);
        var source = TimeField;
        writeAction(_instance, source);
        Assert.Equal(source, _instance.TimeField);
    }
    [Fact]
    public void GetWriteAction_object()
    {
        // Act
        var writeAction = _poco.GetWriteAction<TestInstance, object>(nameof(ObjectProperty));
        // Assert
        Assert.NotNull(writeAction);
        var source = ObjectProperty;
        writeAction(_instance, source);
        Assert.Equal(source, _instance.ObjectProperty);
    }
    [Fact]
    public void GetWriteAction_object2()
    {
        // Act
        var writeAction = _poco.GetWriteAction<TestInstance, object>(nameof(ObjectField));
        // Assert
        Assert.NotNull(writeAction);
        var source = ObjectField;
        writeAction(_instance, source!);
        Assert.Equal(source, _instance.ObjectField);
    }
    [Fact]
    public void GetWriteAction_Nobject()
    {
        // Act
        var writeAction = _poco.GetWriteAction<TestInstance, object?>(nameof(ObjectField));
        // Assert
        Assert.NotNull(writeAction);
        var source = ObjectField;
        writeAction(_instance, source);
        Assert.Equal(source, _instance.ObjectField);
    }
    [Fact]
    public void GetWriteAction_Nobject2()
    {
        // Act
        var writeAction = _poco.GetWriteAction<TestInstance, object?>(nameof(ObjectProperty));
        // Assert
        Assert.NotNull(writeAction);
        var source = ObjectProperty;
        writeAction(_instance, source);
        Assert.Equal(source, _instance.ObjectProperty);
    }
}

