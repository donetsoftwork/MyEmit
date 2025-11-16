using Hand.Reflection;
using PocoEmit.Complexes;
using PocoEmit.Configuration;
using PocoEmit.Members;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace PocoEmit.Converters;

/// <summary>
/// 通过成员读取转化
/// </summary>
/// <param name="key"></param>
/// <param name="reader"></param>
public class MemberReadConverter(in PairTypeKey key, IEmitMemberReader reader)
    : IEmitConverter
    , IComplexPreview
{
    #region 配置
    private readonly PairTypeKey _key = key;
    private readonly IEmitMemberReader _reader = reader;
    /// <inheritdoc />
    public PairTypeKey Key
        => _key;
    /// <summary>
    /// 成员读取器
    /// </summary>
    public IEmitMemberReader Reader
        => _reader;
    /// <inheritdoc />
    bool ICompileInfo.Compiled
        => false;
    #endregion
    /// <inheritdoc />
    void IComplexPreview.Preview(IComplexBundle parent)
        => parent.Visit(_reader);
    /// <inheritdoc />
    public Expression Convert(Expression source)
        => _reader.Read(source);
}
