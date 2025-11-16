using Hand.Structural;
using PocoEmit.Builders;
using PocoEmit.Configuration;
using PocoEmit.Converters;
using System;
using System.Linq.Expressions;
using System.Reflection;

namespace PocoEmit.Members;

/// <summary>
/// 转化实体类型读成员
/// </summary>
/// <param name="converter"></param>
/// <param name="original"></param>
public class ConvertInstanceReader(IEmitConverter converter, IEmitMemberReader original)
    : IEmitMemberReader
    , IWrapper<IEmitMemberReader>
{
    #region 配置
    private readonly IEmitConverter _converter = converter;
    /// <summary>
    /// 转换器
    /// </summary>
    public IEmitConverter Converter
        => _converter;
    private readonly IEmitMemberReader _original = original;
    /// <inheritdoc />
    public IEmitMemberReader Original
        => _original;
    /// <inheritdoc />
    public Type InstanceType
        => _original.InstanceType;
    /// <inheritdoc />
    public string Name 
        => _original.Name;
    /// <inheritdoc />
    public Type ValueType 
        => _original.ValueType;
    /// <inheritdoc />
    MemberInfo IEmitMemberReader.Info
        => _original.Info;
    /// <inheritdoc />
    bool ICompileInfo.Compiled
        => false;
    #endregion
    /// <inheritdoc />
    public Expression Read(Expression instance)
        => _original.Read(_converter.Convert(instance));
}
