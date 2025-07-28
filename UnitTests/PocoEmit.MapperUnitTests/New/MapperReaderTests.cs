using PocoEmit.MapperUnitTests.Supports;

namespace PocoEmit.MapperUnitTests.New;

public class MapperReaderTests : MapperMemberTestBase
{
    [Fact]
    public void GetMemberReader_int()
    {
        // Act
        var reader = _mapper.GetMemberReader<TestInstance, int>(nameof(_instance.IntProperty));
        // Assert
        Assert.NotNull(reader);
        int source = _instance.IntProperty;
        Assert.Equal(source, reader.Read(_instance));
    }
    [Fact]
    public void GetMemberReader_int2()
    {
        // Act
        var reader = _mapper.GetMemberReader<TestInstance, int>(nameof(_instance.IntField));
        // Assert
        Assert.NotNull(reader);
        int source = _instance.IntField!.Value;
        Assert.Equal(source, reader.Read(_instance));
    }
    [Fact]
    public void GetMemberReader_Nint()
    {
        // Act
        var reader = _mapper.GetMemberReader<TestInstance, int?>(nameof(_instance.IntField));
        // Assert
        Assert.NotNull(reader);
        int? source = _instance.IntField;
        Assert.Equal(source, reader.Read(_instance));
    }
    [Fact]
    public void GetMemberReader_Nint2()
    {
        // Act
        var reader = _mapper.GetMemberReader<TestInstance, int?>(nameof(_instance.IntProperty));
        // Assert
        Assert.NotNull(reader);
        int? source = _instance.IntProperty;
        Assert.Equal(source, reader.Read(_instance));
    }
    [Fact]
    public void GetMemberReader_decimal()
    {
        // Act
        var reader = _mapper.GetMemberReader<TestInstance, decimal>(nameof(_instance.DecimalProperty));
        // Assert
        Assert.NotNull(reader);
        decimal source = _instance.DecimalProperty;
        Assert.Equal(source, reader.Read(_instance));
    }
    [Fact]
    public void GetMemberReader_decimal2()
    {
        // Act
        var reader = _mapper.GetMemberReader<TestInstance, decimal>(nameof(_instance.DecimalField));
        // Assert
        Assert.NotNull(reader);
        decimal source = _instance.DecimalField!.Value;
        Assert.Equal(source, reader.Read(_instance));
    }
    [Fact]
    public void GetMemberReader_Ndecimal()
    {
        // Act
        var reader = _mapper.GetMemberReader<TestInstance, decimal?>(nameof(_instance.DecimalField));
        // Assert
        Assert.NotNull(reader);
        decimal? source = _instance.DecimalField;
        Assert.Equal(source, reader.Read(_instance));
    }
    [Fact]
    public void GetMemberReader_Ndecimal2()
    {
        // Act
        var reader = _mapper.GetMemberReader<TestInstance, decimal?>(nameof(_instance.DecimalProperty));
        // Assert
        Assert.NotNull(reader);
        decimal? source = _instance.DecimalProperty;
        Assert.Equal(source, reader.Read(_instance));
    }
    [Fact]
    public void GetMemberReader_string()
    {
        // Act
        var reader = _mapper.GetMemberReader<TestInstance, string>(nameof(_instance.StringField));
        // Assert
        Assert.NotNull(reader);
        string source = _instance.StringField;
        Assert.Equal(source, reader.Read(_instance));
    }
    [Fact]
    public void GetMemberReader_string2()
    {
        // Act
        var reader = _mapper.GetMemberReader<TestInstance, string>(nameof(_instance.StringProperty));
        // Assert
        Assert.NotNull(reader);
        string source = _instance.StringProperty!;
        Assert.Equal(source, reader.Read(_instance));
    }
    [Fact]
    public void GetMemberReader_Nstring()
    {
        // Act
        var reader = _mapper.GetMemberReader<TestInstance, string?>(nameof(_instance.StringProperty));
        // Assert
        Assert.NotNull(reader);
        string? source = _instance.StringProperty;
        Assert.Equal(source, reader.Read(_instance));
    }
    [Fact]
    public void GetMemberReader_Nstring2()
    {
        // Act
        var reader = _mapper.GetMemberReader<TestInstance, string?>(nameof(_instance.StringField));
        // Assert
        Assert.NotNull(reader);
        string? source = _instance.StringField;
        Assert.Equal(source, reader.Read(_instance));
    }
    [Fact]
    public void GetMemberReader_DateTime()
    {
        // Act
        var reader = _mapper.GetMemberReader<TestInstance, DateTime>(nameof(_instance.TimeField));
        // Assert
        Assert.NotNull(reader);
        DateTime source = _instance.TimeField!.Value;
        Assert.Equal(source, reader.Read(_instance));
    }
    [Fact]
    public void GetMemberReader_DateTime2()
    {
        // Act
        var reader = _mapper.GetMemberReader<TestInstance, DateTime>(nameof(_instance.TimeProperty));
        // Assert
        Assert.NotNull(reader);
        DateTime source = _instance.TimeProperty!.Value;
        Assert.Equal(source, reader.Read(_instance));
    }
    [Fact]
    public void GetMemberReader_NDateTime()
    {
        // Act
        var reader = _mapper.GetMemberReader<TestInstance, DateTime?>(nameof(_instance.TimeProperty));
        // Assert
        Assert.NotNull(reader);
        DateTime? source = _instance.TimeProperty;
        Assert.Equal(source, reader.Read(_instance));
    }
    [Fact]
    public void GetMemberReader_NDateTime2()
    {
        // Act
        var reader = _mapper.GetMemberReader<TestInstance, DateTime?>(nameof(_instance.TimeField));
        // Assert
        Assert.NotNull(reader);
        DateTime? source = _instance.TimeField;
        Assert.Equal(source, reader.Read(_instance));
    }
    [Fact]
    public void GetMemberReader_object()
    {
        // Act
        var reader = _mapper.GetMemberReader<TestInstance, object>(nameof(_instance.ObjectProperty));
        // Assert
        Assert.NotNull(reader);
        object source = _instance.ObjectProperty;
        Assert.Equal(source, reader.Read(_instance));
    }
    [Fact]
    public void GetMemberReader_object2()
    {
        // Act
        var reader = _mapper.GetMemberReader<TestInstance, object>(nameof(_instance.ObjectField));
        // Assert
        Assert.NotNull(reader);
        object source = _instance.ObjectField!;
        Assert.Equal(source, reader.Read(_instance));
    }
    [Fact]
    public void GetMemberReader_Nobject()
    {
        // Act
        var reader = _mapper.GetMemberReader<TestInstance, object?>(nameof(_instance.ObjectField));
        // Assert
        Assert.NotNull(reader);
        object? source = _instance.ObjectField;
        Assert.Equal(source, reader.Read(_instance));
    }
    [Fact]
    public void GetMemberReader_Nobject2()
    {
        // Act
        var reader = _mapper.GetMemberReader<TestInstance, object?>(nameof(_instance.ObjectProperty));
        // Assert
        Assert.NotNull(reader);
        object? source = _instance.ObjectProperty;
        Assert.Equal(source, reader.Read(_instance));
    }
}
