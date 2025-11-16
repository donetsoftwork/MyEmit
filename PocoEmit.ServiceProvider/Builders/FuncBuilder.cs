using Hand.Creational;
using PocoEmit.Builders;
using PocoEmit.Configuration;
using System;
using System.Linq.Expressions;

namespace PocoEmit.ServiceProvider.Builders;

/// <summary>
/// 委托构造器
/// </summary>
/// <typeparam name="TValue"></typeparam>
/// <param name="options"></param>
/// <param name="provider"></param>
/// <param name="func"></param>
public class FuncBuilder<TValue>(IMapperOptions options, IServiceProviderBuilder provider, Expression<Func<IServiceProvider, TValue>> func)
        : ICreator<Expression>
{
    #region 配置
    /// <summary>
    /// 映射配置
    /// </summary>
    private readonly IMapperOptions _options = options;
    private readonly IServiceProviderBuilder _provider = provider;
    private readonly Expression<Func<IServiceProvider, TValue>> _func = func;
    /// <summary>
    /// 映射配置
    /// </summary>
    public IMapperOptions Options
        => _options;
    /// <summary>
    /// 定位构造器
    /// </summary>
    public IServiceProviderBuilder Provider
        => _provider;
    /// <summary>
    /// 工厂方法
    /// </summary>
    public Expression<Func<IServiceProvider, TValue>> Func 
        => _func;
    #endregion
    /// <inheritdoc />
    public Expression Create()
    {
        var provider = _provider.CreateProvider();
        var builder = provider.Builder;
        builder.Add(_options.Call(_func, provider.Provider));
        return builder.Create();
    }
}
