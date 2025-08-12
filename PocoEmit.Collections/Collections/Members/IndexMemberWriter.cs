using PocoEmit.Configuration;
using PocoEmit.Indexs;
using PocoEmit.Members;
using System.Linq.Expressions;

namespace PocoEmit.Collections.Members;

/// <summary>
/// 索引成员读取器
/// </summary>
/// <param name="writer"></param>
/// <param name="index"></param>
public class IndexMemberWriter(IEmitIndexMemberWriter writer, Expression index)
    : IEmitWriter
{
    #region 配置
    private readonly IEmitIndexMemberWriter _writer = writer;
    private readonly Expression _index = index;
    /// <summary>
    /// 读取器
    /// </summary>
    public IEmitIndexMemberWriter Writer
        => _writer;
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
    public Expression Write(Expression instance, Expression value)
        => _writer.Write(instance, _index, value);
}
