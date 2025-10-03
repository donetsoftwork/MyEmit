using PocoEmit.Collections;
using PocoEmit.Configuration;
using PocoEmit.Copies;
using PocoEmit.Maping;
using PocoEmit.Members;
using System;
using System.Collections.Generic;

namespace PocoEmit.Builders;

/// <summary>
/// 构建复制器
/// </summary>
/// <param name="options"></param>
public class CopierBuilder(IMapperOptions options)
    : CopierBuilderBase(options)
{
    #region 配置
    /// <summary>
    /// 同类型复制
    /// </summary>
    private readonly CopyToSelf _forSelf = new(options);
    /// <summary>
    /// 同类型复制
    /// </summary>
    public CopyToSelf ForSelf 
        => _forSelf;
    #endregion
    #region 功能
    /// <summary>
    /// 同类型复制
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public virtual IEmitCopier ToSelf(in PairTypeKey key)
        => _forSelf.Build(key);
    /// <summary>
    /// 为可空类型构建复制器
    /// </summary>
    /// <param name="original"></param>
    /// <param name="sourceType"></param>
    /// <param name="destType"></param>
    /// <returns></returns>
    public virtual IEmitCopier ForNullable(IEmitCopier original, Type sourceType, Type destType)
        => new CompatibleCopier(original, sourceType, destType);
    /// <summary>
    /// 不支持数组
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public virtual IEmitCopier ToArray(in PairTypeKey key)
        => null;
    /// <summary>
    /// 不支持字典(预留扩展)
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public virtual IEmitCopier ToDictionary(in PairTypeKey key)
        => null;
    /// <summary>
    /// 不支持集合(预留扩展)
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public virtual IEmitCopier ToCollection(in PairTypeKey key)
       => null;
    #endregion
    /// <inheritdoc />
    public override void CheckMembers(in PairTypeKey key, IEnumerable<IEmitMemberWriter> destMembers, ICollection<IMemberConverter> converters)
    {
        TypeMemberCacher memberCacher = _options.MemberCacher;
        var sourceMembers = memberCacher.Get(key.LeftType)?.EmitReaders.Values;
        if (sourceMembers is null || sourceMembers.Count == 0)
            return;
        IMemberMatch match = _options.GetMemberMatch(key);
        foreach (var destMember in destMembers)
        {
            IMemberConverter converter = CheckMember(_options, match, sourceMembers, destMember);
            if (converter is null)
                continue;
            converters.Add(converter);
        }
    }
    /// <summary>
    /// 构造成员转换器
    /// </summary>
    /// <param name="options"></param>
    /// <param name="match"></param>
    /// <param name="sourceMembers"></param>
    /// <param name="dest"></param>
    /// <returns></returns>
    public static MemberConverter CheckMember(IMapperOptions options, IMemberMatch match, IEnumerable<IEmitMemberReader> sourceMembers, IEmitMemberWriter dest)
    {
        foreach (var reader in match.Select(options.Recognizer, sourceMembers, dest))
        {
            var converter = options.GetEmitConverter(reader.ValueType, dest.ValueType);
            if (converter is null)
                continue;
            return new MemberConverter(options, reader, dest, converter);
        }
        var defaultValue = options.DefaultValueBuilder.Build(dest);
        if (defaultValue is null)
            return null;
        var adapter = new BuilderReaderAdapter(defaultValue);
        var destType = dest.ValueType;
        return new MemberConverter(options, adapter, dest, options.GetEmitConverter(destType, destType));
    }
}
