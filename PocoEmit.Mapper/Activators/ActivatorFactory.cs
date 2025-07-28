using PocoEmit.Collections;
using PocoEmit.Configuration;
using System;
#if (NETSTANDARD1_1 || NETSTANDARD1_3)
using System.Reflection;
#endif

namespace PocoEmit.Activators;

/// <summary>
/// 激活器工厂
/// </summary>
/// <param name="options"></param>
public sealed class ActivatorFactory(IMapperOptions options)
    : CacheBase<Type, IEmitActivator>(options)
{
    #region 配置
    private readonly IMapperOptions _options = options;
    /// <summary>
    /// Emit配置
    /// </summary>
    public IMapperOptions Options
        => _options;
    #endregion
    #region CacheBase
    /// <inheritdoc />
    protected override IEmitActivator CreateNew(Type key)
    {
#if (NETSTANDARD1_1 || NETSTANDARD1_3)
         var isValueType = key.GetTypeInfo().IsValueType;
#else
        var isValueType = key.IsValueType;
#endif
        if (isValueType)
            return new TypeActivator(key);
        var constructor = ReflectionHelper.GetConstructor(key, static parameters => parameters.Length == 0);
        if (constructor is null)
            return null;
        return new ConstructorActivator(key, constructor);
    }
    #endregion
}
