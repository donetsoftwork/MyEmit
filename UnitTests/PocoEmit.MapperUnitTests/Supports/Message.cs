namespace PocoEmit.MapperUnitTests.Supports;

internal class MessageDto
{
    public string Message { get; set; }
}
internal class MessageEntity
{
    public IMapper Mapper { get; set; }
    public string Message { get; set; }
}

internal class MessageEntity2(IMapper mapper, string message)
{
    public IMapper Mapper { get;  } = mapper;
    public string Message { get; set; } = message;
}

internal class MessageDomain(MessageId id, string message)
{
    public MessageId Id { get; } = id;
    public string Message { get; } = message;
    // ...
}
internal class MessageId(int id)
{
    private static int seed = 1;
    public int Id { get; } = id;
    public static MessageId NewId()
        => new(seed++);
}
