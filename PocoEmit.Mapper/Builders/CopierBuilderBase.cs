using PocoEmit.Collections;
using PocoEmit.Configuration;
using PocoEmit.Copies;
using PocoEmit.Members;
using System.Collections.Generic;
using System.Threading;

namespace PocoEmit.Builders;

/// <summary>
/// 构建复制器基类
/// </summary>
/// <param name="factory"></param>
public abstract class CopierBuilderBase(CopierFactory factory)
{
    #region 配置
    /// <summary>
    /// Emit配置
    /// </summary>
    protected readonly IMapperOptions _options = factory.Options;
    private readonly CopierFactory _factory = factory;

    /// <summary>
    /// Emit配置
    /// </summary>
    public IMapperOptions Options
        => _options;
    /// <summary>
    /// 复制器工厂
    /// </summary>
    public CopierFactory Factory
        => _factory;
    #endregion
    /// <summary>
    /// 构建复制器
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public IEmitCopier Build(MapTypeKey key)
    {
        var destType = key.DestType;
        TypeMemberCacher memberCacher = _options.MemberCacher;
        var destMembers = memberCacher.Get(destType)?.EmitWriters.Values;
        if (destMembers is null || destMembers.Count == 0)
            return null;
        var list = new List<IMemberConverter>(destMembers.Count);
        // 阻塞至完成成员初始化
        EventWaitHandle block = new(false, EventResetMode.ManualReset);
        var copier = New(key, block, list);
        CheckMembers(key, destMembers, list);
        // 初始化完成,允许读
        block.Set();
        copier.MembersToArray();
        if (list.Count == 0)
            return null;
        return copier;
    }
    /// <summary>
    /// 处理成员
    /// </summary>
    /// <param name="key"></param>
    /// <param name="destMembers"></param>
    /// <param name="converters"></param>
    public abstract void CheckMembers(MapTypeKey key, IEnumerable<IEmitMemberWriter> destMembers, ICollection<IMemberConverter> converters);
    /// <summary>
    /// 构建复制器
    /// </summary>
    /// <param name="key"></param>
    /// <param name="block"></param>
    /// <param name="members"></param>
    /// <returns></returns>
    private ComplexTypeCopier New(MapTypeKey key, EventWaitHandle block, IEnumerable<IMemberConverter> members)
    {
        ComplexTypeCopier copier = new(Wait(block, members));
        // 提前设置复制器,避免重复构建
        _factory.TryCache(key, copier);
        return copier;
    }
    /// <summary>
    /// 延迟列表
    /// </summary>
    /// <param name="handle"></param>
    /// <param name="list"></param>
    /// <returns></returns>
    private static IEnumerable<IMemberConverter> Wait(EventWaitHandle handle, IEnumerable<IMemberConverter> list)
    {
        handle.WaitOne();
        foreach (var converter in list)
            yield return converter;
    }
}
