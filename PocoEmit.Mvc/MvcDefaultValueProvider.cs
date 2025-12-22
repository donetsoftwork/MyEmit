using Hand.Creational;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using PocoEmit.Configuration;
using PocoEmit.Members;
using PocoEmit.ServiceProvider;
using System.Linq.Expressions;
using System.Reflection;

namespace PocoEmit.Mvc;

/// <summary>
/// Web默认值构造器
/// </summary>
/// <param name="options"></param>
/// <param name="providerBuilder"></param>
public class MvcDefaultValueProvider(IMapperOptions options, IServiceProviderBuilder providerBuilder)
    : ServiceDefaultValueProvider(options, providerBuilder)
{
    #region DefaultValueBuilder
    /// <inheritdoc />
    internal protected override ICreator<Expression> BuildCore(ConstructorParameterMember parameter)
    {
        var info = parameter.Parameter;
        // 参数支持FromKeyedServicesAttribute
        var keyed = info.GetCustomAttribute<FromKeyedServicesAttribute>();
        if (keyed is not null)
            return BuildFromKeyed(parameter.ValueType, keyed);
        // 参数支持FromServicesAttribute
        if (info.IsDefined(typeof(FromServicesAttribute), false))
            return BuildFromService(parameter.ValueType);
        return BuildCore(parameter.ValueType);
    }
    #endregion
}
