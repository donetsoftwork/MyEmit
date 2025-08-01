using PocoEmit.Configuration;
using PocoEmit.Converters;
using System;
using System.Linq.Expressions;
using System.Reflection;

namespace PocoEmit.Members;

/// <summary>
/// 转化实体类型写入成员
/// </summary>
/// <param name="converter"></param>
/// <param name="inner"></param>
public class ConvertInstanceWriter(IEmitConverter converter, IEmitMemberWriter inner)
    : IEmitMemberWriter
{
    #region 配置
    private readonly IEmitConverter _converter = converter;
    /// <summary>
    /// 转换器
    /// </summary>
    public IEmitConverter Converter
        => _converter;
    private readonly IEmitMemberWriter _inner = inner;
    /// <summary>
    /// 内部成员
    /// </summary>
    public IEmitMemberWriter Inner
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
    MemberInfo IEmitMemberWriter.Info
        => _inner.Info;
    /// <inheritdoc />
    bool IEmitInfo.Compiled
        => false;
    #endregion
    /// <inheritdoc />
    public Expression Write(Expression instance, Expression value)
        => _inner.Write(_converter.Convert(instance), value);
}
