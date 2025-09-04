public class Student
{
    public User User { get; set; }
    public string Role { get; set; }
    public int Age { get; set; }

    public Dictionary<string, int> Score { get; set; }

    public Dictionary<string, string> Skill { get; set; }
}

public class User
{
    public int Id { get; set; }
    public string Name { get; set; }
}