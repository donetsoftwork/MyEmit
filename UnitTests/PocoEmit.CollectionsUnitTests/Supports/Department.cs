namespace PocoEmit.CollectionsUnitTests.Supports;

public class Department
{
    public string Name { get; set; }
    public Department[] Children { get; set; }
}

public class DepartmentDTO
{
    public string Name { get; set; }
    public DepartmentDTO[] Children { get; set; }
}
