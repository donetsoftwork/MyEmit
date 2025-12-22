using Hand.Creational;
using PocoEmit.Builders;
using System;
using System.Linq.Expressions;

namespace PocoEmit.ServiceProvider.Builders;

/// <summary>
/// 服务表达式构造器
/// </summary>
/// <param name="provider"></param>
/// <param name="serviceType"></param>
/// <param name="serviceKey"></param>
public class KeyedServiceBuilder(IServiceProviderBuilder provider, Type serviceType, object serviceKey)
    : ICreator<Expression>
{
    #region 配置
    private readonly IServiceProviderBuilder _provider = provider;
    private readonly Type _serviceType = serviceType;
    private readonly ConstantExpression _serviceKey = Expression.Constant(serviceKey);
    /// <summary>
    /// 定位构造器
    /// </summary>
    public IServiceProviderBuilder Provider
        => _provider;
    /// <summary>
    /// 服务类型
    /// </summary>
    public Type ServiceType
        => _serviceType;
    /// <summary>
    /// 服务键
    /// </summary>
    public object ServiceKey
        => _serviceKey;
    #endregion
    /// <inheritdoc />
    public Expression Create()
    {
        var builder = new EmitBuilder();
        var service = Create(builder);
        builder.Add(service);
        return builder.Create();
    }
    /// <summary>
    /// 构造表达式构造器
    /// </summary>
    /// <returns></returns>
    public Expression Create(EmitBuilder builder)
    {
        var provider = _provider.CreateKeyed();
        builder.Join(provider.Builder);
        return EmitProviderHelper.CallGetKeydService(builder, provider.Provider, _serviceType, _serviceKey);
    }
}
