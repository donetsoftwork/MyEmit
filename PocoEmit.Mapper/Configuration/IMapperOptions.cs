using PocoEmit.Activators;
using PocoEmit.Builders;
using PocoEmit.Collections;
using PocoEmit.Copies;
using PocoEmit.Maping;
using PocoEmit.Reflection;
using System;
using System.Linq.Expressions;

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
{
    /// <summary>
    /// 复制器构造器
    /// </summary>
    CopierBuilder CopierBuilder { get; }
    /// <summary>
    /// 默认成员匹配
    /// </summary>
    IMemberMatch DefaultMatcher { get; set; }
    /// <summary>
    /// 获取Emit类型激活器
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    IEmitActivator GetEmitActivator(PairTypeKey key);
    /// <summary>
    /// 获取转化后成员检查配置
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    Delegate GetCheckMembers(PairTypeKey key);
    /// <summary>
    /// 是否基础类型
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    bool CheckPrimitive(Type type);
    /// <summary>
    /// 获取默认值构建器
    /// </summary>
    /// <param name="destType"></param>
    /// <returns></returns>
    IEmitBuilder GetDefaultValueBuilder(Type destType);
    /// <summary>
    /// 构造默认值
    /// </summary>
    /// <param name="destType"></param>
    /// <returns></returns>
    Expression CreateDefault(Type destType);
}
