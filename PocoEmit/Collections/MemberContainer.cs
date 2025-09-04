using PocoEmit.Configuration;
using PocoEmit.Members;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;
using PocoEmit.Enums;

#if NET8_0_OR_GREATER || NETSTANDARD2_0_OR_GREATER
using System.Collections.Frozen;
#endif

namespace PocoEmit.Collections;

/// <summary>
/// 成员容器
/// </summary>
public partial class MemberContainer
    : ICacher<FieldInfo, FieldAccessor>
    , ICacher<PropertyInfo, PropertyAccessor>
    , ICacher<MemberInfo, IEmitMemberReader>
    , ICacher<MemberInfo, IEmitMemberWriter>
    , ICacher<Type, IEnumBundle>
{
    /// <summary>
    /// 成员容器
    /// </summary>
    /// <param name="options">配置</param>
    private MemberContainer(MemberContainerOptions options)
    {
        int concurrencyLevel = options.ConcurrencyLevel;
        int propertyCapacity = options.PropertyCapacity;
        int fieldCapacity = options.FieldCapacity;
        var memberCapacity = propertyCapacity + fieldCapacity;
        _propertyAccessors = new ConcurrentDictionary<PropertyInfo, PropertyAccessor>(concurrencyLevel, propertyCapacity);
        _fieldAccessors = new ConcurrentDictionary<FieldInfo, FieldAccessor>(concurrencyLevel, fieldCapacity);
        _memberReaders = new ConcurrentDictionary<MemberInfo, IEmitMemberReader>(concurrencyLevel, memberCapacity);
        _memberWriters = new ConcurrentDictionary<MemberInfo, IEmitMemberWriter>(concurrencyLevel, memberCapacity);
        _IEnumBundles = new ConcurrentDictionary<Type, IEnumBundle>(concurrencyLevel, options.EnumBundleCapacity);
        _propertyCacher = new PropertyCacher(this);
        _fieldCacher = new FieldCacher(this);
        _memberReaderCacher = new MemberReaderCacher(this);
        _memberWriterCacher = new MemberWriterCacher(this);
        _enumCacher = new EnumCacher(this);
    }
    #region 配置
#if NET8_0_OR_GREATER || NETSTANDARD2_0_OR_GREATER
    private IDictionary<PropertyInfo, PropertyAccessor> _propertyAccessors;
    private IDictionary<FieldInfo, FieldAccessor> _fieldAccessors;
    private IDictionary<MemberInfo, IEmitMemberReader> _memberReaders;
    private IDictionary<MemberInfo, IEmitMemberWriter> _memberWriters;
    private IDictionary<Type, IEnumBundle> _IEnumBundles;
#else
    private readonly ConcurrentDictionary<PropertyInfo, PropertyAccessor> _propertyAccessors;
    private readonly ConcurrentDictionary<FieldInfo, FieldAccessor> _fieldAccessors;    
    private readonly ConcurrentDictionary<MemberInfo, IEmitMemberReader> _memberReaders;
    private readonly ConcurrentDictionary<MemberInfo, IEmitMemberWriter> _memberWriters;
    private readonly ConcurrentDictionary<Type, IEnumBundle> _IEnumBundles;
#endif
    private readonly PropertyCacher _propertyCacher;
    private readonly FieldCacher _fieldCacher;
    private readonly MemberReaderCacher _memberReaderCacher;
    private readonly MemberWriterCacher _memberWriterCacher;
    private readonly EnumCacher _enumCacher;
    /// <summary>
    /// 属性
    /// </summary>
    internal PropertyCacher Propertes
        => _propertyCacher;
    /// <summary>
    /// 字段
    /// </summary>
    internal FieldCacher Fields
        => _fieldCacher;
    /// <summary>
    /// 枚举
    /// </summary>
    public CacheBase<Type, IEnumBundle> Enums
        => _enumCacher;
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
    /// <inheritdoc />
    public MemberReaderCacher MemberReaderCacher
        => _memberReaderCacher;
    /// <inheritdoc />
    public MemberWriterCacher MemberWriterCacher
        => _memberWriterCacher;
    #endregion
    #region ICacher<PropertyInfo, PropertyAccessor>
    /// <inheritdoc />
    bool ICacher<PropertyInfo, PropertyAccessor>.ContainsKey(PropertyInfo key)
        => _propertyAccessors.ContainsKey(key);
    /// <inheritdoc />
    bool ICacher<PropertyInfo, PropertyAccessor>.TryGetValue(PropertyInfo key, out PropertyAccessor value)
        => _propertyAccessors.TryGetValue(key, out value);
    /// <inheritdoc />
    void IStore<PropertyInfo, PropertyAccessor>.Set(PropertyInfo key, PropertyAccessor value)
    => _propertyAccessors[key] = value;
    #endregion
    #region ICacher<FieldInfo, FieldAccessor>
    /// <inheritdoc />
    bool ICacher<FieldInfo, FieldAccessor>.ContainsKey(FieldInfo key)
        => _fieldAccessors.ContainsKey(key);
    /// <inheritdoc />
    bool ICacher<FieldInfo, FieldAccessor>.TryGetValue(FieldInfo key, out FieldAccessor value)
        => _fieldAccessors.TryGetValue(key, out value);
    /// <inheritdoc />
    void IStore<FieldInfo, FieldAccessor>.Set(FieldInfo key, FieldAccessor value)
        => _fieldAccessors[key] = value;
    #endregion
    #region ISettings<MemberInfo, IMemberReader>
    /// <inheritdoc />
    bool ICacher<MemberInfo, IEmitMemberReader>.ContainsKey(MemberInfo key)
        => _memberReaders.ContainsKey(key);
    /// <inheritdoc />
    void IStore<MemberInfo, IEmitMemberReader>.Set(MemberInfo key, IEmitMemberReader value)
        => _memberReaders[key] = value;
    /// <inheritdoc />
    bool ICacher<MemberInfo, IEmitMemberReader>.TryGetValue(MemberInfo key, out IEmitMemberReader value)
        => _memberReaders.TryGetValue(key, out value);
    #endregion
    #region ISettings<MemberInfo, IMemberWriter>
    /// <inheritdoc />
    bool ICacher<MemberInfo, IEmitMemberWriter>.ContainsKey(MemberInfo key)
        => _memberWriters.ContainsKey(key);
    /// <inheritdoc />
    void IStore<MemberInfo, IEmitMemberWriter>.Set(MemberInfo key, IEmitMemberWriter value)
        => _memberWriters[key] = value;
    /// <inheritdoc />
    bool ICacher<MemberInfo, IEmitMemberWriter>.TryGetValue(MemberInfo key, out IEmitMemberWriter value)
        => _memberWriters.TryGetValue(key, out value);
    #endregion
    #region ISettings<Type, IEnumBundle>
    /// <inheritdoc />
    bool ICacher<Type, IEnumBundle>.ContainsKey(Type key)
        => _IEnumBundles.ContainsKey(key);
    /// <inheritdoc />
    void IStore<Type, IEnumBundle>.Set(Type key, IEnumBundle value)
        => _IEnumBundles[key] = value;
    /// <inheritdoc />
    bool ICacher<Type, IEnumBundle>.TryGetValue(Type key, out IEnumBundle value)
        => _IEnumBundles.TryGetValue(key, out value);
    #endregion
#if NET8_0_OR_GREATER || NETSTANDARD2_0_OR_GREATER
    /// <summary>
    /// 设置为不可变
    /// </summary>
    public void ToFrozen()
    {
        _fieldAccessors = _fieldAccessors.ToFrozenDictionary();
        _propertyAccessors = _propertyAccessors.ToFrozenDictionary();
        _memberReaders = _memberReaders.ToFrozenDictionary();
        _memberWriters = _memberWriters.ToFrozenDictionary();
        _IEnumBundles = _IEnumBundles.ToFrozenDictionary();
    }
#endif
    #region 配置
    /// <summary>
    /// 配置
    /// </summary>
    private static Action<MemberContainerOptions> _options = null;
    /// <summary>
    /// 配置(在第一次调用Instance之前配置才能生效)
    /// </summary>
    /// <param name="options"></param>
    public static void Configure(Action<MemberContainerOptions> options)
    {
        if (options is null)
            return;
        if (_options is null)
            _options = options;
        else
            _options += options;
    }
    #endregion
    /// <summary>
    /// 成员容器实例
    /// </summary>
    public static MemberContainer Instance
        => Inner.Instance;
    /// <summary>
    /// 内部延迟初始化(支持先改静态配置,再生成实例)
    /// </summary>
    class Inner
    {
        /// <summary>
        /// 实例
        /// </summary>
        internal static readonly MemberContainer Instance = Create();
        /// <summary>
        /// 构造实例
        /// </summary>
        /// <returns></returns>
        static MemberContainer Create()
        {
            var options = new MemberContainerOptions();
            _options?.Invoke(options);
            return new(options);
        }
    }
}
