using PocoEmit.Members;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;
#if NET7_0_OR_GREATER || NETSTANDARD2_0_OR_GREATER
using System.Collections.Frozen;
#endif

namespace PocoEmit.Collections;

/// <summary>
/// 成员容器
/// </summary>
public class MemberContainer
    : ICacher<MemberInfo, IEmitMemberReader>
    , ICacher<MemberInfo, IEmitMemberWriter>
{
    /// <summary>
    /// 成员容器
    /// </summary>
    /// <param name="concurrencyLevel">并发</param>
    /// <param name="fieldCapacity">字段数量</param>
    /// <param name="propertyCapacity">属性数量</param>
    private MemberContainer(int concurrencyLevel = 4, int fieldCapacity = 1024, int propertyCapacity = 1024)
    {
        _memberReaders = new ConcurrentDictionary<MemberInfo, IEmitMemberReader>(concurrencyLevel, fieldCapacity + propertyCapacity);
        _memberWriters = new ConcurrentDictionary<MemberInfo, IEmitMemberWriter>(concurrencyLevel, fieldCapacity + propertyCapacity);
        _memberReaderCacher = new MemberReaderCacher(this);
        _memberWriterCacher = new MemberWriterCacher(this);
    }
    #region 配置
#if NET7_0_OR_GREATER || NETSTANDARD2_0_OR_GREATER
    private IDictionary<MemberInfo, IEmitMemberReader> _memberReaders;
    private IDictionary<MemberInfo, IEmitMemberWriter> _memberWriters;
#else
    private readonly ConcurrentDictionary<MemberInfo, IEmitMemberReader> _memberReaders;
    private readonly ConcurrentDictionary<MemberInfo, IEmitMemberWriter> _memberWriters;
#endif
    private readonly MemberReaderCacher _memberReaderCacher;
    private readonly MemberWriterCacher _memberWriterCacher;
    /// <summary>
    /// 读取器
    /// </summary>
    public IEnumerable<IEmitMemberReader> Readers
        => _memberReaders.Values;
    /// <summary>
    /// 写入器
    /// </summary>
    public IEnumerable<IEmitMemberWriter> Writers
        => _memberWriters.Values;
    /// <summary>
    /// 成员读取器缓存
    /// </summary>
    public MemberReaderCacher MemberReaderCacher
        => _memberReaderCacher;
    /// <summary>
    /// 成员写入器缓存
    /// </summary>
    public MemberWriterCacher MemberWriterCacher
        => _memberWriterCacher;
    #endregion    
    #region ISettings<MemberInfo, IMemberReader>
    bool ICacher<MemberInfo, IEmitMemberReader>.ContainsKey(MemberInfo key)
        => _memberReaders.ContainsKey(key);
    void IStore<MemberInfo, IEmitMemberReader>.Set(MemberInfo key, IEmitMemberReader value)
        => _memberReaders[key] = value;
    bool ICacher<MemberInfo, IEmitMemberReader>.TryGetValue(MemberInfo key, out IEmitMemberReader value)
        => _memberReaders.TryGetValue(key, out value);
    #endregion
    #region ISettings<MemberInfo, IMemberWriter>
    bool ICacher<MemberInfo, IEmitMemberWriter>.ContainsKey(MemberInfo key)
        => _memberWriters.ContainsKey(key);
    void IStore<MemberInfo, IEmitMemberWriter>.Set(MemberInfo key, IEmitMemberWriter value)
        => _memberWriters[key] = value;
    bool ICacher<MemberInfo, IEmitMemberWriter>.TryGetValue(MemberInfo key, out IEmitMemberWriter value)
        => _memberWriters.TryGetValue(key, out value);
    #endregion
#if NET7_0_OR_GREATER || NETSTANDARD2_0_OR_GREATER
    /// <summary>
    /// 设置为不可变
    /// </summary>
    public void ToFrozen()
    {
        _memberReaders = _memberReaders.ToFrozenDictionary();
        _memberWriters = _memberWriters.ToFrozenDictionary();
    }
#endif
    /// <summary>
    /// 并发
    /// </summary>
    public static int ConcurrencyLevel = Environment.ProcessorCount;
    /// <summary>
    /// 字段数量
    /// </summary>
    public static int FieldCapacity = 1024;
    /// <summary>
    /// 属性数量
    /// </summary>
    public static int PropertyCapacity = 1024;
    /// <summary>
    /// 成员容器实例
    /// </summary>
    public static MemberContainer Instance
        => Inner.Instance;
    /// <summary>
    /// 内部延迟初始化
    /// </summary>
    class Inner
    {
        public static readonly MemberContainer Instance = new(ConcurrencyLevel, FieldCapacity, PropertyCapacity);
    }
}
