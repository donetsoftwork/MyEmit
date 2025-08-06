using PocoEmit.Collections;
using PocoEmit.Configuration;
using PocoEmit.Copies;
using PocoEmit.Maping;
using PocoEmit.Members;
using System.Collections.Generic;

namespace PocoEmit.Builders;

/// <summary>
/// 构建复制器
/// </summary>
/// <param name="factory"></param>
public class CopierBuilder(CopierFactory factory)
    : CopierBuilderBase(factory)
{
    /// <inheritdoc />
    public override void CheckMembers(MapTypeKey key, IEnumerable<IEmitMemberWriter> destMembers, ICollection<IMemberConverter> converters)
    {
        TypeMemberCacher memberCacher = _options.MemberCacher;
        var sourceMembers = memberCacher.Get(key.SourceType)?.EmitReaders.Values;
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
    /// <param name="writer"></param>
    /// <returns></returns>
    public static MemberConverter CheckMember(IMapperOptions options, IMemberMatch match, IEnumerable<IEmitMemberReader> sourceMembers, IEmitMemberWriter writer)
    {
        foreach (var reader in match.Select(options.Recognizer, sourceMembers, writer))
        {
            var sourceType = reader.ValueType;
            var destType = writer.ValueType;
            if (sourceType == destType || ReflectionHelper.CheckValueType(sourceType, destType))
                return new MemberConverter(options, reader, writer);
            var converter = options.GetEmitConverter(sourceType, destType);
            if (converter is null)
                continue;
            return new MemberConverter(options, reader, new ConvertValueWriter(converter, writer));
        }
        return null;
    }
}
