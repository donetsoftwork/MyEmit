namespace MapperBench.Supports;

/// <summary>
/// 树
/// </summary>
public class Tree2
{
    public int Id { get; set; }
    public List<TreeRoot2> Roots { get; set; }
    public TreeBranch2 Trunk { get; set; }
}
public class TreeBranch2
{
    public int Id { get; set; }
    public Tree2 Tree { get; set; }
    public TreeBranch2 Parent { get; set; }
    public TreeBranch2[] Branches { get; set; }
    public TreeLeaf2[] Leaves { get; set; }
    public List<TreeFlower2> Flowers { get; set; }
    public List<TreeFruit2> Fruits { get; set; }
}
/// <summary>
/// 树根
/// </summary>
public class TreeRoot2
{
    public int Id { get; set; }
    public Tree2 Tree { get; set; }
    public TreeRoot2 Parent { get; set; }
    public List<TreeRoot2> Roots { get; set; }
}
public class TreeLeaf2
{
    public int Id { get; set; }
    public TreeBranch2 Branch { get; set; }
}
/// <summary>
/// 花
/// </summary>
public class TreeFlower2
{
    public int Id { get; set; }
    public TreeBranch2 Branch { get; set; }
}
/// <summary>
/// 果实
/// </summary>
public class TreeFruit2
{
    public int Id { get; set; }
    public TreeBranch2 Branch { get; set; }
}
/// <summary>
/// 树
/// </summary>
public class TreeDTO2
{
    public int Id { get; set; }
    public List<TreeRootDTO2> Roots { get; set; }
    public TreeBranchDTO2 Trunk { get; set; }
}
public class TreeBranchDTO2
{
    public int Id { get; set; }
    public TreeDTO2 Tree { get; set; }
    public TreeBranchDTO2 Parent { get; set; }
    public TreeBranchDTO2[] Branches { get; set; }
    public TreeLeafDTO2[] Leaves { get; set; }
    public List<TreeFlowerDTO2> Flowers { get; set; }
    public List<TreeFruitDTO2> Fruits { get; set; }
}
/// <summary>
/// 树根
/// </summary>
public class TreeRootDTO2
{
    public int Id { get; set; }
    public TreeDTO2 Tree { get; set; }
    public TreeRootDTO2 Parent { get; set; }
    public List<TreeRootDTO2> Roots { get; set; }
}
public class TreeLeafDTO2
{
    public int Id { get; set; }
    public TreeBranchDTO2 Branch { get; set; }
}
/// <summary>
/// 花
/// </summary>
public class TreeFlowerDTO2
{
    public int Id { get; set; }
    public TreeBranchDTO2 Branch { get; set; }
}
/// <summary>
/// 果实
/// </summary>
public class TreeFruitDTO2
{
    public int Id { get; set; }
    public TreeBranchDTO2 Branch { get; set; }
}
