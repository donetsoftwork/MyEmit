using PocoEmit.Configuration;
using PocoEmit.Indexs;
using PocoEmit.Members;
using System.Linq.Expressions;

namespace PocoEmit.Collections.Members;

/// <summary>
/// 索引成员读取器
/// </summary>
/// <param name="reader"></param>
/// <param name="index"></param>
public class IndexMemberReader(IEmitIndexMemberReader reader, Expression index)
    : IEmitReader
{
    #region 配置
    private readonly IEmitIndexMemberReader _reader = reader;
    private readonly Expression _index = index;
    /// <summary>
    /// 读取器
    /// </summary>
    public IEmitIndexMemberReader Reader
        => _reader;
    /// <summary>
    /// 索引值
    /// </summary>
    public Expression Index
        => _index;
    /// <inheritdoc />
    bool ICompileInfo.Compiled
        => false;
    #endregion
    /// <inheritdoc />
    public Expression Read(Expression instance)
        => _reader.Read(instance, _index);
}
