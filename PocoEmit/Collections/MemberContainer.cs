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
    //: ISettings<FieldInfo, FieldReader>
    //, ISettings<FieldInfo, FieldWriter>
    //, ISettings<PropertyInfo, PropertyReader>
    //, ISettings<PropertyInfo, PropertyWriter>
    : ISettings<MemberInfo, IEmitMemberReader>
    , ISettings<MemberInfo, IEmitMemberWriter>
{
    /// <summary>
    /// 成员容器
    /// </summary>
    /// <param name="concurrencyLevel">并发</param>
    /// <param name="fieldCapacity">字段数量</param>
    /// <param name="propertyCapacity">属性数量</param>
    private MemberContainer(int concurrencyLevel = 4, int fieldCapacity = 1024, int propertyCapacity = 1024)
    {
        //_fieldReaders = new ConcurrentDictionary<FieldInfo, FieldReader>(concurrencyLevel, fieldCapacity);
        //_fieldWriters = new ConcurrentDictionary<FieldInfo, FieldWriter>(concurrencyLevel, fieldCapacity);
        //_propertyReaders = new ConcurrentDictionary<PropertyInfo, PropertyReader>(concurrencyLevel, propertyCapacity);
        //_propertyWriters = new ConcurrentDictionary<PropertyInfo, PropertyWriter>(concurrencyLevel, propertyCapacity);
        _memberReaders = new ConcurrentDictionary<MemberInfo, IEmitMemberReader>(concurrencyLevel, fieldCapacity + propertyCapacity);
        _memberWriters = new ConcurrentDictionary<MemberInfo, IEmitMemberWriter>(concurrencyLevel, fieldCapacity + propertyCapacity);
        //_fieldReaderCacher = new FieldReaderCacher(this);
        //_fieldWriterCacher = new FieldWriterCacher(this);
        //_propertyReaderCacher = new PropertyReaderCacher(this);
        //_propertyWriterCacher = new PropertyWriterCacher(this);
        _memberReaderCacher = new MemberReaderCacher(this);
        _memberWriterCacher = new MemberWriterCacher(this);
    }
    #region 配置
#if NET7_0_OR_GREATER || NETSTANDARD2_0_OR_GREATER
    //private IDictionary<FieldInfo, FieldReader> _fieldReaders;
    //private IDictionary<FieldInfo, FieldWriter> _fieldWriters;
    //private IDictionary<PropertyInfo, PropertyReader> _propertyReaders;
    //private IDictionary<PropertyInfo, PropertyWriter> _propertyWriters;
    private IDictionary<MemberInfo, IEmitMemberReader> _memberReaders;
    private IDictionary<MemberInfo, IEmitMemberWriter> _memberWriters;
#else
    //private readonly ConcurrentDictionary<FieldInfo, FieldReader> _fieldReaders;
    //private readonly ConcurrentDictionary<FieldInfo, FieldWriter> _fieldWriters;
    //private readonly ConcurrentDictionary<PropertyInfo, PropertyReader> _propertyReaders;
    //private readonly ConcurrentDictionary<PropertyInfo, PropertyWriter> _propertyWriters;
    private readonly ConcurrentDictionary<MemberInfo, IEmitMemberReader> _memberReaders;
    private readonly ConcurrentDictionary<MemberInfo, IEmitMemberWriter> _memberWriters;
#endif
    //private readonly FieldReaderCacher _fieldReaderCacher;
    //private readonly FieldWriterCacher _fieldWriterCacher;
    //private readonly PropertyReaderCacher _propertyReaderCacher;
    //private readonly PropertyWriterCacher _propertyWriterCacher;
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
    ///// <summary>
    ///// 字段读取器缓存
    ///// </summary>
    //public FieldReaderCacher FieldReaderCacher
    //    => _fieldReaderCacher;
    ///// <summary>
    ///// 字段写入器缓存
    ///// </summary>
    //public FieldWriterCacher FieldWriterCacher
    //    => _fieldWriterCacher;
    ///// <summary>
    ///// 属性读取器缓存
    ///// </summary>
    //public PropertyReaderCacher PropertyReaderCacher
    //    => _propertyReaderCacher;
    ///// <summary>
    ///// 属性写入器缓存
    ///// </summary>
    //public PropertyWriterCacher PropertyWriterCacher
    //    => _propertyWriterCacher;
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
    //#region ISettings<FieldInfo, FieldReader>
    //bool ISettings<FieldInfo, FieldReader>.ContainsKey(FieldInfo key)
    //    => _fieldReaders.ContainsKey(key);
    //void ISettings<FieldInfo, FieldReader>.Set(FieldInfo key, FieldReader value)
    //{
    //    _fieldReaders[key] = value;
    //    _memberReaders[key] = value;
    //}
    //bool ISettings<FieldInfo, FieldReader>.TryGetValue(FieldInfo key, out FieldReader value)
    //    => _fieldReaders.TryGetValue(key, out value);
    //#endregion
    //#region ISettings<FieldInfo, FieldWriter>
    //bool ISettings<FieldInfo, FieldWriter>.ContainsKey(FieldInfo key)
    //    => _fieldWriters.ContainsKey(key);
    //void ISettings<FieldInfo, FieldWriter>.Set(FieldInfo key, FieldWriter value)
    //{
    //    _fieldWriters[key] = value;
    //    _memberWriters[key] = value;
    //}
    //bool ISettings<FieldInfo, FieldWriter>.TryGetValue(FieldInfo key, out FieldWriter value)
    //    => _fieldWriters.TryGetValue(key, out value);
    //#endregion
    //#region ISettings<PropertyInfo, PropertyReader>
    //bool ISettings<PropertyInfo, PropertyReader>.ContainsKey(PropertyInfo key)
    //    => _propertyReaders.ContainsKey(key);
    //void ISettings<PropertyInfo, PropertyReader>.Set(PropertyInfo key, PropertyReader value)
    //{
    //    _propertyReaders[key] = value;
    //    _memberReaders[key] = value;
    //}
    //bool ISettings<PropertyInfo, PropertyReader>.TryGetValue(PropertyInfo key, out PropertyReader value)
    //    => _propertyReaders.TryGetValue(key, out value);
    //#endregion
    //#region ISettings<PropertyInfo, PropertyWriter>
    //bool ISettings<PropertyInfo, PropertyWriter>.ContainsKey(PropertyInfo key)
    //    => _propertyWriters.ContainsKey(key);
    //void ISettings<PropertyInfo, PropertyWriter>.Set(PropertyInfo key, PropertyWriter value)
    //{
    //    _propertyWriters[key] = value;
    //    _memberWriters[key] = value;
    //}
    //bool ISettings<PropertyInfo, PropertyWriter>.TryGetValue(PropertyInfo key, out PropertyWriter value)
    //    => _propertyWriters.TryGetValue(key, out value);
    //#endregion
    #region ISettings<MemberInfo, IMemberReader>
    bool ISettings<MemberInfo, IEmitMemberReader>.ContainsKey(MemberInfo key)
        => _memberReaders.ContainsKey(key);
    void ISettings<MemberInfo, IEmitMemberReader>.Set(MemberInfo key, IEmitMemberReader value)
    {
        _memberReaders[key] = value;
        //if (key is FieldInfo field)
        //{
        //    if (value is FieldReader fieldWriter)
        //        _fieldReaders[field] = fieldWriter;
        //}
        //else if (key is PropertyInfo property)
        //{
        //    if (value is PropertyReader propertyWriter)
        //        _propertyReaders[property] = propertyWriter;
        //}
    }
    bool ISettings<MemberInfo, IEmitMemberReader>.TryGetValue(MemberInfo key, out IEmitMemberReader value)
        => _memberReaders.TryGetValue(key, out value);
    #endregion
    #region ISettings<MemberInfo, IMemberWriter>
    bool ISettings<MemberInfo, IEmitMemberWriter>.ContainsKey(MemberInfo key)
        => _memberWriters.ContainsKey(key);
    void ISettings<MemberInfo, IEmitMemberWriter>.Set(MemberInfo key, IEmitMemberWriter value)
    {
        _memberWriters[key] = value;
        //if (key is FieldInfo field)
        //{
        //    if (value is FieldWriter fieldWriter)
        //        _fieldWriters[field] = fieldWriter;
        //}
        //else if (key is PropertyInfo property)
        //{
        //    if (value is PropertyWriter propertyWriter)
        //        _propertyWriters[property] = propertyWriter;
        //}
    }
    bool ISettings<MemberInfo, IEmitMemberWriter>.TryGetValue(MemberInfo key, out IEmitMemberWriter value)
        => _memberWriters.TryGetValue(key, out value);
    #endregion
#if NET7_0_OR_GREATER || NETSTANDARD2_0_OR_GREATER
    /// <summary>
    /// 设置为不可变
    /// </summary>
    public void ToFrozen()
    {
        //_fieldReaders = _fieldReaders.ToFrozenDictionary();
        //_fieldWriters = _fieldWriters.ToFrozenDictionary();
        //_propertyReaders = _propertyReaders.ToFrozenDictionary();
        //_propertyWriters = _propertyWriters.ToFrozenDictionary();
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
