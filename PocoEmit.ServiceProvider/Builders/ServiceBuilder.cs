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
        var provider = _provider.CreateProvider();
        var builder = provider.Builder;
        builder.Add(EmitProviderHelper.CallGetService(provider.Provider, _serviceType));
        return builder.Create();
    }
}
