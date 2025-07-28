namespace PocoEmit.MapperUnitTests.Supports;

public class TestInstance
{
    #region Properties
    public int IntProperty { get; set; } = 11;
    public string? StringProperty { get; set; } = nameof(StringProperty);
    public decimal DecimalProperty { get; set; } = 1.1m;
    public DateTime? TimeProperty { get; set; } = new DateTime(2025, 7, 24);
    public object ObjectProperty { get; set; } = new();
    #endregion
    #region Fields
    public int? IntField = 22;
    public string StringField = nameof(StringField);
    public decimal? DecimalField = 2.3m;
    public DateTime? TimeField = new DateTime(2025, 7, 25);
    public object? ObjectField = new();
    #endregion
}
