using PocoEmit.Collections;
using System;
using System.Collections.Generic;
using System.Reflection;
using PocoEmit.Members;

#if NET7_0_OR_GREATER || NETSTANDARD2_0_OR_GREATER
using System.Collections.Frozen;
#else
using System.Linq;
#endif

namespace PocoEmit.Reflection;

/// <summary>
/// 默认反射成员
/// </summary>
public class DefaultReflectionMember(StringComparer comparer, bool includeField = true)
    : IReflectionMember
{
    /// <summary>
    /// 默认反射成员
    /// </summary>
    public DefaultReflectionMember()
        : this(StringComparer.OrdinalIgnoreCase, true)
    {
    }
    #region 配置
    private readonly StringComparer _comparer = comparer;
    private readonly bool _includeField = includeField;
    /// <summary>
    /// 成员名比较器
    /// </summary>
    public StringComparer Comparer
        => _comparer;
    /// <summary>
    /// 是否包含字段
    /// </summary>
    public bool IncludeField
        => _includeField;
    #endregion
    /// <inheritdoc />
    public MemberBundle GetMembers(Type instanceType)
    {
        Dictionary<string, MemberInfo> readMembers = new(_comparer);
        Dictionary<string, MemberInfo> writeMembers = new(_comparer);
        foreach (var property in GetProperties(instanceType))
        {
            if(property.GetIndexParameters().Length > 0)
                continue;
            var name = property.Name;
            if (property.CanRead && !readMembers.ContainsKey(name))
                readMembers.Add(name, property);
            if (property.CanWrite && !writeMembers.ContainsKey(name))
                writeMembers.Add(name, property);
        }
        if(_includeField)
        {
            foreach (var field in GetFields(instanceType))
            {
                var name = field.Name;
                if (!readMembers.ContainsKey(name))
                    readMembers.Add(name, field);
                if (!writeMembers.ContainsKey(name))
                    writeMembers.Add(name, field);
            }
        }
        List<IEmitMemberReader> readers = [];
        CheckReader(readers, readMembers.Values);
        List<IEmitMemberWriter> writers = [];
        CheckWriter(writers, writeMembers.Values);
        return new MemberBundle(CheckMembers(readMembers), CheckMembers(readers), CheckMembers(writeMembers), CheckMembers(writers));
    }
    /// <summary>
    /// 处理读取器
    /// </summary>
    /// <param name="readers"></param>
    /// <param name="members"></param>
    private static void CheckReader(ICollection<IEmitMemberReader> readers, IEnumerable<MemberInfo> members)
    {
        var readerCacher = MemberContainer.Instance.MemberReaderCacher;
        foreach (var member in members)
        {
            var reader = readerCacher.Get(member);
            if (reader is null)
                continue;
            readers.Add(reader);
        }
    }
    /// <summary>
    /// 处理写入器
    /// </summary>
    /// <param name="writers"></param>
    /// <param name="members"></param>
    private static void CheckWriter(ICollection<IEmitMemberWriter> writers, IEnumerable<MemberInfo> members)
    {
        var writerCacher = MemberContainer.Instance.MemberWriterCacher;
        foreach (var member in members)
        {
            var writer = writerCacher.Get(member);
            if (writer is null)
                continue;
            writers.Add(writer);
        }
    }
    /// <summary>
    /// 检查成员
    /// </summary>
    /// <param name="dic"></param>
    /// <returns></returns>
    protected virtual IDictionary<string, MemberInfo> CheckMembers(Dictionary<string, MemberInfo> dic)
    {
#if NET7_0_OR_GREATER || NETSTANDARD2_0_OR_GREATER
        return dic.ToFrozenDictionary(_comparer);
#else
        return dic;
#endif
    }
    /// <summary>
    /// 检查成员
    /// </summary>
    /// <param name="list"></param>
    /// <returns></returns>
    protected virtual IDictionary<string, IEmitMemberReader> CheckMembers(IEnumerable<IEmitMemberReader> list)
    {
#if NET7_0_OR_GREATER || NETSTANDARD2_0_OR_GREATER
        return list.ToFrozenDictionary(m => m.Name, m => m, _comparer);
#else
        return list.ToDictionary(m => m.Name, m => m, _comparer);
#endif
    }
    /// <summary>
    /// 检查成员
    /// </summary>
    /// <param name="list"></param>
    /// <returns></returns>
    protected virtual IDictionary<string, IEmitMemberWriter> CheckMembers(IEnumerable<IEmitMemberWriter> list)
    {
#if NET7_0_OR_GREATER || NETSTANDARD2_0_OR_GREATER
        return list.ToFrozenDictionary(m => m.Name, m => m, _comparer);
#else
        return list.ToDictionary(m => m.Name, m => m, _comparer);
#endif
    }
    /// <summary>
    /// 获取所有属性
    /// </summary>
    /// <param name="instanceType"></param>
    /// <returns></returns>
    protected virtual IEnumerable<PropertyInfo> GetProperties(Type instanceType)
        => ReflectionHelper.GetProperties(instanceType);
    /// <summary>
    /// 获取所有字段
    /// </summary>
    /// <param name="instanceType"></param>
    /// <returns></returns>
    protected virtual IEnumerable<FieldInfo> GetFields(Type instanceType)
        => ReflectionHelper.GetFields(instanceType);
    /// <summary>
    /// 默认反射成员实例
    /// </summary>
    public static readonly DefaultReflectionMember Default = new();
}
