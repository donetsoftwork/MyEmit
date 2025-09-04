namespace PocoEmit.CollectionsUnitTests.Supports;

public class Department
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int ParentId { get; set; }
    public string ParenName { get; set; }
    public int[] ChildIds { get; set; }
}
