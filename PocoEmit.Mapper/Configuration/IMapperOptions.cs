using PocoEmit.Activators;
using PocoEmit.Builders;
using PocoEmit.Collections;
using PocoEmit.Converters;
using PocoEmit.Copies;
using PocoEmit.Maping;
using PocoEmit.Members;
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
    , ISettings<Type, IBuilder<Expression>>
    , ISettings<MemberInfo, IBuilder<Expression>>
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
    /// 默认值构造器
    /// </summary>
    DefaultValueBuilder DefaultValueBuilder { get; }
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
    ///// <summary>
    ///// 获取默认值构建器
    ///// </summary>
    ///// <param name="destType"></param>
    ///// <returns></returns>
    //IBuilder<Expression> GetDefaultValueBuilder(Type destType);
    ///// <summary>
    ///// 构造默认值
    ///// </summary>
    ///// <param name="destType"></param>
    ///// <returns></returns>
    //Expression CreateDefault(Type destType);
    ///// <summary>
    ///// 构造默认值
    ///// </summary>
    ///// <param name="member"></param>
    ///// <returns></returns>
    //IBuilder<Expression> CreateDefault(IMember member);
    /// <summary>
    /// 被缓存状态
    /// </summary>
    ComplexCached Cached { get; }
}
