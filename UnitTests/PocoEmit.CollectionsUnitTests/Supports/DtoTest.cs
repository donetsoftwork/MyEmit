namespace PocoEmit.CollectionsUnitTests.Supports;

public class DtoTest
{
    public int? Id { get; set; }
    public string Name { get; set; }
    public List<ChildDto<DtoTest>> Items { get; set; }
    public List<string> Tags { get; set; }
    public Dictionary<string, int?> Dict { get; set; }
    public TestEnum TestEnum { get; set; }
    public int? TestEnum2 { get; set; }
    public DtoTest This { get; set; }


    public static DtoTest GetTest()
    {
        var test = new DtoTest
        {
            Id = 123,
            Name = "hello world",
            Tags = ["a", "b", "c"],
            Items =
                [
                    new() { Key = "k1", Value = 42 },
                        new() { Key = "k2", Value = 100 }
                ],
            Dict = new Dictionary<string, int?> { { "x", 1 }, { "y", 2 } },
            TestEnum = TestEnum.Sale,
            TestEnum2 = null
        };
        test.This = test;
        test.Items.Add(test.Items[0]);
        foreach (var item in test.Items)
        {
            item.Mother = test;
        }
        test.Id = 222;
        test.Name = "bbb";
        test.Tags[0] = "d";
        test.Items.First().Key = "k0";
        test.Dict["x"] = -1;
        return test;
    }
}
public class DtoTest2
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<ChildDto<DtoTest2>?>? Items { get; set; }
    public List<string> Tags { get; set; }
    public Dictionary<string, int> Dict { get; set; }
    public int? TestEnum { get; set; }
    public TestEnum TestEnum2 { get; set; }
    public DtoTest2 This { get; set; }
}
public class ChildDto<T>
{
    public string Key { get; set; }
    public int Value { get; set; }
    public T Mother { get; set; }
}
public enum TestEnum
{
    Take = 0,
    Sale = 1,
    Pull = 2
}