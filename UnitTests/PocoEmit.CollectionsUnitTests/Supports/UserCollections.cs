namespace PocoEmit.CollectionsUnitTests.Supports;

public class UserArray
{
    public string Name { get; set; }

    public User[] Users { get; set; }
}

public class UserList
{
    public string Name { get; set; }

    public List<User> Users { get; set; }
}

public class UserDictionary
{
    public string Name { get; set; }

    public Dictionary<int, User> Users { get; set; }
}

public class UserIDictionary
{
    public string Name { get; set; }

    public IDictionary<int, User> Users { get; set; }
}

public class UserIList
{
    public string Name { get; set; }

    public IList<User> Users { get; set; }
}

public class UserICollection
{
    public string Name { get; set; }

    public ICollection<User> Users { get; set; }
}

public class UserIEnumerable
{
    public string Name { get; set; }
    public IEnumerable<User> Users { get; set; }
}
