using PocoEmit.Configuration;
using PocoEmit.Converters;
using System;
using System.Linq.Expressions;
using System.Reflection;

namespace PocoEmit.Members;

/// <summary>
/// 读取成员转化值类型
/// </summary>
/// <param name="converter"></param>
/// <param name="inner"></param>
public class ConvertValueReader(IEmitMemberReader inner, IEmitConverter converter)
    : IEmitMemberReader
{
    #region 配置
    private readonly IEmitMemberReader _inner = inner;
    /// <summary>
    /// 内部成员
    /// </summary>
    public IEmitMemberReader Inner
        => _inner;
    private readonly IEmitConverter _converter = converter;
    /// <summary>
    /// 转换器
    /// </summary>
    public IEmitConverter Converter
        => _converter;
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
    MemberInfo IEmitMemberReader.Info
        => _inner.Info;
    /// <inheritdoc />
    bool ICompileInfo.Compiled
        => false;
    #endregion
    /// <inheritdoc />
    public Expression Read(Expression instance)
        => _converter.Convert(_inner.Read(instance));
}
