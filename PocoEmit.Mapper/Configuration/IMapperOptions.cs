using Hand.Cache;
using Hand.Configuration;
using Hand.Creational;
using Hand.Reflection;
using PocoEmit.Activators;
using PocoEmit.Builders;
using PocoEmit.Converters;
using PocoEmit.Copies;
using PocoEmit.Maping;
using PocoEmit.Reflection;
using System;
using System.Linq.Expressions;
using System.Reflection;

namespace PocoEmit.Configuration;

/// <summary>
///  映射配置接口
/// </summary>
public interface IMapperOptions
    : IPocoOptions
    , IMapper
    , ICacher<PairTypeKey, IEmitCopier>
    , ICacher<Type, bool>
    , IReflectionConstructor
    , ICacher<PairTypeKey, IEmitContextConverter>
    , IConfiguration<Type, ICreator<Expression>>
    , IConfiguration<MemberInfo, ICreator<Expression>>
{
    /// <summary>
    /// 复制器构造器
    /// </summary>
    CopierBuilder CopierBuilder { get; }
    /// <summary>
    /// 默认成员匹配
    /// </summary>
    IMemberMatch DefaultMatcher { get; }
    /// <summary>
    /// 默认值提供器
    /// </summary>
    DefaultValueProvider DefaultValueProvider { get; }
    /// <summary>
    /// 获取Emit类型激活器
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    IEmitActivator GetEmitActivator(in PairTypeKey key);
    /// <summary>
    /// 获取转化后成员检查配置
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    Delegate GetCheckMembers(in PairTypeKey key);
    /// <summary>
    /// 是否基础类型
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    bool CheckPrimitive(Type type);
    /// <summary>
    /// 被缓存状态
    /// </summary>
    ComplexCached Cached { get; }
}
