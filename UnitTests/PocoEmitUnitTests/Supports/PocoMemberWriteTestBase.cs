namespace PocoEmitUnitTests.Supports;

/// <summary>
/// 写入成员测试基类
/// </summary>
public abstract class PocoMemberWriteTestBase
    : PocoMemberTestBase
{
    #region expected
    #region Properties
    public const int IntProperty = 33;
    public const string? StringProperty = nameof(StringProperty);
    public const decimal DecimalProperty = 3.3m;
    public static readonly DateTime? TimeProperty = new DateTime(2025, 6, 1);
    public static readonly object ObjectProperty = new();
    #endregion
    #region Fields
    public static readonly int? IntField = 44;
    public const string StringField = nameof(StringField);
    public static readonly decimal? DecimalField = 4.4m;
    public static readonly DateTime? TimeField = new DateTime(2025, 7, 1);
    public static readonly object? ObjectField = new();
    #endregion
    #endregion
}
