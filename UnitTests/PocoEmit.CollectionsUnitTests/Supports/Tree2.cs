namespace PocoEmit.CollectionsUnitTests.Supports;

public class Tree2
{
    public int Id { get; set; }
    public TreeBranch2 Trunk { get; set; }
}
public class TreeBranch2
{
    public int Id { get; set; }
    public Tree2 Tree { get; set; }
    public TreeBranch2 Parent { get; set; }
    public TreeBranch2[] Branches { get; set; }
    public TreeLeaf2[] Leaves { get; set; }
}
public class TreeLeaf2
{
    public int Id { get; set; }
    public TreeBranch2 Branch { get; set; }
}
public class TreeDTO2
{
    public int Id { get; set; }
    public TreeBranchDTO2 Trunk { get; set; }
}
public class TreeBranchDTO2
{
    public int Id { get; set; }
    public TreeDTO2 Tree { get; set; }
    public TreeBranchDTO2 Parent { get; set; }
    public TreeBranchDTO2[] Branches { get; set; }
    public TreeLeafDTO2[] Leaves { get; set; }
}
public class TreeLeafDTO2
{
    public int Id { get; set; }
    public TreeBranchDTO2 Branch { get; set; }
}
