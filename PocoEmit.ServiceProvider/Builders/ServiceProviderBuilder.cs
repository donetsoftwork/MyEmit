using PocoEmit.Builders;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace PocoEmit.ServiceProvider.Builders;

/// <summary>
/// 构造定位器
/// </summary>
/// <param name="providerType"></param>
/// <param name="provider"></param>
/// <param name="builder"></param>
public class ServiceProviderBuilder(Type providerType, Expression provider, EmitBuilder builder)
    : IBuilder<Expression>
{
    #region 配置
    private readonly Type _providerType = providerType;
    private readonly Expression _provider = provider;
    private readonly EmitBuilder _builder = builder;
    /// <summary>
    /// 定位器类型
    /// </summary>
    public Type ProviderType
        => _providerType;
    /// <summary>
    /// 定位器
    /// </summary>
    public Expression Provider 
        => _provider;
    /// <summary>
    /// 构造器
    /// </summary>
    public EmitBuilder Builder
        => _builder;
    #endregion
    #region IBuilder<Expression>
    /// <inheritdoc />
    public Expression Build()
    {
        var expressions = _builder.Expressions;
        var last = expressions.LastOrDefault();
        if (last is null)
            return _provider;
        if(last.Type == _providerType)
            return _builder.Build();
        var builder = new EmitBuilder(_builder);
        builder.Add(_provider);
        return builder.Build();
    }
    #endregion
}
