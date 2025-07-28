using PocoEmit.Configuration;
using PocoEmit.Copies;
using PocoEmit.Members;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PocoEmit.Copies;

/// <summary>
/// 成员复制器
/// </summary>
/// <param name="sourceReader"></param>
/// <param name="destReader"></param>
/// <param name="inner"></param>
public class MemberCopier(IEmitReader sourceReader, IEmitReader destReader, IEmitCopier inner)
    : IMemberConverter
{
    #region 配置
    private readonly IEmitReader _sourceReader = sourceReader;
    private readonly IEmitReader _destReader = destReader;
    private readonly IEmitCopier _inner = inner;
    /// <summary>
    /// 源成员读取器
    /// </summary>
    public IEmitReader SourceReader
        => _sourceReader;
    /// <summary>
    /// 目标成员写入器
    /// </summary>
    public IEmitReader DestReader
        => _destReader;
    /// <summary>
    /// 内部复制器
    /// </summary>
    public IEmitCopier Inner
        => _inner;
    #endregion
    /// <inheritdoc />
    public Expression Convert(Expression source, Expression dest)
    {
        var sourceMember = _sourceReader.Read(source);
        var destMember = _destReader.Read(dest);
        var list = _inner.Copy(sourceMember, destMember).ToArray();
        if (list.Length == 0)
            return Expression.Empty();
        else if (list.Length == 1)
            return list[0];
        return Expression.Block(list);
    }
}
