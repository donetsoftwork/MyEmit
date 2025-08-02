using PocoEmit.Collections;
using PocoEmit.Configuration;
using System;
#if (NETSTANDARD1_1 || NETSTANDARD1_3 || NETSTANDARD1_6)
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
        var constructor = _options.GetConstructor(key);
        if (constructor is not null)
        {
            var parameters = constructor.GetParameters();
            if(parameters.Length == 0)
                return new ConstructorActivator(key, constructor);
            return new ParameterConstructorActivator(_options, key, constructor, parameters);
        }
#if (NETSTANDARD1_1 || NETSTANDARD1_3 || NETSTANDARD1_6)
        var isValueType = key.GetTypeInfo().IsValueType;
#else
        var isValueType = key.IsValueType;
#endif
        if (isValueType)
            return new TypeActivator(key);
        return null;
    }
    #endregion
}
