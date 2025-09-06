namespace MapperBench.Supports;

public class Node
{
    public Node Parent { get; set; }
    public int Id { get; set; }
    public string Name { get; set; }

    public int SortOrder { get; set; }
}


public class NodeDTO
{
    public NodeDTO Parent { get; set; }
    public int Id { get; set; }
    public string Name { get; set; }

    public int SortOrder { get; set; }
}