using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace PocoEmit.Mvc;

/// <summary>
/// Mvc默认值相关扩展方法
/// </summary>
public static class PocoMvcDefaultServices
{
    /// <summary>
    /// 使用上下文容器
    /// </summary>
    /// <param name="mapper"></param>
    /// <param name="root"></param>
    /// <param name="accessor"></param>
    /// <returns></returns>
    public static MvcDefaultValueProvider UseContext(this IMapper mapper, IServiceProvider root, IHttpContextAccessor accessor)
    {
        var options = (Mapper)mapper;
        var bulder = new ContextBuilder(root, accessor);
        var defaultValue = new MvcDefaultValueProvider(options, bulder);
        options.DefaultValueProvider = defaultValue;
        options.UseProviderDefault(bulder);
        return defaultValue;
    }
}
