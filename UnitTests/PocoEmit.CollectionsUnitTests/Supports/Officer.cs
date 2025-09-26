namespace PocoEmit.CollectionsUnitTests.Supports;

/// <summary>
/// 军官
/// </summary>
public class Officer
{
    public string Name { get; set; }
    public int Level { get; set; }
    /// <summary>
    /// 部队
    /// </summary>
    /// <returns></returns>
    public static Dictionary<Officer, Officer> GetArmy()
    {
        var officer1 = new Officer { Name = "司令", Level = 1 };
        var officer2 = new Officer { Name = "1军军长", Level = 2 };
        var officer3 = new Officer { Name = "2军军长", Level = 2 };
        var officer21 = new Officer { Name = "101师师长", Level = 3 };
        var officer22 = new Officer { Name = "102师师长", Level = 3 };
        var officer31 = new Officer { Name = "201师师长", Level = 3 };
        var officer32 = new Officer { Name = "202师师长", Level = 3 };
        var army = new Dictionary<Officer, Officer>()
        {
            { officer2 , officer1 },
            { officer21 , officer2 },
            { officer22 , officer2 },
            { officer3 , officer1 },
            { officer31 , officer3 },
            { officer32 , officer3 }
        };
        return army;
    }
}

public class OfficerDTO
{
    public string Name { get; set; }
    public string Level { get; set; }
}
