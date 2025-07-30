using PocoEmit.Configuration;
using PocoEmit.Members;
using System.Linq.Expressions;

namespace PocoEmit.Converters;

/// <summary>
/// 通过成员读取转化
/// </summary>
/// <param name="reader"></param>
public class MemberReadConverter(IEmitMemberReader reader)
    : IEmitConverter
{
    #region 配置
    private readonly IEmitMemberReader _reader = reader;
    /// <summary>
    /// 成员读取器
    /// </summary>
    public IEmitMemberReader Reader
        => _reader;
    /// <inheritdoc />
    bool IEmitInfo.Compiled
        => false;
    #endregion
    /// <inheritdoc />
    public Expression Convert(Expression source)
        => _reader.Read(source);
}
