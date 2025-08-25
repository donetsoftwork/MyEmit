using PocoEmit;
using PocoEmitUnitTests.Supports;

namespace PocoEmitUnitTests.Enums;

public class EnumerationTests : PocoConvertTestBase
{
    [Fact]
    public void ShouldMapSharedEnum()
    {
        var dto = _poco.Convert<Status, Status>(Status.InProgress);
        Assert.Equal(Status.InProgress, dto);
    }

    [Fact]
    public void ShouldMapToUnderlyingType()
    {
        var dto = _poco.Convert<Status, int>(Status.InProgress);
        Assert.Equal(1, dto);
    }

    [Fact]
    public void ShouldMapToStringType() 
    {
        var dto = _poco.Convert<Status, string>(Status.InProgress);
        Assert.Equal("InProgress", dto);
    }

    [Fact]
    public void ShouldMapFromUnderlyingType()
    {
        var dto = _poco.Convert<int, Status>(1);
        Assert.Equal(Status.InProgress, dto);
    }

    [Fact]
    public void ShouldMapFromStringType() 
    {
        var dto = _poco.Convert<string, Status>("InProgress");
        Assert.Equal(Status.InProgress, dto);
    }
    
    [Fact]
    public void ShouldMapEnumByMatchingNames()
    {
        var dto = _poco.Convert<Status, StatusForDto>(Status.InProgress);
        Assert.Equal(StatusForDto.InProgress, dto);
    }

    [Fact]
    public void ShouldMapSharedNullableEnum() 
    {
        Status? status = Status.InProgress;
        var dto = _poco.Convert<Status?, Status?>(status);
        Assert.Equal(Status.InProgress, dto);
    }

    [Fact]
    public void ShouldMapNullableEnumToNullWhenSourceEnumIsNull() 
    {
        Status? status = null;
        var dto = _poco.Convert<Status?, Status?>(status);
        Assert.Null(dto);
    }

    [Fact]
    public void ShouldMapEnumWithInvalidValue()
    {
        Status status = 0;
        var dto = _poco.Convert<Status, StatusForDto>(status);
        var expected = (StatusForDto)0;
        Assert.Equal(expected, dto);
    }
    #region 实体   
    public enum Status
    {
        InProgress = 1,
        Complete = 2
    }

    public enum StatusForDto
    {
        InProgress = 1,
        Complete = 2
    }
    #endregion
}
