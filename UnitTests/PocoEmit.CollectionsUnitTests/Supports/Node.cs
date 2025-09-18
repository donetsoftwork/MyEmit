namespace PocoEmit.CollectionsUnitTests.Supports;

public class Node
{
    public Node Parent { get; set; }
    public NodeId Id { get; set; }
    public string Name { get; set; }
    public int SortOrder { get; set; }
    public Leaf[] Leaves { get; set; }
}

public class NodeId(int value)
{
    public int Value { get;  } = value;
}

public class NodeDTO
{
    public NodeDTO Parent { get; set; }
    public int Id { get; set; }
    public string Name { get; set; }

    public int SortOrder { get; set; }

    public LeafDTO[] Leaves { get; set; }
}