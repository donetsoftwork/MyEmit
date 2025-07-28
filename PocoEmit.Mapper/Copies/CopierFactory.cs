using PocoEmit.Builders;
using PocoEmit.Collections;
using PocoEmit.Configuration;
using PocoEmit.Copies;
using System;

namespace PocoEmit.Copies;

/// <summary>
/// 复制器工厂
/// </summary>
public class CopierFactory
    : CacheBase<MapTypeKey, IEmitCopier>
{
    /// <summary>
    /// 复制器工厂
    /// </summary>
    /// <param name="options"></param>
    public CopierFactory(IMapperOptions options)
        : base(options)
    {
        _options = options;
        _builderForSelf = new CopierBuilderForSelf(this);
        _builder = new CopierBuilder(this);
    }
    #region 配置
    private readonly IMapperOptions _options;
    /// <summary>
    /// Emit配置
    /// </summary>
    public IMapperOptions Options
        => _options;
    private CopierBuilderForSelf _builderForSelf;
    private readonly CopierBuilder _builder;
    #endregion
    #region CacheBase
    /// <inheritdoc />
    protected override IEmitCopier CreateNew(MapTypeKey key)
    {
        var destType = key.DestType;
        if(CheckPrimitive(destType))
            return null;
        var sourceType = key.SourceType;
        // 同类型
        if (sourceType == destType)
            return _builderForSelf.Build(key);
        // 兼容类型
        if (ReflectionHelper.CheckValueType(sourceType, destType))
            return _builderForSelf.Build(key);
        bool isNullable = false;
        if (ReflectionHelper.IsNullable(sourceType))
        {
            isNullable = true;
            sourceType = sourceType.GenericTypeArguments[0];
        }
        if (ReflectionHelper.IsNullable(destType))
        {
            isNullable = true;
            destType = destType.GenericTypeArguments[0];
        }
        if (isNullable)
        {
            var originalKey = new MapTypeKey(sourceType, destType);
            IEmitCopier original = Get(originalKey);
            if (original is null)
                return null;
            // 可空类型
            return BuildForNullable(original, sourceType, key.DestType);
        }
        // 普通类型
        return _builder.Build(key);
    }
    #endregion
    /// <summary>
    /// 为可空类型构建复制器
    /// </summary>
    /// <param name="original"></param>
    /// <param name="sourceType"></param>
    /// <param name="destType"></param>
    /// <returns></returns>
    public static IEmitCopier BuildForNullable(IEmitCopier original, Type sourceType, Type destType)
    => new CompatibleCopier(original, sourceType, destType);
    private bool CheckPrimitive(Type destType)
        => _options.Primitives.Get(destType) || destType == typeof(object);
    ///// <summary>
    ///// 构造Emit类型转化原始方法
    ///// </summary>
    ///// <param name="key"></param>
    ///// <returns></returns>
    //private IEmitCopier CreateCore(MapTypeKey key)
    //{
    //    var destType = key.DestType;
    //    var sourceType = key.SourceType;
    //    var dests = _options.Dest.Get(destType);
    //    if (dests is null || dests.Length == 0)
    //        return null;
    //    var sources = _options.Source.Get(sourceType);
    //    if (sources is null || sources.Length == 0)
    //        return null;
    //    List<IMemberConverter> members = new();
    //    // 阻塞至成员完全初始化完成
    //    EventWaitHandle block = new(false, EventResetMode.ManualReset);
    //    ComplexTypeCopier copier = new(Wait(block, members));
    //    this.TrySet(key, copier);
    //    var match = _options.GetMemberMatch(key);
    //    foreach (var dest in dests)
    //    {
    //        foreach (var source in sources)
    //        {
    //            var converter = Mapping(match, source, dest);
    //            if(converter is null)
    //                continue;
    //            members.Add(converter);
    //        }
    //    }
    //    // 完全初始化完成,允许读
    //    block.Set();
    //    copier.MembersToArray();
    //    return copier;
    //}
    ///// <summary>
    ///// 为自身类型构建复制器
    ///// </summary>
    ///// <param name="key"></param>
    ///// <param name="instanceType"></param>
    ///// <returns></returns>
    //public ComplexTypeCopier BuildForSelfByCache(MapTypeKey key, Type instanceType)
    //{
    //    TypeMemberCacher memberCacher = _options.MemberCacher;
    //    MemberBundle bundle = memberCacher.Get(instanceType);
    //    if (bundle is null || bundle.WriteMembers.Count == 0)
    //        return null;
    //    var list = new List<IMemberConverter>(bundle.WriteMembers.Count);
    //    // 阻塞至成员完全初始化完成
    //    EventWaitHandle block = new(false, EventResetMode.ManualReset);
    //    ComplexTypeCopier copier = new(Wait(block, list));
    //    this.TrySet(key, copier);
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
    //    // 完全初始化完成,允许读
    //    block.Set();
    //    copier.MembersToArray();
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
    //public ComplexTypeCopier BuildByCache(IMapperOptions options, IMemberMatch match, MapTypeKey key, Type sourceType, Type destType)
    //{
    //    TypeMemberCacher memberCacher = options.MemberCacher;
    //    var sourceMembers = memberCacher.Get(sourceType)?.ReadMembers.Values;
    //    if (sourceMembers is null || sourceMembers.Count == 0)
    //        return null;
    //    var destMembers = memberCacher.Get(destType)?.WriteMembers.Values;
    //    if (destMembers is null || destMembers.Count == 0)
    //        return null;

    //    var list = new List<IMemberConverter>(destMembers.Count);
    //    // 阻塞至成员完全初始化完成
    //    EventWaitHandle block = new(false, EventResetMode.ManualReset);
    //    ComplexTypeCopier copier = new(Wait(block, list));
    //    this.TrySet(key, copier);
    //    foreach (var destMember in destMembers)
    //    {
    //        IMemberConverter converter = CheckMember(options, match, sourceMembers, destMember);
    //        if (converter is null)
    //            continue;
    //        list.Add(converter);
    //    }
    //    // 完全初始化完成,允许读
    //    block.Set();
    //    copier.MembersToArray();
    //    return copier;
    //}
    ///// <summary>
    ///// 构造成员转换器
    ///// </summary>
    ///// <param name="options"></param>
    ///// <param name="match"></param>
    ///// <param name="sourceMembers"></param>
    ///// <param name="destMember"></param>
    ///// <returns></returns>
    //public MemberConverter CheckMember(IMapperOptions options, IMemberMatch match, IEnumerable<MemberInfo> sourceMembers, MemberInfo destMember)
    //{
    //    var container = MemberContainer.Instance;
    //    foreach (var sourceMember in sourceMembers)
    //    {
    //        if (match.Match(sourceMember, destMember))
    //        {
    //            var reader = container.MemberReaderCacher.Get(sourceMember);
    //            if (reader is null)
    //                continue;
    //            var writer = container.MemberWriterCacher.Get(destMember);
    //            if (writer is null)
    //                continue;
    //            var sourceType = reader.ValueType;
    //            var destType = writer.ValueType;
    //            if (sourceType == destType || ReflectionHelper.CheckValueType(sourceType, destType))
    //                return new MemberConverter(reader, writer);
    //            if (CheckPrimitive(destType))
    //            {
    //                var converter = options.ConverterFactory.Get(sourceType, destType);
    //                if (converter is null)
    //                    continue;
    //                return new MemberConverter(reader, new ConvertValueWriter(converter, writer));
    //            }
    //            var memberType = new MapTypeKey(sourceType, destType);
    //            var memberCopier = Get(memberType);
    //            if (memberCopier is null)
    //                continue;


    //        }
    //    }
    //    return null;
    //}
    ///// <summary>
    ///// 映射
    ///// </summary>
    ///// <param name="match"></param>
    ///// <param name="source"></param>
    ///// <param name="dest"></param>
    ///// <returns></returns>
    //public IMemberConverter Mapping(IMemberMatch match, IEmitMemberReader source, IEmitMemberWriter dest)
    //{
    //    if(!match.Match(source, dest))
    //        return null;
    //    var sourceType = source.ValueType;
    //    var destType = dest.ValueType;


    //    return null;
    //}
    ///// <summary>
    ///// 延迟列表
    ///// </summary>
    ///// <param name="handle"></param>
    ///// <param name="list"></param>
    ///// <returns></returns>
    //static IEnumerable<IMemberConverter> Wait(EventWaitHandle handle, IEnumerable<IMemberConverter> list)
    //{
    //    handle.WaitOne();
    //    foreach (var converter in list)
    //        yield return converter;
    //}
}
