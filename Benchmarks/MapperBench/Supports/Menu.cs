namespace MapperBench.Supports;

public class Menu
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public List<Menu> Children { get; set; }
    public static Menu GetMenu()
    {
        var programs = new Menu { Id = 2, Name = "Programs", Description = "程序" };
        var documents = new Menu { Id = 3, Name = "Documents", Description = "文档" };
        var settings = new Menu { Id = 4, Name = "Settings", Description = "设置" };
        var help = new Menu { Id = 5, Name = "Help", Description = "帮助" };
        var run = new Menu { Id = 6, Name = "Run", Description = "运行" };
        var shutdown = new Menu { Id = 7, Name = "Shut Down", Description = "关闭" };
        var start = new Menu { Id = 1, Name = "Start", Description = "开始", Children = [programs, documents, settings, help, run, shutdown] };
        return start;
    }
}
public class MenuDTO
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public List<MenuDTO> Children { get; set; }
}