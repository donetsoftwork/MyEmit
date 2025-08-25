using System;
using System.Collections.Generic;
using System.Linq;

namespace PocoEmit.Enums;

/// <summary>
/// 枚举配置基类
/// </summary>
/// <typeparam name="TField"></typeparam>
/// <param name="enumType"></param>
/// <param name="underType"></param>
/// <param name="capacity"></param>
public abstract class EnumBundleBase<TField>(Type enumType, Type underType, int capacity)
    : IEnumBundle
    where TField : EnumField
{
    #region 配置
    private readonly Type _enumType = enumType;
    private readonly Type _underType = underType;
    /// <summary>
    /// 字段
    /// </summary>
    protected readonly List<TField> _fields = new(capacity);
    /// <inheritdoc />
    public Type EnumType
        => _enumType;
    /// <inheritdoc />
    public Type UnderType
        => _underType;
    /// <summary>
    /// 字段
    /// </summary>
    public List<TField> Fields
        => _fields;
    /// <inheritdoc />
    public virtual bool HasFlag
        => false;
    IEnumerable<IEnumField> IEnumBundle.Fields
        => _fields;
    #endregion
    /// <summary>
    /// 添加字段
    /// </summary>
    /// <param name="field"></param>
    public void AddField(TField field)
        => _fields.Add(field);
    /// <summary>
    /// 按名获取字段
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public IEnumField GetFieldByName(string name)
    {
        if(string.IsNullOrEmpty(name))
            return null;
        return _fields.FirstOrDefault(f => f.MatchMember(name)) ?? _fields.FirstOrDefault(f => f.Match(name));
    }
}
