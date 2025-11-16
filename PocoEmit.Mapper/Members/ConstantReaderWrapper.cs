using Hand.Structural;
using PocoEmit.Builders;
using PocoEmit.Configuration;
using System.Linq.Expressions;

namespace PocoEmit.Members;

/// <summary>
/// 常量包装成员读取器
/// </summary>
/// <param name="original"></param>
public class ConstantReaderWrapper(ConstantExpression original)
    : IEmitReader, IWrapper<ConstantExpression>
{
    #region 配置
    private readonly ConstantExpression _original = original;
    /// <inheritdoc />
    public ConstantExpression Original
        => _original;
    /// <inheritdoc />
    bool ICompileInfo.Compiled
        => false;
    #endregion
    /// <inheritdoc />
    public Expression Read(Expression instance)
        => _original;
}
