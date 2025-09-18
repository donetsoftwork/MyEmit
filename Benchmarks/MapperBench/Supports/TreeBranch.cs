namespace MapperBench.Supports;

public class TreeBranch
{
    public int Id { get; set; }
    public TreeBranch[] Branches { get; set; }
    public TreeLeaf[] Leaves { get; set; }
}
public class TreeLeaf
{
    public int Id { get; set; }
}

public class TreeBranchDTO
{
    public int Id { get; set; }
    public TreeBranchDTO[] Branches { get; set; }
    public TreeLeafDTO[] Leaves { get; set; }
}
public class TreeLeafDTO
{
    public int Id { get; set; }
}
