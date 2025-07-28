using PocoEmit.Collections;
using System;
using System.Collections.Generic;
using System.Reflection;
#if NET7_0_OR_GREATER || NETSTANDARD2_0_OR_GREATER
using System.Collections.Frozen;
#else
using System.Linq;
#endif

namespace PocoEmit.Reflection;

/// <summary>
/// 默认反射成员
/// </summary>
public class DefaultReflectionMember(IEqualityComparer<string> comparer, bool includeField = true)
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
    private readonly IEqualityComparer<string> _comparer = comparer;
    private readonly bool _includeField = includeField;
    /// <summary>
    /// 成员名比较器
    /// </summary>
    public IEqualityComparer<string> Comparer
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
        List<MemberInfo> readMembers = [];
        List<MemberInfo> writeMembers = [];
        foreach (var property in GetProperties(instanceType))
        {
            if (property.CanRead)
                readMembers.Add(property);
            if (property.CanWrite)
                writeMembers.Add(property);
        }
        if(_includeField)
        {
            var fields = ReflectionHelper.GetFields(instanceType);
            readMembers.AddRange(fields);
            writeMembers.AddRange(fields);
        }
        return new MemberBundle(CheckMembers(readMembers), CheckMembers(writeMembers));
    }
    /// <summary>
    /// 检查成员
    /// </summary>
    /// <param name="list"></param>
    /// <returns></returns>
    protected virtual IDictionary<string, MemberInfo> CheckMembers(IEnumerable<MemberInfo> list)
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
