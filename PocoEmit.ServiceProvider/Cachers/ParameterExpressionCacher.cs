using Hand.Cache;
using Hand.Creational;
using Microsoft.Extensions.DependencyInjection;
using PocoEmit.Builders;
using PocoEmit.Collections;
using PocoEmit.Members;
using System.Linq.Expressions;
using System.Reflection;

namespace PocoEmit.ServiceProvider.Cachers;

/// <summary>
/// 参数默认值缓存
/// </summary>
/// <param name="builder"></param>
public class ParameterExpressionCacher(ServiceDefaultValueBuilder builder)
    : CacheFactoryBase<ConstructorParameterMember, ICreator<Expression>>(builder)
{
    #region 配置
    private readonly ServiceDefaultValueBuilder _builder = builder;
    /// <summary>
    /// 默认值构造器
    /// </summary>
    public ServiceDefaultValueBuilder Builder
        => _builder;
    #endregion
    /// <inheritdoc />
    protected override ICreator<Expression> CreateNew(in ConstructorParameterMember key)
    {
        var parameter = key.Parameter;
        var parameterType = key.ValueType;
        var keyed = parameter.GetCustomAttribute<FromKeyedServicesAttribute>();
        if (keyed is null)
        {
            _builder.TryGetConfig(key.ValueType, out var serviceBuilder);
            return serviceBuilder;
        }
        return _builder.BuildFromKeyed(parameterType, keyed);
    }
}
