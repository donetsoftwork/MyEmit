using Hand.Configuration;
using Hand.Creational;
using PocoEmit;
using PocoEmit.Builders;
using PocoEmit.Collections;
using PocoEmit.ServiceProvider;
using System;
using System.Linq.Expressions;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// 默认值相关扩展方法
/// </summary>
public static partial class PocoDefaultServices
{
    /// <summary>
    /// 使用容器单例
    /// </summary>
    /// <param name="mapper"></param>
    /// <param name="provider"></param>
    /// <returns></returns>
    public static ServiceDefaultValueBuilder UseSingleton(this IMapper mapper, IServiceProvider provider)
        => UseProvider((Mapper)mapper, new SingletonBuilder(provider));
    /// <summary>
    /// 使用容器作用域
    /// </summary>
    /// <param name="mapper"></param>
    /// <param name="root"></param>
    /// <returns></returns>
    public static ServiceDefaultValueBuilder UseScope(this IMapper mapper, IServiceProvider root)
        => UseProvider((Mapper)mapper, new ScopeBuilder(root));
    /// <summary>
    /// 使用定位器
    /// </summary>
    /// <param name="mapper"></param>
    /// <param name="builder"></param>
    /// <returns></returns>
    private static ServiceDefaultValueBuilder UseProvider(Mapper mapper, IServiceProviderBuilder builder)
    {
        var defaultValue = new ServiceDefaultValueBuilder(mapper, builder);
        mapper.DefaultValueBuilder = defaultValue;
        UseProviderDefault(mapper, builder);
        return defaultValue;
    }
    /// <summary>
    /// 设置定位器默认值
    /// </summary>
    /// <param name="mapper"></param>
    /// <param name="builder"></param>
    internal static void UseProviderDefault(this Mapper mapper, IServiceProviderBuilder builder)
    {
        IConfigure<Type, ICreator<Expression>> config = mapper;
        var provider = builder.CreateProvider();
        config.Set(provider.ProviderType, provider);
        var keyed = builder.CreateKeyed();
        config.Set(keyed.ProviderType, keyed);
    }
}
