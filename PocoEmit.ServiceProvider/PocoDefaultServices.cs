using Hand.Creational;
using PocoEmit.ServiceProvider;
using PocoEmit.ServiceProvider.Builders;
using System;
using System.Linq.Expressions;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// 默认值相关扩展方法
/// </summary>
public static partial class PocoDefaultServices
{
    ///// <summary>
    ///// 使用容器初始化默认值
    ///// </summary>
    ///// <typeparam name="TService"></typeparam>
    ///// <param name="builder"></param>
    ///// <returns></returns>
    //public static ServiceDefaultValueBuilder UseDefault<TService>(this ServiceDefaultValueBuilder builder)
    //    => UseDefault(builder, typeof(TService), new ServiceBuilder(builder.ProviderBuilder, typeof(TService)));
    ///// <summary>
    ///// 使用容器初始化默认值
    ///// </summary>
    ///// <typeparam name="TService"></typeparam>
    ///// <param name="builder"></param>
    ///// <param name="serviceKey"></param>
    ///// <returns></returns>
    //public static ServiceDefaultValueBuilder UseDefault<TService>(this ServiceDefaultValueBuilder builder, object serviceKey)
    //    => UseDefault(builder, typeof(TService), new KeyedServiceBuilder(builder.ProviderBuilder, typeof(TService), serviceKey));
    ///// <summary>
    ///// 使用委托构造默认值
    ///// </summary>
    ///// <typeparam name="TService"></typeparam>
    ///// <param name="builder"></param>
    ///// <param name="func"></param>
    ///// <returns></returns>
    //public static ServiceDefaultValueBuilder UseDefault<TService>(this ServiceDefaultValueBuilder builder, Expression<Func<IServiceProvider, TService>> func)
    //    => UseDefault(builder, typeof(TService), new ProviderFuncBuilder<TService>(builder.Options, builder.ProviderBuilder, func));
    ///// <summary>
    ///// 使用默认值
    ///// </summary>
    ///// <param name="builder"></param>
    ///// <param name="type"></param>
    ///// <param name="value"></param>
    ///// <returns></returns>
    //internal static ServiceDefaultValueBuilder UseDefault(this ServiceDefaultValueBuilder builder, Type type, ICreator<Expression> value)
    //{
    //    builder.Options.Set(type, value);
    //    return builder;
    //}
}
