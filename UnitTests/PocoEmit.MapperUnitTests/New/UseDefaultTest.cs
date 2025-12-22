using PocoEmit.MapperUnitTests.Supports;

namespace PocoEmit.MapperUnitTests.New;

public class UseDefaultTest
{
    [Fact]
    public void UseMapper()
    {
        var mapper = Mapper.Create();
        var dto = new MessageDto { Message = "Hello UseMapper" };
        MessageEntity message = mapper.Convert<MessageDto, MessageEntity>(dto);
        Assert.NotNull(message);
        Assert.NotNull(message.Mapper);
    }
    [Fact]
    public void UseMapper2()
    {
        var mapper = Mapper.Create();
        var dto = new MessageDto { Message = "Hello UseMapper2" };
        MessageEntity2 message = mapper.Convert<MessageDto, MessageEntity2>(dto);
        Assert.NotNull(message);
        Assert.NotNull(message.Mapper);
    }
    [Fact]
    public void UseMessageId()
    {
        IMapper mapper = Mapper.Create()
            .UseDefault(() => MessageId.NewId());
        var dto = new MessageDto { Message = "Hello UseMessageId" };
        MessageDomain message = mapper.Convert<MessageDto, MessageDomain>(dto);
        Assert.NotNull(message);
        Assert.NotNull(message.Id);
    }
}
