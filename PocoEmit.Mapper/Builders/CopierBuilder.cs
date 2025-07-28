using PocoEmit.Collections;
using PocoEmit.Configuration;
using PocoEmit.Copies;
using PocoEmit.Maping;
using PocoEmit.Members;
using System.Collections.Generic;
using System.Reflection;

namespace PocoEmit.Builders;

/// <summary>
/// 构建复制器
/// </summary>
/// <param name="factory"></param>
public class CopierBuilder(CopierFactory factory)
    : CopierBuilderBase(factory)
{
    /// <inheritdoc />
    protected override void CheckMembers(MapTypeKey key, IEnumerable<MemberInfo> destMembers, ICollection<IMemberConverter> converters)
    {
        TypeMemberCacher memberCacher = _options.MemberCacher;
        var sourceMembers = memberCacher.Get(key.SourceType)?.ReadMembers.Values;
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
    //#region ICopierBuilder
    ///// <inheritdoc />
    //public IEmitCopier Build(IMemberMatch match, Type sourceType, Type destType)
    //    => BuildByCache(_options, match, sourceType, destType);
    ///// <inheritdoc />
    //public IEmitCopier BuildForNullable(IEmitCopier original, Type sourceType, Type destType)
    //    => new CompatibleCopier(original, sourceType, destType);
    ///// <inheritdoc />
    //public IEmitCopier BuildForSelf(Type instanceType)
    //    => BuildForSelfByCache(_options.MemberCacher, instanceType);
    //#endregion
    ///// <summary>
    ///// 为自身类型构建复制器
    ///// </summary>
    ///// <param name="memberCacher"></param>
    ///// <param name="instanceType"></param>
    ///// <returns></returns>
    //public static ComplexTypeCopier BuildForSelfByCache(TypeMemberCacher memberCacher, Type instanceType)
    //{
    //    MemberBundle bundle = memberCacher.Get(instanceType);
    //    if (bundle is null || bundle.WriteMembers.Count == 0)
    //        return null;
    //    var list = new List<IMemberConverter>(bundle.WriteMembers.Count);
    //    var container = MemberContainer.Instance;
    //    foreach (var member in bundle.WriteMembers.Values)
    //    {
    //        var reader = container.MemberReaderCacher.Get(member);
    //        if (reader is null)
    //            continue;
    //        var writer = container.MemberWriterCacher.Get(member);
    //        if (writer is null)
    //            continue;
    //        list.Add(new MemberConverter(reader, writer));
    //    }
    //    if(list.Count == 0)
    //        return null;
    //    ComplexTypeCopier copier = new(list);
    //    return copier;
    //}
    ///// <summary>
    ///// 构建复制器
    ///// </summary>
    ///// <param name="options"></param>
    ///// <param name="match"></param>
    ///// <param name="sourceType"></param>
    ///// <param name="destType"></param>
    ///// <returns></returns>
    //public static ComplexTypeCopier BuildByCache(IMapperOptions options, IMemberMatch match, Type sourceType, Type destType)
    //{
    //    TypeMemberCacher memberCacher = options.MemberCacher;
    //    var sourceMembers = memberCacher.Get(sourceType)?.ReadMembers.Values;
    //    if (sourceMembers is null || sourceMembers.Count == 0)
    //        return null;
    //    var destMembers = memberCacher.Get(destType)?.WriteMembers.Values;
    //    if (destMembers is null || destMembers.Count == 0)
    //        return null;

    //    var list = new List<IMemberConverter>(destMembers.Count);
    //    foreach (var destMember in destMembers)
    //    {
    //        IMemberConverter converter = CheckMember(options, match, sourceMembers, destMember);
    //        if (converter is null)
    //            continue;
    //        list.Add(converter);
    //    }
    //    if (list.Count == 0)
    //        return null;
    //    ComplexTypeCopier copier = new(list);
    //    return copier;
    //}
    /// <summary>
    /// 构造成员转换器
    /// </summary>
    /// <param name="options"></param>
    /// <param name="match"></param>
    /// <param name="sourceMembers"></param>
    /// <param name="destMember"></param>
    /// <returns></returns>
    public static MemberConverter CheckMember(IMapperOptions options, IMemberMatch match, IEnumerable<MemberInfo> sourceMembers, MemberInfo destMember)
    {
        var container = MemberContainer.Instance;
        foreach (var sourceMember in sourceMembers)
        {
            if (match.Match(sourceMember, destMember))
            {
                var reader = container.MemberReaderCacher.Get(sourceMember);
                if (reader is null)
                    continue;
                var writer = container.MemberWriterCacher.Get(destMember);
                if (writer is null)
                    continue;
                var sourceType = reader.ValueType;
                var destType = writer.ValueType;
                if (sourceType == destType || ReflectionHelper.CheckValueType(sourceType, destType))
                    return new MemberConverter(reader, writer);
                var converter = options.ConverterFactory.Get(sourceType, destType);
                if (converter is null)
                    continue;
                return new MemberConverter(reader, new ConvertValueWriter(converter, writer));
            }
        }
        return null;
    }
}
