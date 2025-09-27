namespace MapperBench.Supports;

public class Menu0
{
    public int ParentId { get; set; }
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }

    public static List<Menu0> GetMenus()
    {
        var start = new Menu0 { Id = 1, Name = "Start", Description = "开始", ParentId = 0 };
        var programs = new Menu0 { Id = 2, Name = "Programs", Description = "程序", ParentId = 1 };
        var documents = new Menu0 { Id = 3, Name = "Documents", Description = "文档", ParentId = 1 };
        var settings = new Menu0 { Id = 4, Name = "Settings", Description = "设置", ParentId = 1 };
        var help = new Menu0 { Id = 5, Name = "Help", Description = "帮助", ParentId = 1 };
        var run = new Menu0 { Id = 6, Name = "Run", Description = "运行" , ParentId = 1 };
        var shutdown = new Menu0 { Id = 7, Name = "Shut Down", Description = "关闭", ParentId = 1 };
        return [start, programs, documents, settings, help, run, shutdown];
    }
}
public class Menu0DTO
{
    public int ParentId { get; set; }
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
}

