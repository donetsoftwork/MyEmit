namespace MapperBench.Supports;

public class SoldierTeam
{
    public Soldier Leader { get; set; }
    public Soldier Courier { get; set; }
    public List<Soldier> Members { get; set; }
    public static SoldierTeam GetTeam()
    {
        var leader = new Soldier { Name = "张三" };
        var courier = new Soldier { Name = "李四" };
        var other = new Soldier { Name = "王二" };
        var team = new SoldierTeam
        {
            Leader = leader,
            Courier = courier,
            Members = new List<Soldier>
            {
                leader,
                courier,
                other
            }
        };
        return team;
    }
}
public class Soldier
{
    public string Name { get; set; }
}

public class SoldierTeamDTO
{
    public SoldierDTO Leader { get; set; }
    public SoldierDTO Courier { get; set; }
    public List<SoldierDTO> Members { get; set; }
}

public class SoldierDTO
{
    public string Name { get; set; }
}