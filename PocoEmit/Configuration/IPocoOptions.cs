using Hand.Cache;
using Hand.Reflection;
using PocoEmit.Builders;
using PocoEmit.Collections;
using PocoEmit.Converters;
using PocoEmit.Reflection;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace PocoEmit.Configuration;

/// <summary>
/// 简单对象配置接口
/// </summary>
public interface IPocoOptions
    : IPoco
    , ICacher<PairTypeKey, IEmitConverter>
    , ICacher<Type, MemberBundle>
{
    /// <summary>
    /// 转换器构造器
    /// </summary>
    ConvertBuilder ConvertBuilder { get; }
    /// <summary>
    /// 转换器
    /// </summary>
    IEnumerable<IEmitConverter> Converters { get; }
    /// <summary>
    /// 反射获取成员
    /// </summary>
    IReflectionMember ReflectionMember { get; }
    /// <summary>
    /// 调用
    /// </summary>
    /// <param name="lambda"></param>
    /// <param name="arguments"></param>
    /// <returns></returns>
    Expression Call(LambdaExpression lambda, params Expression[] arguments);
}
