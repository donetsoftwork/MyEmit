namespace PocoEmit.Enums;

#if NET7_0_OR_GREATER
/// <summary>
/// 枚举字段映射
/// </summary>
/// <param name="SourceField"></param>
/// <param name="DestField"></param>
public record EnumFieldPair(IEnumField SourceField, IEnumField DestField)
{
#else
/// <summary>
/// 枚举字段映射
/// </summary>
/// <param name="sourceField"></param>
/// <param name="destField"></param>
public class EnumFieldPair(IEnumField sourceField, IEnumField destField)
     : System.IEquatable<EnumFieldPair>
{
    #region 配置
    private readonly IEnumField _sourceField = sourceField;
    private readonly IEnumField _destField = destField;
    /// <summary>
    /// 映射源类型
    /// </summary>
    public IEnumField SourceField
        => _sourceField;
    /// <summary>
    /// 映射目标类型
    /// </summary>
    public IEnumField DestField
        => _destField;
    #endregion
    /// <summary>
    /// HashCode
    /// </summary>
    /// <returns></returns>
    public override int GetHashCode()
#if (NETSTANDARD1_1 || NETSTANDARD1_3 || NETSTANDARD1_6 || NET45)
        => _sourceField.GetHashCode() ^ _destField.GetHashCode();
#else
        => System.HashCode.Combine(_sourceField, _destField);
#endif
    #region IEquatable
    /// <summary>
    /// 判同
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool Equals(EnumFieldPair other)
        => _sourceField.Equals(other._sourceField) && _destField.Equals(other._destField);
    /// <summary>
    /// 判同
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public override bool Equals(object other)
        => other is EnumFieldPair key && Equals(key);
    #endregion
#endif
}