using Hand.Structural;
using PocoEmit.Builders;
using PocoEmit.Configuration;
using PocoEmit.Converters;
using System;
using System.Linq.Expressions;
using System.Reflection;

namespace PocoEmit.Members;

/// <summary>
/// 读取成员转化值类型
/// </summary>
/// <param name="original"></param>
/// <param name="converter"></param>
/// <param name="valueType"></param>
public class ConvertValueReader(IEmitMemberReader original, IEmitConverter converter, Type valueType)
    : IEmitMemberReader
    , IWrapper<IEmitMemberReader>
{
    #region 配置
    private readonly IEmitMemberReader _original = original;
    /// <inheritdoc />
    public IEmitMemberReader Original
        => _original;
    private readonly IEmitConverter _converter = converter;
    private readonly Type _valueType = valueType;
    /// <summary>
    /// 转换器
    /// </summary>
    public IEmitConverter Converter
        => _converter;
    /// <inheritdoc />
    public Type InstanceType
        => _original.InstanceType;
    /// <inheritdoc />
    public string Name
        => _original.Name;
    /// <inheritdoc />
    public Type ValueType
        => _valueType;
    /// <inheritdoc />
    MemberInfo IEmitMemberReader.Info
        => _original.Info;
    /// <inheritdoc />
    bool ICompileInfo.Compiled
        => false;
    #endregion
    /// <inheritdoc />
    public Expression Read(Expression instance)
        => _converter.Convert(_original.Read(instance));
}
