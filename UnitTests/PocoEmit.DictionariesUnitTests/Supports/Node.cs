namespace PocoEmit.DictionariesUnitTests.Supports;

public class Node
{
    public Node Parent { get; set; }
    public NodeId Id { get; set; }
    public string Name { get; set; }
    public int SortOrder { get; set; }
}

public class NodeId(int value)
{
    public int Value { get; } = value;
}