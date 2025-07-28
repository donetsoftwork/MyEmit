using PocoEmit.Configuration;
using PocoEmit.Converters;
using System;
using System.Linq.Expressions;

namespace PocoEmit.Members;

/// <summary>
/// 转化实体类型读成员
/// </summary>
/// <param name="converter"></param>
/// <param name="inner"></param>
public class ConvertInstanceReader(IEmitConverter converter, IEmitMemberReader inner)
    : IEmitMemberReader
{
    #region 配置
    private readonly IEmitConverter _converter = converter;
    /// <summary>
    /// 转换器
    /// </summary>
    public IEmitConverter Converter
        => _converter;
    private readonly IEmitMemberReader _inner = inner;
    /// <summary>
    /// 内部成员
    /// </summary>
    public IEmitMemberReader Inner
        => _inner;
    /// <inheritdoc />
    public Type InstanceType
        => _inner.InstanceType;
    /// <inheritdoc />
    public string Name 
        => _inner.Name;
    /// <inheritdoc />
    public Type ValueType 
        => _inner.ValueType;
    /// <inheritdoc />
    bool IEmitInfo.Compiled
        => false;
    #endregion
    /// <inheritdoc />
    public Expression Read(Expression instance)
        => _inner.Read(_converter.Convert(instance));
}
