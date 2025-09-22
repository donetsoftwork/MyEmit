namespace MapperBench.Supports;

public class Node
{
    public int Id { get; set; }
    public string Name { get; set; }
    public Node Next { get; set; }
    public static Node GetNode()
    {
        Node node9 = new() { Id = 9, Name = "node9" };
        Node node8 = new() { Id = 8, Name = "node8", Next = node9 };
        Node node7 = new() { Id = 7, Name = "node7", Next = node8 };
        Node node6 = new() { Id = 6, Name = "node6", Next = node7 };
        Node node5 = new() { Id = 5, Name = "node5", Next = node6 };
        Node node4 = new() { Id = 4, Name = "node4", Next = node5 };
        Node node3 = new() { Id = 3, Name = "node3", Next = node4 };
        Node node2 = new() { Id = 2, Name = "node2", Next = node3 };
        Node node1 = new() { Id = 1, Name = "node1", Next = node2 };
        node9.Next = node1; // 形成环
        return node1;
    }
}


public class NodeDTO
{
    public int Id { get; set; }
    public string Name { get; set; }
    public NodeDTO Next { get; set; }
}