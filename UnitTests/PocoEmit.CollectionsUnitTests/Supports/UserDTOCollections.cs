using System.Collections.Concurrent;

namespace PocoEmit.CollectionsUnitTests.Supports;

public class UserDTOArray
{
    public string Name { get; set; }

    public UserDTO[] Users { get; set; }
}

public class UserDTOList
{
    public string Name { get; set; }

    public List<UserDTO> Users { get; set; }
}

public class UserDTOHashSet
{
    public string Name { get; set; }

    public HashSet<UserDTO> Users { get; set; }
}

public class UserDTOQueue
{
    public string Name { get; set; }

    public Queue<UserDTO> Users { get; set; }
}

public class UserDTOConcurrentQueue
{
    public string Name { get; set; }

    public ConcurrentQueue<UserDTO> Users { get; set; }
}

public class UserDTOStack
{
    public string Name { get; set; }

    public Stack<UserDTO> Users { get; set; }
}
public class UserDTOConcurrentStack
{
    public string Name { get; set; }

    public ConcurrentStack<UserDTO> Users { get; set; }
}
public class UserDTOBlockingCollection
{
    public string Name { get; set; }

    public BlockingCollection<UserDTO> Users { get; set; }
}

public class UserDTOConcurrentBag
{
    public string Name { get; set; }

    public ConcurrentBag<UserDTO> Users { get; set; }
}
public class UserDTODictionary
{
    public string Name { get; set; }

    public Dictionary<int, UserDTO> Users { get; set; }
}
public class UserDTOConcurrentDictionary
{
    public string Name { get; set; }

    public ConcurrentDictionary<string, UserDTO> Users { get; set; }
}
public class UserDTOIDictionary
{
    public string Name { get; set; }

    public IDictionary<string, UserDTO> Users { get; set; }
}
public class UserDTOIList
{
    public string Name { get; set; }

    public IList<UserDTO> Users { get; set; }
}

public class UserDTOICollection
{
    public string Name { get; set; }

    public ICollection<UserDTO> Users { get; set; }
}

public class UserDTOIEnumerable
{
    public string Name { get; set; }

    public IEnumerable<UserDTO> Users { get; set; }
}
public class UserDTOISet
{
    public string Name { get; set; }

    public ISet<UserDTO> Users { get; set; }
}
public class UserDTOIProducerConsumerCollection
{
    public string Name { get; set; }

    public IProducerConsumerCollection<UserDTO> Users { get; set; }
}