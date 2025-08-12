using PocoEmit;
using PocoEmit.Configuration;
using System;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// DI注入扩展方法
/// </summary>
public static class PocoEmitDIServices
{
    #region UseConverter
    /// <summary>
    /// 注册转化接口
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection UseConverter(this IServiceCollection services)
    {
        return services.AddSingletonTypedFactory(typeof(IPocoConverter<,>), GetGenericConverter);
    }
    /// <summary>
    /// 注册转化接口
    /// </summary>
    /// <param name="services"></param>
    /// <param name="poco"></param>
    /// <returns></returns>
    public static IServiceCollection UseConverter(this IServiceCollection services, IPoco poco)
    {
        return services.AddSingletonTypedFactory(typeof(IPocoConverter<,>), (sp, converterType) => poco.GetGenericConverter(converterType));
    }
    /// <summary>
    /// 注册转化接口
    /// </summary>
    /// <param name="services"></param>
    /// <param name="poco"></param>
    /// <param name="serviceKey"></param>
    /// <returns></returns>
    public static IServiceCollection UseConverter(this IServiceCollection services, IPoco poco, object serviceKey)
    {
        return services.AddKeyedTypedFactorySingleton(typeof(IPocoConverter<,>), serviceKey, (sp, key, converterType) => poco.GetGenericConverter(converterType));
    }
    #endregion
    #region UseCopier
    /// <summary>
    /// 注册转化接口
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection UseCopier(this IServiceCollection services)
    {
        return services.AddSingletonTypedFactory(typeof(IPocoCopier<,>), GetGenericCopier);
    }
    /// <summary>
    /// 注册转化接口
    /// </summary>
    /// <param name="services"></param>
    /// <param name="mapper"></param>
    /// <returns></returns>
    public static IServiceCollection UseCopier(this IServiceCollection services, IMapperOptions mapper)
    {
        return services.AddSingletonTypedFactory(typeof(IPocoCopier<,>), (sp, CopierType) => mapper.GetGenericCopier(CopierType));
    }
    /// <summary>
    /// 注册转化接口
    /// </summary>
    /// <param name="services"></param>
    /// <param name="mapper"></param>
    /// <param name="serviceKey"></param>
    /// <returns></returns>
    public static IServiceCollection UseCopier(this IServiceCollection services, IMapperOptions mapper, object serviceKey)
    {
        return services.AddKeyedTypedFactorySingleton(typeof(IPocoCopier<,>), serviceKey, (sp, key, CopierType) => mapper.GetGenericCopier(CopierType));
    }
    #endregion
    #region GetGenericConverter
    /// <summary>
    /// 获取类型转化(用于IOC注入)
    /// </summary>
    /// <param name="sp"></param>
    /// <param name="converterType"></param>
    /// <returns></returns>
    public static object GetGenericConverter(IServiceProvider sp, Type converterType)
    {
        IMapper mapper = sp.GetService<IMapper>() ?? Mapper.Default;
        return GetGenericConverter(mapper, converterType);
    }
    /// <summary>
    /// 获取类型转化(用于IOC注入)
    /// </summary>
    /// <param name="poco"></param>
    /// <param name="converterType"></param>
    /// <returns></returns>
    public static object GetGenericConverter(this IPoco poco, Type converterType)
    {
        if (!ReflectionHelper.IsGenericType(converterType, typeof(IPocoConverter<,>)))
            return null;
        var argumentsType = converterType.GetGenericArguments();
        return poco.GetObjectConverter(argumentsType[0], argumentsType[1]);
    }
    #endregion
    #region GetGenericCopier
    /// <summary>
    /// 获取复制器(用于IOC注)
    /// </summary>
    /// <param name="sp"></param>
    /// <param name="converterType"></param>
    /// <returns></returns>
    public static object GetGenericCopier(IServiceProvider sp, Type converterType)
    {
        IMapper mapper = sp.GetService<IMapper>() ?? Mapper.Default;
        return mapper.GetGenericCopier(converterType);
    }
    /// <summary>
    /// 获取复制器(用于IOC注)
    /// </summary>
    /// <param name="mapper"></param>
    /// <param name="copierType"></param>
    /// <returns></returns>
    public static object GetGenericCopier(this IMapper mapper, Type copierType)
    {
        if (!ReflectionHelper.IsGenericType(copierType, typeof(IPocoCopier<,>)))
            return null;
        
        var argumentsType = copierType.GetGenericArguments();
        return mapper.GetObjectCopier(argumentsType[0], argumentsType[1]);
    }
    #endregion
}
