using PocoEmit.Configuration;
using System.Linq.Expressions;

namespace PocoEmit.Converters;

/// <summary>
/// 相同类型直接跳过转化
/// </summary>
public sealed class PassConverter : IEmitConverter
{
    private PassConverter() { }
    bool ICompileInfo.Compiled
        => false;

    /// <inheritdoc />
    public Expression Convert(Expression source)
        => source;
    /// <summary>
    /// 单例
    /// </summary>
    public static readonly PassConverter Instance = new();
}
