namespace PocoEmit.MapperUnitTests.Supports;

public abstract class MapHelperBaseTests
{

    internal class UserCustomDTO(string userName)
    {
        public int? UserId { get; set; }

        public string UserName { get; } = userName;
    }
}
