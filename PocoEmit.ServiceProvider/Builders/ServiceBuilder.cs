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
public class ServiceBuilder(IServiceProviderBuilder provider, Type serviceType)
    : ICreator<Expression>
{
    #region 配置
    private readonly IServiceProviderBuilder _provider = provider;    
    private readonly Type _serviceType = serviceType;
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
        var provider = _provider.CreateProvider();
        builder.Join(provider.Builder);
        return EmitProviderHelper.CallGetService(builder, provider.Provider, _serviceType);
    }
}
