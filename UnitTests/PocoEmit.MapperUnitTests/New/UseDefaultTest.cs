namespace PocoEmit.MapperUnitTests.New;

public class UseDefaultTest
{
    [Fact]
    public void UseDefault()
    {
        IMapper mapper = Mapper.Create()
            .UseDefault(() => MessageId.NewId());
        var dto = new MessageDto { Message = "Hello UseDefault" };
        MessageDomain message = mapper.Convert<MessageDto, MessageDomain>(dto);
        Assert.NotNull(message);
    }

    class MessageDto
    {
        public string Message { get; set; }
    }
    class MessageDomain(MessageId id, string message)
    {
        public MessageId Id { get; } = id;
        public string Message { get; } = message;
        // ...
    }
    class MessageId(int id)
    {
        private static int seed = 1;
        public int Id { get; } = id;
        public static MessageId NewId()
            =>new(seed++);
    }
}
