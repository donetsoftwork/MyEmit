using PocoEmit.MapperUnitTests.Supports;

namespace PocoEmit.MapperUnitTests.New;

public class MapperWriterTests : MapperMemberWriteTestBase
{
    [Fact]
    public void GetMemberWriter_int()
    {
        // Act
        var writer = _mapper.GetMemberWriter<TestInstance, int>(nameof(_instance.IntProperty));
        // Assert
        Assert.NotNull(writer);
        var source = IntProperty;
        writer.Write(_instance, source);
        Assert.Equal(source, _instance.IntProperty);
    }
    [Fact]
    public void GetMemberWriter_int2()
    {
        // Act
        var writer = _mapper.GetMemberWriter<TestInstance, int>(nameof(IntField));
        // Assert
        Assert.NotNull(writer);
        var source = IntField;
        writer.Write(_instance, source!.Value);
        Assert.Equal(source, _instance.IntField);
    }
    [Fact]
    public void GetMemberWriter_Nint()
    {
        // Act
        var writer = _mapper.GetMemberWriter<TestInstance, int?>(nameof(IntField));
        // Assert
        Assert.NotNull(writer);
        var source = IntField;
        writer.Write(_instance, source!.Value);
        Assert.Equal(source, _instance.IntField);
    }
    [Fact]
    public void GetMemberWriter_Nint2()
    {
        // Act
        var writer = _mapper.GetMemberWriter<TestInstance, int?>(nameof(IntProperty));
        // Assert
        Assert.NotNull(writer);
        var source = IntProperty;
        writer.Write(_instance, source);
        Assert.Equal(source, _instance.IntProperty);
    }
    [Fact]
    public void GetMemberWriter_decimal()
    {
        // Act
        var writer = _mapper.GetMemberWriter<TestInstance, decimal>(nameof(DecimalProperty));
        // Assert
        Assert.NotNull(writer);
        var source = DecimalProperty;
        writer.Write(_instance, source);
        Assert.Equal(source, _instance.DecimalProperty);
    }
    [Fact]
    public void GetMemberWriter_decimal2()
    {
        // Act
        var writer = _mapper.GetMemberWriter<TestInstance, decimal>(nameof(DecimalField));
        // Assert
        Assert.NotNull(writer);
        var source = DecimalField;
        writer.Write(_instance, source!.Value);
        Assert.Equal(source, _instance.DecimalField);
    }
    [Fact]
    public void GetMemberWriter_Ndecimal()
    {
        // Act
        var writer = _mapper.GetMemberWriter<TestInstance, decimal?>(nameof(DecimalField));
        // Assert
        Assert.NotNull(writer);
        var source = DecimalField;
        writer.Write(_instance, source);
        Assert.Equal(source, _instance.DecimalField);
    }
    [Fact]
    public void GetMemberWriter_Ndecimal2()
    {
        // Act
        var writer = _mapper.GetMemberWriter<TestInstance, decimal?>(nameof(DecimalProperty));
        // Assert
        Assert.NotNull(writer);
        var source = DecimalProperty;
        writer.Write(_instance, source);
        Assert.Equal(source, _instance.DecimalProperty);
    }
    [Fact]
    public void GetMemberWriter_string()
    {
        // Act
        var writer = _mapper.GetMemberWriter<TestInstance, string>(nameof(StringField));
        // Assert
        Assert.NotNull(writer);
        var source = StringField;
        writer.Write(_instance, source);
        Assert.Equal(source, _instance.StringField);
    }
    [Fact]
    public void GetMemberWriter_string2()
    {
        // Act
        var writer = _mapper.GetMemberWriter<TestInstance, string>(nameof(StringProperty));
        // Assert
        Assert.NotNull(writer);
        var source = StringProperty;
        writer.Write(_instance, source!);
        Assert.Equal(source, _instance.StringProperty);
    }
    [Fact]
    public void GetMemberWriter_Nstring()
    {
        // Act
        var writer = _mapper.GetMemberWriter<TestInstance, string?>(nameof(StringProperty));
        // Assert
        Assert.NotNull(writer);
        var source = StringProperty;
        writer.Write(_instance, source);
        Assert.Equal(source, _instance.StringProperty);
    }
    [Fact]
    public void GetMemberWriter_Nstring2()
    {
        // Act
        var writer = _mapper.GetMemberWriter<TestInstance, string?>(nameof(StringField));
        // Assert
        Assert.NotNull(writer);
        var source = StringField;
        writer.Write(_instance, source);
        Assert.Equal(source, _instance.StringField);
    }
    [Fact]
    public void GetMemberWriter_DateTime()
    {
        // Act
        var writer = _mapper.GetMemberWriter<TestInstance, DateTime>(nameof(TimeField));
        // Assert
        Assert.NotNull(writer);
        var source = TimeField!.Value;
        writer.Write(_instance, source);
        Assert.Equal(source, _instance.TimeField);
    }
    [Fact]
    public void GetMemberWriter_DateTime2()
    {
        // Act
        var writer = _mapper.GetMemberWriter<TestInstance, DateTime>(nameof(TimeProperty));
        // Assert
        Assert.NotNull(writer);
        var source = TimeProperty;
        writer.Write(_instance, source!.Value);
        Assert.Equal(source, _instance.TimeProperty);
    }
    [Fact]
    public void GetMemberWriter_NDateTime()
    {
        // Act
        var writer = _mapper.GetMemberWriter<TestInstance, DateTime?>(nameof(TimeProperty));
        // Assert
        Assert.NotNull(writer);
        var source = TimeProperty;
        writer.Write(_instance, source);
        Assert.Equal(source, _instance.TimeProperty);
    }
    [Fact]
    public void GetMemberWriter_NDateTime2()
    {
        // Act
        var writer = _mapper.GetMemberWriter<TestInstance, DateTime?>(nameof(TimeField));
        // Assert
        Assert.NotNull(writer);
        var source = TimeField;
        writer.Write(_instance, source);
        Assert.Equal(source, _instance.TimeField);
    }
    [Fact]
    public void GetMemberWriter_object()
    {
        // Act
        var writer = _mapper.GetMemberWriter<TestInstance, object>(nameof(ObjectProperty));
        // Assert
        Assert.NotNull(writer);
        var source = ObjectProperty;
        writer.Write(_instance, source);
        Assert.Equal(source, _instance.ObjectProperty);
    }
    [Fact]
    public void GetMemberWriter_object2()
    {
        // Act
        var writer = _mapper.GetMemberWriter<TestInstance, object>(nameof(ObjectField));
        // Assert
        Assert.NotNull(writer);
        var source = ObjectField;
        writer.Write(_instance, source!);
        Assert.Equal(source, _instance.ObjectField);
    }
    [Fact]
    public void GetMemberWriter_Nobject()
    {
        // Act
        var writer = _mapper.GetMemberWriter<TestInstance, object?>(nameof(ObjectField));
        // Assert
        Assert.NotNull(writer);
        var source = ObjectField;
        writer.Write(_instance, source);
        Assert.Equal(source, _instance.ObjectField);
    }
    [Fact]
    public void GetMemberWriter_Nobject2()
    {
        // Act
        var writer = _mapper.GetMemberWriter<TestInstance, object?>(nameof(ObjectProperty));
        // Assert
        Assert.NotNull(writer);
        var source = ObjectProperty;
        writer.Write(_instance, source);
        Assert.Equal(source, _instance.ObjectProperty);
    }
}
