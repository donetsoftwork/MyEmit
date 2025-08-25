using PocoEmit;
using PocoEmit.MapperUnitTests.Supports;

namespace PocoEmit.MapperUnitTests.Enums;

public class EnumerationTests : MapperConvertTestBase
{
    [Fact]
    public void ShouldMapSharedEnum()
    {
        var order = new Order
        {
            Status = Status.InProgress
        };
        var dto = _mapper.Convert<Order, OrderDto>(order);
        Assert.Equal(Status.InProgress, dto.Status);
    }

    [Fact]
    public void ShouldMapToUnderlyingType()
    {
        var order = new Order {
            Status = Status.InProgress
        };
        var dto = _mapper.Convert<Order, OrderDtoInt>(order);
        Assert.Equal(1, dto.Status);
    }

    [Fact]
    public void ShouldMapToStringType() 
    {
        var order = new Order {
            Status = Status.InProgress
        };
        var dto = _mapper.Convert<Order, OrderDtoString>(order);
        Assert.Equal("InProgress", dto.Status);
    }

    [Fact]
    public void ShouldMapFromUnderlyingType()
    {
        var order = new OrderDtoInt {
            Status = 1
        };
        var dto = _mapper.Convert<OrderDtoInt, Order>(order);
        Assert.Equal(Status.InProgress, dto.Status);
    }

    [Fact]
    public void ShouldMapFromStringType() 
    {
        var order = new OrderDtoString {
            Status = "InProgress"
        };
        var dto = _mapper.Convert<OrderDtoString, Order>(order);
        Assert.Equal(Status.InProgress, dto.Status);
    }
    
    [Fact]
    public void ShouldMapEnumByMatchingNames()
    {
        var order = new Order
            {
                Status = Status.InProgress
            };

        var dto = _mapper.Convert<Order, OrderDtoWithOwnStatus>(order);
        Assert.Equal(StatusForDto.InProgress, dto.Status);
    }

    [Fact]
    public void ShouldMapEnumByMatchingValues()
    {
        var order = new Order
            {
                Status = Status.InProgress
            };
        var dto = _mapper.Convert<Order, OrderDtoWithOwnStatus>(order);
        Assert.Equal(StatusForDto.InProgress, dto.Status);
    }

    [Fact]
    public void ShouldMapSharedNullableEnum() 
    {
        var order = new OrderWithNullableStatus {
            Status = Status.InProgress
        };
        var dto = _mapper.Convert<OrderWithNullableStatus, OrderDtoWithNullableStatus>(order);
        Assert.Equal(Status.InProgress, dto.Status);;
    }

    [Fact]
    public void ShouldMapNullableEnumByMatchingValues() 
    {
        var order = new OrderWithNullableStatus {
            Status = Status.InProgress
        };
        var dto = _mapper.Convert<OrderWithNullableStatus, OrderDtoWithOwnNullableStatus>(order);
        Assert.Equal(StatusForDto.InProgress, dto.Status);
    }

    [Fact]
    public void ShouldMapNullableEnumToNullWhenSourceEnumIsNullAndDestinationWasNotNull() 
    {
        var source = new OrderWithNullableStatus
        {
            Status = null
        };
        var dest = new OrderDtoWithOwnNullableStatus
        {
            Status = StatusForDto.Complete
        };
        _mapper.Copy(source, dest);
        Assert.Null(dest.Status);
    }

    [Fact]
    public void ShouldMapNullableEnumToNullWhenSourceEnumIsNull() 
    {
        var order = new OrderWithNullableStatus {
            Status = null
        };
        var dto = _mapper.Convert<OrderWithNullableStatus, OrderDtoWithOwnNullableStatus>(order);
        Assert.Null(dto.Status);
    }

    [Fact]
    public void ShouldMapEnumUsingCustomResolver()
    {
        var order = new Order
            {
                Status = Status.InProgress
            };
        var mappedDto = _mapper.Convert<Order, OrderDtoWithOwnStatus>(order);
        Assert.Equal(StatusForDto.InProgress, mappedDto.Status);
    }

    [Fact]
    public void ShouldMapEnumUsingGenericEnumResolver()
    {
        var order = new Order
            {
                Status = Status.InProgress
            };
        var mappedDto = _mapper.Convert<Order, OrderDtoWithOwnStatus>(order);
        Assert.Equal(StatusForDto.InProgress, mappedDto.Status);
    }

    [Fact]
    public void ShouldMapEnumWithInvalidValue()
    {
        var order = new Order
        {
            Status = 0
        };
        var dto = _mapper.Convert<Order, OrderDtoWithOwnStatus>(order);
        var expected = (StatusForDto)0;
        Assert.Equal(expected, dto.Status);
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

    public class Order
    {
        public Status Status { get; set; }
    }

    public class OrderDto
    {
        public Status Status { get; set; }
    }

    public class OrderDtoInt {
        public int Status { get; set; }
    }

    public class OrderDtoString {
        public string Status { get; set; }
    }

    public class OrderDtoWithOwnStatus
    {
        public StatusForDto Status { get; set; }
    }

    public class OrderWithNullableStatus 
    {
        public Status? Status { get; set; }
    }

    public class OrderDtoWithNullableStatus 
    {
        public Status? Status { get; set; }
    }

    public class OrderDtoWithOwnNullableStatus 
    {
        public StatusForDto? Status { get; set; }
    }
    #endregion
}
