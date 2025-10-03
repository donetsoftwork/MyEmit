using PocoEmit.Configuration;
using PocoEmit.Copies;
using System;
using System.Linq.Expressions;
#if (NETSTANDARD1_1 || NETSTANDARD1_3 || NETSTANDARD1_6)
using System.Reflection;
#endif

namespace PocoEmit;

/// <summary>
/// 复制配置扩展方法
/// </summary>
public static partial class MapperServices
{
    /// <summary>
    /// 设置委托来复制
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TDest"></typeparam>
    /// <param name="mapper"></param>
    /// <param name="copyAction"></param>
    /// <returns></returns>
    public static IPoco UseCopyAction<TSource, TDest>(this IMapper mapper, Expression<Action<TSource, TDest>> copyAction)
    {
        var key = new PairTypeKey(typeof(TSource), typeof(TDest));
        mapper.Configure(key, new ActionCopier((IMapperOptions)mapper, copyAction));
        return mapper;
    }
    /// <summary>
    /// 添加静态复制方法
    /// </summary>
    /// <typeparam name="TCopier"></typeparam>
    /// <param name="mapper"></param>
    /// <returns></returns>
    public static IMapper UseStaticCopier<TCopier>(this IMapper mapper)
        => UseStaticCopier(mapper, typeof(TCopier));
    /// <summary>
    /// 添加静态复制方法
    /// </summary>
    /// <param name="mapper"></param>
    /// <param name="converterType"></param>
    /// <returns></returns>
    public static IMapper UseStaticCopier(this IMapper mapper, Type converterType)
    {
#if (NETSTANDARD1_1 || NETSTANDARD1_3 || NETSTANDARD1_6)
        var methods = converterType.GetTypeInfo().DeclaredMethods;
#else
        var methods = converterType.GetMethods();
#endif
        foreach (var method in methods)
        {
            var parameters = method.GetParameters();
            if (method.DeclaringType == converterType && method.IsStatic && method.ReturnType == typeof(void) && parameters.Length == 2)
            {
                MethodCopier converter = new(null, method);
                PairTypeKey key = new(parameters[0].ParameterType, parameters[1].ParameterType);
                mapper.Configure(key, converter);
            }
        }
        return mapper;
    }
    /// <summary>
    /// 添加实例复制方法
    /// </summary>
    /// <param name="mapper"></param>
    /// <param name="instance"></param>
    /// <returns></returns>
    public static IMapper UseCopier(this IMapper mapper, object instance)
    {
        Type converterType = instance.GetType();
#if (NETSTANDARD1_1 || NETSTANDARD1_3 || NETSTANDARD1_6)
        var methods = converterType.GetTypeInfo().DeclaredMethods;
#else
        var methods = converterType.GetMethods();
#endif
        var target = Expression.Constant(instance);
        foreach (var method in methods)
        {
            var parameters = method.GetParameters();
            if (method.DeclaringType == converterType && !method.IsStatic && method.ReturnType == typeof(void) && parameters.Length == 2)
            {
                MethodCopier converter = new(target, method);
                PairTypeKey key = new(parameters[0].ParameterType, parameters[1].ParameterType);
                mapper.Configure(key, converter);
            }
        }
        return mapper;
    }
    #region SetCopy
    /// <summary>
    /// 设置委托来复制
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TDest"></typeparam>
    /// <param name="mapper"></param>
    /// <param name="copy"></param>
    /// <returns></returns>
    public static ActionCopier SetCopy<TSource, TDest>(this IMapper mapper, Expression<Action<TSource, TDest>> copy)
        where TDest : class
    {
        var key = new PairTypeKey(typeof(TSource), typeof(TDest));
        var copier = new ActionCopier((IMapperOptions)mapper, copy);
        mapper.Set(key, copier);
        return copier;
    }
    /// <summary>
    /// 尝试设置委托来复制不覆盖
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TDest"></typeparam>
    /// <param name="mapper"></param>
    /// <param name="copy"></param>
    /// <returns></returns>
    public static IEmitCopier TrySetCopy<TSource, TDest>(this IMapper mapper, Expression<Action<TSource, TDest>> copy)
        where TDest : class
    {
        var option = (IMapperOptions)mapper;
        var key = new PairTypeKey(typeof(TSource), typeof(TDest));
        if (option.TryGetCache(key, out IEmitCopier value0) && value0 is not null)
            return value0;
        var copier = new ActionCopier(option, copy);
        mapper.Set(key, copier);
        return copier;
    }
    #endregion
}
