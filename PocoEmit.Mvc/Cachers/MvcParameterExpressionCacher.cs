using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using PocoEmit.Builders;
using PocoEmit.Collections;
using PocoEmit.Members;
using System.Linq.Expressions;
using System.Reflection;

namespace PocoEmit.Mvc.Cachers;

/// <summary>
/// Mvc参数默认值缓存
/// </summary>
/// <param name="builder"></param>
public class MvcParameterExpressionCacher(MvcDefaultValueBuilder builder)
    : CacheBase<ConstructorParameterMember, IBuilder<Expression>>(builder)
{
    #region 配置
    private readonly MvcDefaultValueBuilder _builder = builder;
    /// <summary>
    /// 默认值构造器
    /// </summary>
    public MvcDefaultValueBuilder Builder
        => _builder;
    #endregion
    /// <inheritdoc />
    protected override IBuilder<Expression> CreateNew(in ConstructorParameterMember key)
    {
        var parameter = key.Parameter;
        var parameterType = key.ValueType;
        var keyed = parameter.GetCustomAttribute<FromKeyedServicesAttribute>();
        if (keyed is null)
        {
            if (_builder.TryGetConfig(key.ValueType, out var serviceBuilder))
                return serviceBuilder;
            if (parameter.IsDefined(typeof(FromServicesAttribute), false))
                return _builder.BuildFromServices(parameterType);
            return null;
        }
        return _builder.BuildFromKeyed(parameterType, keyed);
    }
}
