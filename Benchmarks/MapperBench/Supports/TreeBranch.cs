namespace MapperBench.Supports;

/// <summary>
/// 树
/// </summary>
public class Tree
{
    public int Id { get; set; }
    public List<TreeRoot> Roots { get; set; }
    public TreeBranch Trunk { get; set; }
}
/// <summary>
/// 树根
/// </summary>
public class TreeRoot
{
    public int Id { get; set; }
    public List<TreeRoot> Roots { get; set; }
}
/// <summary>
/// 树枝
/// </summary>
public class TreeBranch
{
    public int Id { get; set; }
    public List<TreeBranch> Branches { get; set; }
    public List<Leaf> Leaves { get; set; }
    public List<Flower> Flowers { get; set; }
    public List<Fruit> Fruits { get; set; }
}
/// <summary>
/// 叶子
/// </summary>
public class Leaf
{
    public int Id { get; set; }
}
/// <summary>
/// 花
/// </summary>
public class Flower
{
    public int Id { get; set; }
}
/// <summary>
/// 果实
/// </summary>
public class Fruit
{
    public int Id { get; set; }
}
/// <summary>
/// 树
/// </summary>
public class TreeDTO
{
    public int Id { get; set; }
    public List<TreeRootDTO> Roots { get; set; }
    public TreeBranchDTO Trunk { get; set; }
}
/// <summary>
/// 树根
/// </summary>
public class TreeRootDTO
{
    public int Id { get; set; }
    public List<TreeRootDTO> Roots { get; set; }
}
/// <summary>
/// 树枝
/// </summary>
public class TreeBranchDTO
{
    public int Id { get; set; }
    public List<TreeBranchDTO> Branches { get; set; }
    public List<LeafDTO> Leaves { get; set; }

    public List<FlowerDTO> Flowers { get; set; }
    public List<FruitDTO> Fruits { get; set; }
}
/// <summary>
/// 叶子
/// </summary>
public class LeafDTO
{
    public int Id { get; set; }
}
/// <summary>
/// 花
/// </summary>
public class FlowerDTO
{
    public int Id { get; set; }
}
/// <summary>
/// 果实
/// </summary>
public class FruitDTO
{
    public int Id { get; set; }
}