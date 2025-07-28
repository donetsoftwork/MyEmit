using System;
using System.Reflection;

namespace PocoEmit.Members;

/// <summary>
/// 成员操作基类
/// </summary>
/// <typeparam name="TMemberType"></typeparam>
/// <param name="instanceType"></param>
/// <param name="member"></param>
/// <param name="name"></param>
/// <param name="valueType"></param>
public abstract class MemberAccessor<TMemberType>(Type instanceType, TMemberType member, string name, Type valueType)
    : IMember
    where TMemberType : MemberInfo    
{
    #region 配置
    private readonly Type _instanceType = instanceType;
    /// <inheritdoc />
    public Type InstanceType
        => _instanceType;
    /// <summary>
    /// 成员
    /// </summary>
    protected readonly TMemberType _member = member;
    private readonly string _name = name;
    /// <summary>
    /// 值类型
    /// </summary>
    protected readonly Type _valueType = valueType;
    /// <summary>
    /// 成员
    /// </summary>
    public TMemberType Member
        => _member;
    /// <inheritdoc />
    public string Name 
        => _name;
    /// <inheritdoc />
    public Type ValueType
        => _valueType;
    #endregion
}
